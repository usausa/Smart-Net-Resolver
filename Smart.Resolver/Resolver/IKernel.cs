namespace Smart.Resolver
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Constraints;

    public interface IKernel : IResolver
    {
        bool TryResolveFactory(Type type, IConstraint constraint, out Func<IKernel, object> factory);

        bool TryResolveFactories(Type type, IConstraint constraint, out Func<IKernel, object>[] factories);
    }
}
