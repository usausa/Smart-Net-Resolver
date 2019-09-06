namespace Smart.Resolver
{
    using System;
    using System.Runtime.CompilerServices;

    using Smart.Resolver.Constraints;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
    public static class SmartResolverExtensions
    {
        // CanGet

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanGet<T>(this SmartResolver resolver, string name)
        {
            return resolver.CanGet<T>(new NameConstraint(name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanGet(this SmartResolver resolver, Type type, string name)
        {
            return resolver.CanGet(type, new NameConstraint(name));
        }

        public static bool CanGet<T>(this IResolver resolver, string name)
        {
            return resolver.CanGet<T>(new NameConstraint(name));
        }

        public static bool CanGet(this IResolver resolver, Type type, string name)
        {
            return resolver.CanGet(type, new NameConstraint(name));
        }

        // TryGet

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(this SmartResolver resolver, string name, out T obj)
        {
            return resolver.TryGet(new NameConstraint(name), out obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet(this SmartResolver resolver, Type type, string name, out object obj)
        {
            return resolver.TryGet(type, new NameConstraint(name), out obj);
        }

        public static bool TryGet<T>(this IResolver resolver, string name, out T obj)
        {
            return resolver.TryGet(new NameConstraint(name), out obj);
        }

        public static bool TryGet(this IResolver resolver, Type type, string name, out object obj)
        {
            return resolver.TryGet(type, new NameConstraint(name), out obj);
        }

        // Get

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(this SmartResolver resolver, string name)
        {
            return resolver.Get<T>(new NameConstraint(name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Get(this SmartResolver resolver, Type type, string name)
        {
            return resolver.Get(type, new NameConstraint(name));
        }

        public static T Get<T>(this IResolver resolver, string name)
        {
            return resolver.Get<T>(new NameConstraint(name));
        }

        public static object Get(this IResolver resolver, Type type, string name)
        {
            return resolver.Get(type, new NameConstraint(name));
        }
    }
}