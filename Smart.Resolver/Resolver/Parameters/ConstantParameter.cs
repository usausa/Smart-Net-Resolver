namespace Smart.Resolver.Parameters
{
    public sealed class ConstantParameter : IParameter
    {
        private readonly object value;

        public ConstantParameter(object value)
        {
            this.value = value;
        }

        public object Resolve(IResolver resolver)
        {
            return value;
        }
    }
}
