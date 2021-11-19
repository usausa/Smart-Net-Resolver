namespace Example.GenericHost;

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class ExampleHostedService : IHostedService, IDisposable
{
    private ILogger<ExampleHostedService> Log { get; }

    private SettingService SettingService { get; }

    public ExampleHostedService(
        ILogger<ExampleHostedService> log,
        SettingService settingService)
    {
        Log = log;
        SettingService = settingService;
    }

    public void Dispose()
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Log.LogInformation("StartAsync");

        SettingService.Write();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.LogInformation("StopAsync");

        return Task.CompletedTask;
    }
}
