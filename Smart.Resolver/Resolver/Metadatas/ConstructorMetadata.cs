namespace Smart.Resolver.Metadatas
{
    using System.Reflection;

    using Smart.Reflection;
    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class ConstructorMetadata
    {
        private readonly object sync = new object();

        private volatile IActivator defaultActivator;

        private IActivator sharedActivator;

        public ConstructorInfo Constructor { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public ParameterMetadata[] Parameters { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Performance")]
        public IConstraint[] Constraints { get; }

        /// <summary>
        ///
        /// </summary>
        public IActivator DefaultActivator
        {
            get
            {
                if (defaultActivator == null)
                {
                    lock (sync)
                    {
                        if (defaultActivator == null)
                        {
                            defaultActivator = Constructor.ToActivator();
                        }
                    }
                }

                return defaultActivator;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivator SharedActivator => sharedActivator ?? (sharedActivator = Constructor.ToActivator(GeneratorMode.Responsivity));

        /// <summary>
        ///
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="parameters"></param>
        /// <param name="constraints"></param>
        public ConstructorMetadata(ConstructorInfo constructor, ParameterMetadata[] parameters, IConstraint[] constraints)
        {
            Constructor = constructor;
            Parameters = parameters;
            Constraints = constraints;
        }
    }
}
