[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]

namespace Example.FormsApp
{
    using Example.FormsApp.Services;

    using Smart.Resolver;

    public partial class App
    {
        public App()
        {
            InitializeComponent();

            var resolver = CreateResolver();

            MainPage = resolver.Get<MainPage>();
        }

        private SmartResolver CreateResolver()
        {
            var config = new ResolverConfig()
                .UseAutoBinding()
                .UseArrayBinding()
                .UseAssignableBinding()
                .UsePropertyInjector()
                .UseBindingContextInjectProcessor()
                .UseBindingContextInitializeProcessor();

            config.Bind<CounterService>().ToSelf().InSingletonScope();

            return config.ToResolver();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
