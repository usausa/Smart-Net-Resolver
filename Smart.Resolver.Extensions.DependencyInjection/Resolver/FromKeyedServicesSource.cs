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
        if (attributes.Length == 0)
        {
            return null;
        }

        return ((FromKeyedServicesAttribute)attributes[0]).Key ?? InheritKeyMarker.Instance;
    }
}
#pragma warning restore CA1812
