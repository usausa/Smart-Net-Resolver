namespace Smart.Resolver.Keys;

using System.Reflection;

using Smart.Resolver.Attributes;

public sealed class ResolveBySource : IKeySource
{
    public object? GetValue(ICustomAttributeProvider provider)
    {
        var attributes = provider.GetCustomAttributes(typeof(ResolveByAttribute), false);
        return attributes.Length > 0 ? ((ResolveByAttribute)attributes[0]).Key : null;
    }
}
