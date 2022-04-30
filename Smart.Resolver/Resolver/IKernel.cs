namespace Smart.Resolver;

using Smart.Resolver.Constraints;

public interface IKernel
{
    bool TryResolveFactory(Type type, IConstraint? constraint, out Func<IResolver, object> factory);

    bool TryResolveFactories(Type type, IConstraint? constraint, out Func<IResolver, object>[] factories);
}
