namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public static T TryGet<T>(this IResolver resolver)
        {
            return (T)resolver.TryResolve(typeof(T), null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, string name)
        {
            return (T)resolver.TryResolve(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this IResolver resolver, IConstraint constraint)
        {
            return (T)resolver.TryResolve(typeof(T), constraint);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type)
        {
            return resolver.TryResolve(type, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, string name)
        {
            return resolver.TryResolve(type, new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this IResolver resolver, Type type, IConstraint constraint)
        {
            return resolver.TryResolve(type, constraint);
        }

        // Get

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T), null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver, string name)
        {
            return (T)resolver.Resolve(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver, IConstraint constraint)
        {
            return (T)resolver.Resolve(typeof(T), constraint);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type)
        {
            return resolver.Resolve(type, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type, string name)
        {
            return resolver.Resolve(type, new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type, IConstraint constraint)
        {
            return resolver.Resolve(type, constraint);
        }

        // GetAll

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static IEnumerable<T> GetAll<T>(this IResolver resolver)
        {
            return resolver.ResolveAll(typeof(T), null).Cast<T>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static IEnumerable<object> GetAll(this IResolver resolver, Type type)
        {
            return resolver.ResolveAll(type, null);
        }
    }
}
