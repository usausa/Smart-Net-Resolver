namespace Smart.Resolver.Helpers
{
    using System;
    using System.Collections.Generic;

    public static class TypeHelper
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);

        private static readonly Type CollectionType = typeof(ICollection<>);

        private static readonly Type ListType = typeof(IList<>);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static Type GetEnumerableElementType(Type type)
        {
            // Array
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            // IEnumerable type
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if ((genericType == EnumerableType) || (genericType == CollectionType) || (genericType == ListType))
                {
                    return type.GenericTypeArguments[0];
                }
            }

            return null;
        }
    }
}
