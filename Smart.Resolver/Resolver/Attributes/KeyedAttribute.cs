namespace Smart.Resolver.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class KeyedAttribute : Attribute
{
    public object Key { get; }

    public KeyedAttribute(object key)
    {
        Key = key;
    }
}
