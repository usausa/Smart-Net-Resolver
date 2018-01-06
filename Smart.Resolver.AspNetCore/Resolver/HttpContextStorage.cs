namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;

    public static class HttpContextStorage
    {
        private static readonly string StorageKey = "__RequestScopeStorage";

        public static object GetOrAdd(HttpContext context, IBinding binding, IObjectFactory objectFactory)
        {
            context.Items.TryGetValue(StorageKey, out var value);
            var dictionary = (Dictionary<IBinding, object>)value;

            if ((dictionary != null) && dictionary.TryGetValue(binding, out var instance))
            {
                return instance;
            }

            instance = objectFactory.Create();

            if (dictionary == null)
            {
                dictionary = new Dictionary<IBinding, object>();
                context.Items[StorageKey] = dictionary;
            }

            dictionary[binding] = instance;

            return instance;
        }

        public static void Clear(HttpContext context)
        {
            if (context == null)
            {
                return;
            }

            context.Items.TryGetValue(StorageKey, out var value);
            var dictionary = (Dictionary<IBinding, object>)value;
            if (dictionary == null)
            {
                return;
            }

            foreach (var instance in dictionary.Values)
            {
                (instance as IDisposable)?.Dispose();
            }

            context.Items.Remove(StorageKey);
        }
    }
}
