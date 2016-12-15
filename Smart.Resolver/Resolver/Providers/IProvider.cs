namespace Smart.Resolver.Providers
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        ///
        /// </summary>
        Type TargetType { get; }

        object Create(IResolver resolver, IComponentContainer components, IBinding binding);
    }
}
