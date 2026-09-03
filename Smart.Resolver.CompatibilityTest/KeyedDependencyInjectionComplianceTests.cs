namespace Smart.Resolver.CompatibilityTest;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;

using Smart.Resolver;

public sealed class KeyedDependencyInjectionComplianceTests : KeyedDependencyInjectionSpecificationTests
{
    public override bool SupportsIServiceProviderIsKeyedService => true;

    protected override IServiceProvider CreateServiceProvider(IServiceCollection collection)
    {
        var factory = new SmartServiceProviderFactory();
        var config = factory.CreateBuilder(collection);
        return factory.CreateServiceProvider(config);
    }
}
