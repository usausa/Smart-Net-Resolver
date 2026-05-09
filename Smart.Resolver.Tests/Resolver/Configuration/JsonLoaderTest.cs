namespace Smart.Resolver.Configuration;

using Smart.Resolver.Mocks;

public sealed class JsonLoaderTest
{
    private const string Asm = "Smart.Resolver.Tests";

    // ──────────────────────────────────────────────────────────
    // TargetKind: Self
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenTargetKindIsSelfThenResolvesConcreteType()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<Service1>();

        Assert.NotNull(instance);
    }

    // ──────────────────────────────────────────────────────────
    // TargetKind: Type
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenTargetKindIsTypeThenResolvesImplementation()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Type",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<IService>();

        Assert.IsType<Service1>(instance);
    }

    // ──────────────────────────────────────────────────────────
    // TargetKind: Constant (string)
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenTargetKindIsConstantThenReturnsStringValue()
    {
        var json = """
            {
              "bindings": [
                {
                  "binding": "Constant",
                  "service": "System.String",
                  "constant": "hello"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var value = resolver.Get<string>();

        Assert.Equal("hello", value);
    }

    // ──────────────────────────────────────────────────────────
    // TargetKind: Constant with explicit constantValueType
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenConstantValueTypeIsSpecifiedThenValueIsConverted()
    {
        var json = """
            {
              "bindings": [
                {
                  "binding": "Constant",
                  "service": "System.Object",
                  "constantType": "System.Int32",
                  "constant": "42"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var value = resolver.Get<object>();

        Assert.Equal(42, value);
    }

    // ──────────────────────────────────────────────────────────
    // Scope: Transient (default)
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenScopeIsTransientThenInstancesAreNotSame()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Transient",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var a = resolver.Get<Service1>();
        var b = resolver.Get<Service1>();

        Assert.NotSame(a, b);
    }

    // ──────────────────────────────────────────────────────────
    // Scope: Singleton
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenScopeIsSingletonThenInstancesAreSame()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var a = resolver.Get<Service1>();
        var b = resolver.Get<Service1>();

        Assert.Same(a, b);
    }

    // ──────────────────────────────────────────────────────────
    // Scope: Container
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenScopeIsContainerThenInstancesInSameChildAreSame()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Container",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        using var child = resolver.CreateChildResolver();
        var a = child.Get<Service1>();
        var b = child.Get<Service1>();

        Assert.Same(a, b);
    }

    [Fact]
    public void WhenScopeIsContainerThenInstancesInDifferentChildrenAreNotSame()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Container",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        using var child1 = resolver.CreateChildResolver();
        using var child2 = resolver.CreateChildResolver();
        var a = child1.Get<Service1>();
        var b = child2.Get<Service1>();

        Assert.NotSame(a, b);
    }

    // ──────────────────────────────────────────────────────────
    // ConstraintKey
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenConstraintKeyIsSetThenResolvesByKey()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "key": "a",
                  "binding": "Type",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                },
                {
                  "key": "b",
                  "binding": "Type",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service2, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var a = resolver.Get<IService>("a");
        var b = resolver.Get<IService>("b");

        Assert.IsType<Service1>(a);
        Assert.IsType<Service2>(b);
    }

    // ──────────────────────────────────────────────────────────
    // Metadata (string value)
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenMetadataIsSetThenBindingHasMetadataValue()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}",
                  "metadata": [
                    { "key": "tag", "value": "primary" }
                  ]
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);

        var components = ((IResolverConfig)config).CreateComponentContainer();
        var binding = ((IResolverConfig)config).CreateBindings(components).First(static x => x.Type == typeof(Service1));

        Assert.Equal("primary", binding.Metadata.Get<string>("tag"));
    }

    // ──────────────────────────────────────────────────────────
    // Metadata with valueType (typed value)
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenMetadataValueTypeIsSpecifiedThenValueIsConverted()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.Service1, {{Asm}}",
                  "metadata": [
                    { "key": "order", "valueType": "System.Int32", "value": "10" }
                  ]
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);

        var components = ((IResolverConfig)config).CreateComponentContainer();
        var binding = ((IResolverConfig)config).CreateBindings(components).First(static x => x.Type == typeof(Service1));

        Assert.Equal(10, binding.Metadata.Get<int>("order"));
    }

    // ──────────────────────────────────────────────────────────
    // ConstructorArguments: Constant + valueType + Reference
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenConstructorArgumentIsConstantStringThenValueIsInjected()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Type",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                },
                {
                  "binding": "Self",
                  "service": "Smart.Resolver.Mocks.ServiceWithConstructor, {{Asm}}",
                  "constructorArguments": [
                    { "name": "name", "kind": "Constant", "value": "test-name" },
                    { "name": "count", "kind": "Constant", "valueType": "System.Int32", "value": "5" },
                    {
                      "name": "service",
                      "kind": "Reference",
                      "value": "Smart.Resolver.Mocks.IService, {{Asm}}"
                    }
                  ]
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<ServiceWithConstructor>();

        Assert.Equal("test-name", instance.Name);
        Assert.Equal(5, instance.Count);
        Assert.IsType<Service1>(instance.Service);
    }

    // ──────────────────────────────────────────────────────────
    // ConstructorArguments: Reference resolves same singleton
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenConstructorArgumentIsReferenceThenResolvedInstanceIsInjected()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Type",
                  "scope": "Singleton",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service2, {{Asm}}"
                },
                {
                  "binding": "Self",
                  "service": "Smart.Resolver.Mocks.ServiceWithConstructor, {{Asm}}",
                  "constructorArguments": [
                    { "name": "name", "kind": "Constant", "value": "ref-test" },
                    { "name": "count", "kind": "Constant", "valueType": "System.Int32", "value": "0" },
                    {
                      "name": "service",
                      "kind": "Reference",
                      "value": "Smart.Resolver.Mocks.IService, {{Asm}}"
                    }
                  ]
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<ServiceWithConstructor>();
        var service = resolver.Get<IService>();

        Assert.Same(service, instance.Service);
    }

    // ──────────────────────────────────────────────────────────
    // PropertyValues: Constant + valueType
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenPropertyValueIsConstantThenValueIsInjected()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Self",
                  "service": "Smart.Resolver.Mocks.ServiceWithProperty, {{Asm}}",
                  "propertyValues": [
                    { "name": "Name", "kind": "Constant", "value": "prop-name" },
                    { "name": "Count", "kind": "Constant", "valueType": "System.Int32", "value": "7" }
                  ]
                }
              ]
            }
            """;

        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<ServiceWithProperty>();

        Assert.Equal("prop-name", instance.Name);
        Assert.Equal(7, instance.Count);
    }

    // ──────────────────────────────────────────────────────────
    // Multiple bindings: last registration wins
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenMultipleBindingsAreLoadedThenLastRegistrationWins()
    {
        var json = $$"""
            {
              "bindings": [
                {
                  "binding": "Type",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service1, {{Asm}}"
                },
                {
                  "binding": "Type",
                  "service": "Smart.Resolver.Mocks.IService, {{Asm}}",
                  "implementation": "Smart.Resolver.Mocks.Service2, {{Asm}}"
                }
              ]
            }
            """;

        var config = new ResolverConfig().LoadJson(json);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<IService>();

        Assert.IsType<Service2>(instance);
    }

    // ──────────────────────────────────────────────────────────
    // LoadDefinition (intermediate definition)
    // ──────────────────────────────────────────────────────────

    [Fact]
    public void WhenLoadDefinitionIsCalledThenBindingsAreApplied()
    {
        var definition = new ResolverDefinition
        {
            Bindings =
            [
                new BindingDefinition
                {
                    Service = $"Smart.Resolver.Mocks.IService, {Asm}",
                    Binding = BindingKind.Type,
                    Implementation = $"Smart.Resolver.Mocks.Service1, {Asm}",
                    Scope = ScopeKind.Singleton
                }
            ]
        };

        var config = new ResolverConfig().LoadDefinition(definition);
        using var resolver = config.ToResolver();

        var instance = resolver.Get<IService>();

        Assert.IsType<Service1>(instance);
    }
}
