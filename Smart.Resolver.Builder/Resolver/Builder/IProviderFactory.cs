namespace Smart.Resolver.Builder
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public interface IProviderFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IProvider Create(IComponentContainer components);
    }
}
