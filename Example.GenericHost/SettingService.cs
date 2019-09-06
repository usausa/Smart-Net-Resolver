namespace Example.GenericHost
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SettingService
    {
        private ILogger<ExampleHostedService> Log { get; }

        private Settings Settings { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public SettingService(
            ILogger<ExampleHostedService> log,
            IOptions<Settings> settings)
        {
            Log = log;
            Settings = settings.Value;
        }

        public void Write()
        {
            Log.LogInformation(Settings.Data);
        }
    }
}
