namespace Example.FormsApp.iOS.Services
{
    using Example.FormsApp.Services;

    public class MessageService : IMessageService
    {
        public string GetMessage() => "Hello iOS.";
    }
}
