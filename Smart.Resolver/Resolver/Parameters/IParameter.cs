namespace Smart.Resolver.Parameters
{
    public interface IParameter
    {
        object Resolve(IKernel kernel);
    }
}
