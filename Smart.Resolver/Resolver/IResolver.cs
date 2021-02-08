namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Smart.Resolver.Constraints;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
    public interface IResolver : IDisposable
    {
        // CanGet

        bool CanGet<T>();

        bool CanGet<T>(IConstraint constraint);

        bool CanGet(Type type);

        bool CanGet(Type type, IConstraint constraint);

        // TryGet

        bool TryGet<T>([MaybeNullWhen(false)] out T obj);

        bool TryGet<T>(IConstraint constraint, [MaybeNullWhen(false)] out T obj);

        bool TryGet(Type type, [MaybeNullWhen(false)] out object obj);

        bool TryGet(Type type, IConstraint constraint, [MaybeNullWhen(false)] out object obj);

        // Get

        T Get<T>();

        T Get<T>(IConstraint constraint);

        object Get(Type type);

        object Get(Type type, IConstraint constraint);

        // GetAll

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAll<T>(IConstraint constraint);

        IEnumerable<object> GetAll(Type type);

        IEnumerable<object> GetAll(Type type, IConstraint constraint);

        // Inject

        void Inject(object instance);
    }
}
