namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public interface IResolver : IServiceProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        object Resolve(Type type, IConstraint constraint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type type, IConstraint constraint);
    }
}
