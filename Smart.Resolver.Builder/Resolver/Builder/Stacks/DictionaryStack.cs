namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections;

    using Smart.Converter;

    /// <summary>
    ///
    /// </summary>
    public class DictionaryStack : IDictionaryStack
    {
        private readonly Type keyType;

        private readonly Type valueType;

        private readonly IObjectConverter converter;

        /// <summary>
        ///
        /// </summary>
        public IDictionary Dictionary { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <param name="converter"></param>
        public DictionaryStack(IDictionary dictionary, Type keyType, Type valueType, IObjectConverter converter)
        {
            Dictionary = dictionary;
            this.keyType = keyType;
            this.valueType = valueType;
            this.converter = converter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            Dictionary[converter.Convert(key, keyType)] = converter.Convert(value, valueType);
        }
    }
}
