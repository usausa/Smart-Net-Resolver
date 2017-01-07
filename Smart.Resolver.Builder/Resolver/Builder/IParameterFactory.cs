namespace Smart.Resolver.Builder
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Parameters;

    /// <summary>
    ///
    /// </summary>
    public interface IParameterFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IParameter Create(IComponentContainer components);
    }
}
