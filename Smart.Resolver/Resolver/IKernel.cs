namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public interface IKernel : IResolver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        Func<object> ResolveFactory(Type type, IConstraint constraint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IEnumerable<Func<object>> ResolveAllFactory(Type type, IConstraint constraint);
    }
}
