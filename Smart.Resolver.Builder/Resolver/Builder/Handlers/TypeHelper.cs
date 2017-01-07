namespace Smart.Resolver.Builder.Handlers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Where(p => p.Name == name)
                .Select(p => p.ParameterType)
                .Distinct()
                .ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructorArguments"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        public static object ActivateInstance(Type type, ConstructorArguments constructorArguments, IDictionary<string, object> propertyValues)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (constructorArguments == null)
            {
                throw new ArgumentNullException(nameof(constructorArguments));
            }

            if (propertyValues == null)
            {
                throw new ArgumentNullException(nameof(propertyValues));
            }

            ConstructorInfo constructorInfo = null;
            object[] arguments = null;
            foreach (var ci in type.GetConstructors().Where(ci => ci.GetParameters().Length == constructorArguments.Count))
            {
                bool result;
                var args = ResolveArgument(constructorArguments, ci, out result);
                if (result)
                {
                    constructorInfo = ci;
                    arguments = args;
                    break;
                }
            }

            if (constructorInfo == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor parameter unmatched. type = [{0}]", type));
            }

            var instance = constructorInfo.Invoke(arguments);

            foreach (var property in propertyValues)
            {
                var pi = type.GetProperty(property.Key);
                if (pi == null)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Property not found. type = [{0}], property = [{1}]", type, property.Key));
                }

                pi.SetValue(instance, property.Value);
            }

            return instance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="constructorArguments"></param>
        /// <param name="constructorInfo"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static object[] ResolveArgument(ConstructorArguments constructorArguments, ConstructorInfo constructorInfo, out bool result)
        {
            var parameters = constructorInfo.GetParameters();

            var arguments = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                object value;
                if (constructorArguments.TryGetNamadParameter(parameters[i].Name, out value) ||
                    constructorArguments.TryGetIndexedParameter(i, out value))
                {
                    arguments[i] = value;
                }
                else
                {
                    result = false;
                    return null;
                }
            }

            result = true;
            return arguments;
        }
    }
}
