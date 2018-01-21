namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Constraints;
    using Smart.Resolver.Factories;

    /// <summary>
    ///
    /// </summary>
    public interface IResolver
    {
        // CanGet

        bool CanGet<T>();

        bool CanGet<T>(IConstraint constraint);

        bool CanGet(Type type);

        bool CanGet(Type type, IConstraint constraint);

        // TryGet

        T TryGet<T>(out bool result);

        T TryGet<T>(IConstraint constraint, out bool result);

        object TryGet(Type type, out bool result);

        object TryGet(Type type, IConstraint constraint, out bool result);

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
    }
}
