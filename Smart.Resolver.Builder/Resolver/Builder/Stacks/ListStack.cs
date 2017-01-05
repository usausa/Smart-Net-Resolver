namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections;

    using Smart.Converter;

    /// <summary>
    ///
    /// </summary>
    public class ListStack
    {
        private readonly Type elementType;

        private readonly IObjectConverter converter;

        public IList List { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="elementType"></param>
        /// <param name="converter"></param>
        public ListStack(IList list, Type elementType, IObjectConverter converter)
        {
            List = list;
            this.elementType = elementType;
            this.converter = converter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            List.Add(converter.Convert(value, elementType));
        }
    }
}
