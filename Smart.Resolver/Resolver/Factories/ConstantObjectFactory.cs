namespace Smart.Resolver.Factories
{
    public sealed class ConstantObjectFactory : IObjectFactory
    {
        private readonly object value;

        public ConstantObjectFactory(object value)
        {
            this.value = value;
        }

        public object Create()
        {
            return value;
        }
    }
}
