namespace Smart.Resolver.Activators
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class ActivatePipeline : IActivatePipeline
    {
        /// <summary>
        ///
        /// </summary>
        public IList<IActivator> Activators { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="activators"></param>
        public ActivatePipeline(params IActivator[] activators)
        {
            Activators = activators.ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        public void Activate(object instance)
        {
            if (Activators.Count == 0)
            {
                return;
            }

            foreach (var activator in Activators)
            {
                activator.Activate(instance);
            }
        }
    }
}
