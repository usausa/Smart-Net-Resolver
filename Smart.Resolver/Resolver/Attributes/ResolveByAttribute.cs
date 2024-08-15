namespace Smart.Resolver.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class ResolveByAttribute : Attribute
{
    public object Parameter { get; }

    public ResolveByAttribute(object parameter)
    {
        Parameter = parameter;
    }
}
