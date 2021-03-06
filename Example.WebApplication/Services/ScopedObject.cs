namespace Example.WebApplication.Services
{
    using System;

    using Microsoft.Extensions.Logging;

    public sealed class ScopedObject : IDisposable
    {
        private readonly ILogger<ScopedObject> logger;

        public ScopedObject(ILogger<ScopedObject> logger)
        {
            this.logger = logger;
            logger.LogInformation("Construct {0}", GetHashCode());
        }

        public void Dispose()
        {
            logger.LogInformation("Dispose {0}", GetHashCode());
        }
    }
}
