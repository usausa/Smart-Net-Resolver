namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Bindings;
using Smart.Resolver.Constraints;

internal sealed class MediKeyConstraint : IConstraint
{
    private readonly object serviceKey;

    public MediKeyConstraint(object serviceKey)
    {
        this.serviceKey = serviceKey;
    }

    public bool Match(BindingMetadata metadata, object? key)
        => ReferenceEquals(key, KeyedService.AnyKey) || serviceKey.Equals(key);
}

internal sealed class MediAnyKeyConstraint : IConstraint
{
    public static readonly MediAnyKeyConstraint Instance = new();

    private MediAnyKeyConstraint()
    {
    }

    public bool Match(BindingMetadata metadata, object? key) =>
        (key is not null) && !ReferenceEquals(key, KeyedService.AnyKey);
}
