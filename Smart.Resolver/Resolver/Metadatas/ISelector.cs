namespace Smart.Resolver.Metadatas
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///
    /// </summary>
    public interface ISelector
    {
        ConstructorInfo SelectConstructor(Type type);

        IEnumerable<PropertyInfo> SelectProperties(Type type);
    }
}
