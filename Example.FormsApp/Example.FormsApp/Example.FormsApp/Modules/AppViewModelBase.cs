namespace Example.FormsApp.Modules
{
    using Smart.Forms.ViewModels;
    using Smart.Navigation;

    public class AppViewModelBase : ViewModelBase, INavigatorAware, INavigationEventSupport
    {
        public INavigator Navigator { get; set; }

        public ApplicationState ApplicationState { get; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            System.Diagnostics.Debug.WriteLine($"{GetType()} is Disposed");
        }

        public AppViewModelBase(ApplicationState applicationState)
            : base(applicationState)
        {
            ApplicationState = applicationState;
        }

        public virtual void OnNavigatingFrom(INavigationContext context)
        {
        }

        public virtual void OnNavigatingTo(INavigationContext context)
        {
        }

        public virtual void OnNavigatedTo(INavigationContext context)
        {
        }
    }
}
