namespace Example.FormsApp
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;

    using Example.FormsApp.Services;

    using Smart;
    using Smart.ComponentModel;
    using Smart.Forms.Input;
    using Smart.Forms.ViewModels;
    using Smart.Resolver.Attributes;

    public class MainPageViewModel : ViewModelBase, IInitializable
    {
        [Inject]
        public CounterService CounterService { get; set; }

        public NotificationValue<int> Counter { get; } = new NotificationValue<int>();

        public DelegateCommand IncrementCommand { get; }

        public MainPageViewModel()
        {
            IncrementCommand = MakeDelegateCommand(() => CounterService.Increment());
        }

        public void Initialize()
        {
            Disposables.Add(Observable
                .FromEvent<EventHandler<CounterEventArgs>, CounterEventArgs>(
                    h => (s, e) => h(e),
                    h => CounterService.Changed += h,
                    h => CounterService.Changed -= h)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(e => Counter.Value = e.Value));
        }
    }
}
