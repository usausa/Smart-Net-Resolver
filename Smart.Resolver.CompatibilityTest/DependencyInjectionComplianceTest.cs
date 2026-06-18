namespace Smart.Resolver.CompatibilityTest;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;

using Smart.Resolver;

public sealed class DependencyInjectionComplianceTest : DependencyInjectionSpecificationTests
{
    // The adapter does not register IServiceProviderIsService. See "Microsoft.Extensions.DependencyInjection compatibility" in README.
    public override bool SupportsIServiceProviderIsService => false;

    protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
    {
        var factory = new SmartServiceProviderFactory();
        var config = factory.CreateBuilder(serviceCollection);
        return factory.CreateServiceProvider(config);
    }
}
