namespace Smart.Resolver.Processors
{
    /// <summary>
    ///
    /// </summary>
    public class InitializeProcessor : IProcessor
    {
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
