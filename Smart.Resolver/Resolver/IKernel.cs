namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Factories;

    /// <summary>
    ///
    /// </summary>
    public interface IKernel : IResolver
    {
        /// <summary>
        ///
        /// </summary>
        IComponentContainer Components { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IObjectFactory ResolveFactory(Type type, IConstraint constraint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IEnumerable<IObjectFactory> ResolveAllFactory(Type type, IConstraint constraint);
    }
}
