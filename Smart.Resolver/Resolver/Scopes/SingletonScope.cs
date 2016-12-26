namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScope : IScope
    {
        private readonly SingletonScopeStorage storage;

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        public SingletonScope(IComponentContainer components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            storage = components.Get<SingletonScopeStorage>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public IScopeStorage GetStorage(IKernel kernel)
        {
            return storage;
        }
    }
}
