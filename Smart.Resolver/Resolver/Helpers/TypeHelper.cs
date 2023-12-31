namespace Smart.Resolver.Helpers;

public static class TypeHelper
{
    public static Type? GetEnumerableElementType(Type type)
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
            if ((genericType == typeof(IEnumerable<>)) ||
                (genericType == typeof(ICollection<>)) ||
                (genericType == typeof(IList<>)))
            {
                return type.GenericTypeArguments[0];
            }
        }

        return null;
    }
}
