namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public static class TypeHelper
    {
        private static readonly Type ListType = typeof(List<>);

        private static readonly Type DictionaryType = typeof(Dictionary<,>);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static IList CreateList(Type valueType)
        {
            var listType = ListType.MakeGenericType(valueType);
            return (IList)Activator.CreateInstance(listType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public static Array ConvertListToArray(IList list)
        {
            var array = Array.CreateInstance(list.GetType().GetElementType(), list.Count);
            list.CopyTo(array, 0);
            return array;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static IDictionary CreateDictionary(Type keyType, Type valueType)
        {
            var dictionaryType = DictionaryType.MakeGenericType(keyType, valueType);
            return (IDictionary)Activator.CreateInstance(dictionaryType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type[] ResolveConstructorArtumentType(Type type, string name)
        {
            // TODO
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructorArguments"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        public static object ActivateInstance(Type type, IList<KeyValuePair<string, object>> constructorArguments, IDictionary<string, object> propertyValues)
        {
            // TODO
            return Activator.CreateInstance(type);
        }
    }
}
