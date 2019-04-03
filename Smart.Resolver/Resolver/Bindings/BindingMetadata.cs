namespace Smart.Resolver.Bindings
{
    using System.Collections.Generic;

    using Smart.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public sealed class BindingMetadata : IBindingMetadata
    {
        private readonly IDictionary<string, object> values;

        /// <summary>
        ///
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///
        /// </summary>
        public BindingMetadata()
            : this(null, null)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        public BindingMetadata(string name, IDictionary<string, object> values)
        {
            Name = name;
            this.values = values;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has(string key)
        {
            return (values != null) && values.ContainsKey(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Get(key, default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue)
        {
            return values is null ? defaultValue : (T)values.GetOr(key, defaultValue);
        }
    }
}
