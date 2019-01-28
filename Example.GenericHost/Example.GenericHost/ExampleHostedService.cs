﻿namespace Example.GenericHost
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ExampleHostedService : IHostedService
    {
        private ILogger<ExampleHostedService> Log { get; }

        private IApplicationLifetime ApplicationLifetime { get; }

        public ExampleHostedService(
            ILogger<ExampleHostedService> log,
            IApplicationLifetime applicationLifetime)
        {
            Log = log;
            ApplicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation("StartAsync");

            ApplicationLifetime.ApplicationStarted.Register(() => Run(cancellationToken));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation("StopAsync");

            return Task.CompletedTask;
        }

        private async void Run(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(30000, cancellationToken);
            }
            catch (Exception e)
            {
                Log.LogError(e, "Error");
            }
            finally
            {
                ApplicationLifetime.StopApplication();
            }
        }
    }
}
