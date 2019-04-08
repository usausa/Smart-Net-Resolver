namespace Smart.Resolver.Attributes
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
    }

    public static class InjectAttributeExtensions
    {
        private static readonly Type InjectType = typeof(InjectAttribute);

        public static bool IsInjectDefined(this ConstructorInfo ci)
        {
            return ci.IsDefined(InjectType);
        }

        public static bool IsInjectDefined(this PropertyInfo pi)
        {
            return pi.IsDefined(InjectType);
        }
    }
}
