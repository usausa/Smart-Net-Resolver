namespace Example.FormsApp
{
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
            IncrementCommand = MakeDelegateCommand(() => Counter.Value = CounterService.IncrementAndGet());
        }

        public void Initialize()
        {
        }
    }
}
