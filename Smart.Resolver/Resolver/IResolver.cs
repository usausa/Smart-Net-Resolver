namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

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

        bool TryGet<T>(out T obj);

        bool TryGet<T>(IConstraint constraint, out T obj);

        bool TryGet(Type type, out object obj);

        bool TryGet(Type type, IConstraint constraint, out object obj);

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
