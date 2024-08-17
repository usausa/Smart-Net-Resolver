namespace Smart.Resolver.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class ResolveByAttribute : Attribute
{
    public object Key { get; }

    public ResolveByAttribute(object key)
    {
        Key = key;
    }
}
