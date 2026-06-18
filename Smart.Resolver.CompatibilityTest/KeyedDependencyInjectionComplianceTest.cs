namespace Smart.Resolver.CompatibilityTest;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;

using Smart.Resolver;

public sealed class KeyedDependencyInjectionComplianceTest : KeyedDependencyInjectionSpecificationTests
{
    // The adapter does not register IServiceProviderIsKeyedService. See "Microsoft.Extensions.DependencyInjection compatibility" in README.
    public override bool SupportsIServiceProviderIsKeyedService => false;

    protected override IServiceProvider CreateServiceProvider(IServiceCollection collection)
    {
        var factory = new SmartServiceProviderFactory();
        var config = factory.CreateBuilder(collection);
        return factory.CreateServiceProvider(config);
    }
}
