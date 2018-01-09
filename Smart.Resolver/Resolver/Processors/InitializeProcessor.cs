namespace Smart.Resolver.Processors
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class InitializeProcessor : IProcessor
    {
        private static readonly Type InitializableType = typeof(IInitializable);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTarget(Type type)
        {
            return InitializableType.IsAssignableFrom(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        public void Initialize(object instance)
        {
            (instance as IInitializable)?.Initialize();
        }
    }
}
