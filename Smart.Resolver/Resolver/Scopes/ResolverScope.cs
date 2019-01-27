using Smart.Resolver.Bindings;

namespace Smart.Resolver.Scopes
{
    public sealed class ResolverScope : IScope
    {
        public IScope Copy(IComponentContainer components)
        {
            throw new NotImplementedException();
        }

        public Func<Object> Create(IKernel kernel, IBinding binding, Func<Object> factory)
        {
            throw new NotImplementedException();
        }
    }
}
