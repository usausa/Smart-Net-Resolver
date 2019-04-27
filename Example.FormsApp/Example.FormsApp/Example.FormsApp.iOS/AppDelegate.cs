namespace Example.FormsApp.iOS
{
    using Example.FormsApp.iOS.Services;
    using Example.FormsApp.Services;

    using Foundation;

    using Smart.Resolver;

    using UIKit;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new ComponentProvider()));

            return base.FinishedLaunching(app, options);
        }

        private sealed class ComponentProvider : IComponentProvider
        {
            public void RegisterComponents(ResolverConfig config)
            {
                config.Bind<IMessageService>().To<MessageService>().InSingletonScope();
            }
        }
    }
}
