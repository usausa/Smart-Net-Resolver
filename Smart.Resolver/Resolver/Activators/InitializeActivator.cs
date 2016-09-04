namespace Smart.Resolver.Activators
{
    /// <summary>
    ///
    /// </summary>
    public class InitializeActivator : IActivator
    {
        public void Activate(object instance)
        {
            (instance as IInitializable)?.Initialize();
        }
    }
}
