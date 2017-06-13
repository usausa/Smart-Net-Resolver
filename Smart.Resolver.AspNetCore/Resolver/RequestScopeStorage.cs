namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Scopes;

    public class RequestScopeStorage : IScopeStorage
    {
        private static readonly string StorageKey = "__RequestScopeStorage";

        private readonly IHttpContextAccessor accessor;

        public RequestScopeStorage(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public object GetOrAdd(IBinding binding, Func<IBinding, object> factory)
        {
            var context = accessor.HttpContext;

            context.Items.TryGetValue(StorageKey, out object value);
            var dictionary = (Dictionary<IBinding, object>)value;

            if ((dictionary != null) && dictionary.TryGetValue(binding, out object instance))
            {
                return instance;
            }

            instance = factory(binding);

            if (dictionary == null)
            {
                dictionary = new Dictionary<IBinding, object>();
                context.Items[StorageKey] = dictionary;
            }

            dictionary[binding] = instance;

            return instance;
        }

        public void Clear()
        {
            var context = accessor.HttpContext;

            context.Items.TryGetValue(StorageKey, out object value);
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
