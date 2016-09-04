namespace Smart.Resolver.Metadatas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.Resolver.Attributes;

    /// <summary>
    ///
    /// </summary>
    public class Selector : ISelector
    {
        private static readonly Type InjectType = typeof(InjectAttribute);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public ConstructorInfo SelectConstructor(Type type)
        {
            return type.GetConstructors()
                .OrderByDescending(_ => _.IsDefined(InjectType) ? 1 : 0)
                .ThenByDescending(_ => _.GetParameters().Length)
                .FirstOrDefault();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IEnumerable<PropertyInfo> SelectProperties(Type type)
        {
            return type.GetProperties()
                .Where(_ => _.IsDefined(InjectType));
        }
    }
}
