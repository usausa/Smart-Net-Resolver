namespace Smart.Resolver.Bindings
{
    using System.Collections.Generic;

    using Smart.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class BindingMetadata : IBindingMetadata
    {
        private Dictionary<string, object> values;

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

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
            return values != null ? (T)values.GetOr(key, defaultValue) : defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (values == null)
            {
                values = new Dictionary<string, object>();
            }

            values[key] = value;
        }
    }
}
