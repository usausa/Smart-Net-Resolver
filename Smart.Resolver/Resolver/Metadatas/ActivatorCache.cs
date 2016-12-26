namespace Smart.Resolver.Metadatas
{
    using System.Collections.Concurrent;
    using System.Reflection;

    using Smart.Reflection;

    /// <summary>
    ///
    /// </summary>
    public static class ActivatorCache
    {
        private static readonly ConcurrentDictionary<ConstructorInfo, IActivator> DefaultActivators = new ConcurrentDictionary<ConstructorInfo, IActivator>();

        private static readonly ConcurrentDictionary<ConstructorInfo, IActivator> SharedActivators = new ConcurrentDictionary<ConstructorInfo, IActivator>();

        public static IActivator GetActivator(ConstructorInfo ci, bool shared)
        {
            if (shared)
            {
                return SharedActivators.GetOrAdd(ci, x => x.ToActivator(GeneratorMode.Responsivity));
            }

            return DefaultActivators.GetOrAdd(ci, x => x.ToActivator());
        }
    }
}
