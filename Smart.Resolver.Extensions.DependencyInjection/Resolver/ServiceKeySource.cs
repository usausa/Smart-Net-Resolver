namespace Smart.Resolver;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Keys;

#pragma warning disable CA1812
internal sealed class ServiceKeySource : IKeySource
{
    public object? GetValue(ICustomAttributeProvider provider)
    {
        var attributes = provider.GetCustomAttributes(typeof(ServiceKeyAttribute), false);
        return attributes.Length > 0 ? ServiceKeyMarker.Instance : null;
    }
}
#pragma warning restore CA1812
