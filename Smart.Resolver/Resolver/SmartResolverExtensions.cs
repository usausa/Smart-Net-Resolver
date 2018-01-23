namespace Smart.Resolver
{
    using System;
    using System.Runtime.CompilerServices;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public static class SmartResolverExtensions
    {
        // CanGet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanGet<T>(this SmartResolver resolver, string name)
        {
            return resolver.CanGet(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanGet(this SmartResolver resolver, Type type, string name)
        {
            return resolver.CanGet(type, new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet<T>(this IResolver resolver, string name)
        {
            return resolver.CanGet(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet(this IResolver resolver, Type type, string name)
        {
            return resolver.CanGet(type, new NameConstraint(name));
        }

        // Get

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(this SmartResolver resolver, string name)
        {
            return (T)resolver.Get(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Get(this SmartResolver resolver, Type type, string name)
        {
            return resolver.Get(type, new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this IResolver resolver, string name)
        {
            return (T)resolver.Get(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this IResolver resolver, Type type, string name)
        {
            return resolver.Get(type, new NameConstraint(name));
        }
    }
}