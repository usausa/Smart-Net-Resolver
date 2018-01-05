namespace Smart.Resolver.Factories
{
    using Smart.Resolver.Parameters;

    public sealed class ParameterObjectFactory : IObjectFactory
    {
        private readonly IKernel kernel;

        private readonly IParameter parameter;

        public ParameterObjectFactory(IKernel kernel, IParameter parameter)
        {
            this.kernel = kernel;
            this.parameter = parameter;
        }

        public object Create()
        {
            return parameter.Resolve(kernel);
        }
    }
}
