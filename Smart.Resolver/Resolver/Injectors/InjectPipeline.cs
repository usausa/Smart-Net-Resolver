namespace Smart.Resolver.Injectors
{
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class InjectPipeline : IInjectPipeline
    {
        /// <summary>
        ///
        /// </summary>
        public IList<IInjector> Injectors { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="injectors"></param>
        public InjectPipeline(params IInjector[] injectors)
        {
            Injectors = injectors.ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        public void Inject(IKernel kernel, IBinding binding, TypeMetadata metadata, object instance)
        {
            for (var i = 0; i < Injectors.Count; i++)
            {
                Injectors[i].Inject(kernel, binding, metadata, instance);
            }
        }
    }
}
