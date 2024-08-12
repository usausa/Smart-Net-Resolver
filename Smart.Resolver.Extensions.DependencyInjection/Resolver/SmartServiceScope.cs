namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartServiceScope : IServiceScope
{
    private readonly SmartChildResolver childResolver;

    public IServiceProvider ServiceProvider { get; }

    public SmartServiceScope(SmartResolver resolver)
    {
        childResolver = resolver.CreateChildResolver();
        ServiceProvider = new SmartChildServiceProvider(childResolver);
    }

    public void Dispose()
    {
        childResolver.Dispose();
    }
}
