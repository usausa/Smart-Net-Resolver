namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public static class SmartResolverExtensions
    {
        // TODO inline?

        // CanGet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet<T>(this SmartResolver resolver, string name)
        {
            return resolver.CanGet(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static bool CanGet(this SmartResolver resolver, Type type, string name)
        {
            return resolver.CanGet(type, new NameConstraint(name));
        }

        // TryGet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this SmartResolver resolver, string name, out bool result)
        {
            return (T)resolver.TryGet(typeof(T), new NameConstraint(name), out result);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this SmartResolver resolver, Type type, string name, out bool result)
        {
            return resolver.TryGet(type, new NameConstraint(name), out result);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T TryGet<T>(this SmartResolver resolver, string name)
        {
            return (T)resolver.TryGet(typeof(T), new NameConstraint(name), out bool _);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object TryGet(this SmartResolver resolver, Type type, string name)
        {
            return resolver.TryGet(type, new NameConstraint(name), out bool _);
        }

        // Get

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static T Get<T>(this SmartResolver resolver, string name)
        {
            return (T)resolver.Get(typeof(T), new NameConstraint(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static object Get(this SmartResolver resolver, Type type, string name)
        {
            return resolver.Get(type, new NameConstraint(name));
        }
    }
}