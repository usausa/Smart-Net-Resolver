namespace Smart.Resolver.Attributes
{
    using System;
    using System.Reflection;

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
    }

    /// <summary>
    ///
    /// </summary>
    public static class InjectAttributeExtensions
    {
        private static readonly Type InjectType = typeof(InjectAttribute);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static bool IsInjectDefined(this ConstructorInfo ci)
        {
            return ci.IsDefined(InjectType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsInjectDefined(this PropertyInfo pi)
        {
            return pi.IsDefined(InjectType);
        }
    }
}
