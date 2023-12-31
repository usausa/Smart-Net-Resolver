namespace Smart.Resolver.Attributes;

using System.Reflection;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property)]
public sealed class InjectAttribute : Attribute
{
}

public static class InjectAttributeExtensions
{
    public static bool IsInjectDefined(this ConstructorInfo ci)
    {
        return ci.IsDefined(typeof(InjectAttribute));
    }

    public static bool IsInjectDefined(this PropertyInfo pi)
    {
        return pi.IsDefined(typeof(InjectAttribute));
    }
}
