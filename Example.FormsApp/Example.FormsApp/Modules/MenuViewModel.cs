namespace Example.FormsApp.Modules
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;

    using Example.FormsApp.Services;

    using Smart.ComponentModel;
    using Smart.Forms.Components;
    using Smart.Forms.Input;
    using Smart.Resolver.Attributes;

    public class MenuViewModel : AppViewModelBase
    {
        public static MenuViewModel DesignInstance { get; } = null; // For design

        [Inject]
        public IMessageService MessageService { get; set; } // Property injection sample

        public NotificationValue<int> Counter { get; } = new NotificationValue<int>();

        public AsyncCommand MessageCommand { get; }

        public AsyncCommand Message2Command { get; }

        public DelegateCommand IncrementCommand { get; }

        public MenuViewModel(
            ApplicationState applicationState,
            IDialogService dialogService,
            CounterService counterService,
            IMessageService2 messageService2)
            : base(applicationState)
        {
            MessageCommand = MakeAsyncCommand(() => dialogService.DisplayAlert("Message", MessageService.GetMessage(), "ok"));
            Message2Command = MakeAsyncCommand(() => dialogService.DisplayAlert("Message", messageService2.GetMessage(), "ok"));
            IncrementCommand = MakeDelegateCommand(counterService.Increment);

            Disposables.Add(Observable
                .FromEvent<EventHandler<CounterEventArgs>, CounterEventArgs>(
                    h => (s, e) => h(e),
                    h => counterService.Changed += h,
                    h => counterService.Changed -= h)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(e => Counter.Value = e.Value));
        }
    }
}
