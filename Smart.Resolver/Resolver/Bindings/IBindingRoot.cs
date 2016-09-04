namespace Smart.Resolver.Bindings
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IBindingRoot
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBindingToSyntax<T> Bind<T>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBindingToSyntax<object> Bind(Type type);
    }
}
