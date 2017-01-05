namespace Smart.Resolver.Builder.Stacks
{
    using System;
    using System.Collections.Generic;

    using Smart.Converter;

    /// <summary>
    ///
    /// </summary>
    public class ListStack
    {
        private readonly Type elementType;

        private readonly IObjectConverter converter;

        public IList<object> List { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="converter"></param>
        /// <param name="list"></param>
        public ListStack(Type elementType, IObjectConverter converter, IList<object> list)
        {
            this.elementType = elementType;
            this.converter = converter;
            List = list;
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
