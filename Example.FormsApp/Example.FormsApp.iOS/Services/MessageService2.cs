[assembly: Xamarin.Forms.Dependency(typeof(Example.FormsApp.iOS.Services.MessageService2))]
namespace Example.FormsApp.iOS.Services
{
    using Example.FormsApp.Services;

    public class MessageService2 : IMessageService2
    {
        public string GetMessage() => "by Dependency Service.";
    }
}