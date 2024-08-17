namespace Smart.Resolver;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Keys;

#pragma warning disable CA1812
internal sealed class FromKeyedServicesSource : IKeySource
{
    public object? GetValue(ICustomAttributeProvider provider)
    {
        var attributes = provider.GetCustomAttributes(typeof(FromKeyedServicesAttribute), false);
        return attributes.Length > 0 ? ((FromKeyedServicesAttribute)attributes[0]).Key : null;
    }
}
#pragma warning restore CA1812
