[assembly: Xamarin.Forms.Dependency(typeof(Example.FormsApp.Droid.Services.MessageService2))]

namespace Example.FormsApp.Droid.Services
{
    using Example.FormsApp.Services;

    public class MessageService2 : IMessageService2
    {
        public string GetMessage() => "by Dependency Service.";
    }
}