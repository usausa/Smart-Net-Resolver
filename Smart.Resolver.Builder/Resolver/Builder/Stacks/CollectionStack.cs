namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections;

    using Smart.Converter;

    /// <summary>
    ///
    /// </summary>
    public class CollectionStack : ICollectionStack
    {
        private readonly Type valueType;

        private readonly IObjectConverter converter;

        public IList List { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="valueType"></param>
        /// <param name="converter"></param>
        public CollectionStack(IList list, Type valueType, IObjectConverter converter)
        {
            List = list;
            this.valueType = valueType;
            this.converter = converter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            List.Add(converter.Convert(value, valueType));
        }
    }
}
