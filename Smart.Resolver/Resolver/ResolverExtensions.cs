namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public static class ResolverExtensions
    {
        // CanGet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet<T>(this IResolver resolver)
        {
            return resolver.CanResolve(typeof(T), null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet<T>(this IResolver resolver, string name)
        {
            return resolver.CanResolve(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet<T>(this IResolver resolver, IConstraint constraint)
        {
            return resolver.CanResolve(typeof(T), constraint);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet(this IResolver resolver, Type type)
        {
            return resolver.CanResolve(type, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet(this IResolver resolver, Type type, string name)
        {
            return resolver.CanResolve(type, new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet(this IResolver resolver, Type type, IConstraint constraint)
        {
            return resolver.CanResolve(type, constraint);
        }

        // TryGet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, out bool result)
        {
            return (T)resolver.TryResolve(typeof(T), null, out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, string name, out bool result)
        {
            return (T)resolver.TryResolve(typeof(T), new NameConstraint(name), out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, IConstraint constraint, out bool result)
        {
            return (T)resolver.TryResolve(typeof(T), constraint, out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, out bool result)
        {
            return resolver.TryResolve(type, null, out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, string name, out bool result)
        {
            return resolver.TryResolve(type, new NameConstraint(name), out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, IConstraint constraint, out bool result)
        {
            return resolver.TryResolve(type, constraint, out result)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver)
        {
            return (T)resolver.TryResolve(typeof(T), null, out bool _)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, string name)
        {
            return (T)resolver.TryResolve(typeof(T), new NameConstraint(name), out bool _)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, IConstraint constraint)
        {
            return (T)resolver.TryResolve(typeof(T), constraint, out bool _)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type)
        {
            return resolver.TryResolve(type, null, out bool _)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, string name)
        {
            return resolver.TryResolve(type, new NameConstraint(name), out bool _)?.Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, IConstraint constraint)
        {
            return resolver.TryResolve(type, constraint, out bool _)?.Create();
        }

        // Get

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T), null).Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver, string name)
        {
            return (T)resolver.Resolve(typeof(T), new NameConstraint(name)).Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver, IConstraint constraint)
        {
            return (T)resolver.Resolve(typeof(T), constraint).Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type)
        {
            return resolver.Resolve(type, null).Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type, string name)
        {
            return resolver.Resolve(type, new NameConstraint(name)).Create();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type, IConstraint constraint)
        {
            return resolver.Resolve(type, constraint).Create();
        }

        // GetAll

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T[] GetAll<T>(this IResolver resolver)
        {
            var factories = resolver.ResolveAll(typeof(T), null);
            var array = new T[factories.Length];
            for (var i = 0; i < factories.Length; i++)
            {
                array[i] = (T)factories[i].Create();
            }

            return array;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static IEnumerable<object> GetAll(this IResolver resolver, Type type)
        {
            var factories = resolver.ResolveAll(type, null);
            var array = new object[factories.Length];
            for (var i = 0; i < factories.Length; i++)
            {
                array[i] = factories[i].Create();
            }

            return array;
        }
    }
}
