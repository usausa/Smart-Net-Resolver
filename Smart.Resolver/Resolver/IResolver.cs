namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        bool CanResolve(Type type, IConstraint constraint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        object TryResolve(Type type, IConstraint constraint, out bool result);

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
