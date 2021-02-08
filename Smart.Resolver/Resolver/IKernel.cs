namespace Smart.Resolver
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Smart.Resolver.Constraints;

    public interface IKernel
    {
        bool TryResolveFactory(Type type, IConstraint? constraint, [NotNullWhen(true)] out Func<IResolver, object> factory);

        bool TryResolveFactories(Type type, IConstraint? constraint, [NotNullWhen(true)] out Func<IResolver, object>[] factories);
    }
}
