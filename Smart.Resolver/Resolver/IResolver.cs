namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Constraints;
    using Smart.Resolver.Factories;

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
        IObjectFactory TryResolve(Type type, IConstraint constraint, out bool result);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IObjectFactory Resolve(Type type, IConstraint constraint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IObjectFactory[] ResolveAll(Type type, IConstraint constraint);
    }
}
