namespace Example.WebApplication.Services;

using Microsoft.Extensions.Logging;

#pragma warning disable CA1848
public sealed class ScopedObject : IDisposable
{
    private readonly ILogger<ScopedObject> logger;

    public ScopedObject(ILogger<ScopedObject> logger)
    {
        this.logger = logger;
        logger.LogInformation("Construct {Hash}", GetHashCode());
    }

    public void Dispose()
    {
        logger.LogInformation("Dispose {Hash}", GetHashCode());
    }
}
#pragma warning restore CA1848
