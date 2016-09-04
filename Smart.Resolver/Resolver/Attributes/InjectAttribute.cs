namespace Smart.Resolver.Attributes
{
    using System;

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
