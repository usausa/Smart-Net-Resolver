namespace Smart.Resolver.Metadatas
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.Reflection;
    using Smart.Resolver.Attributes;
    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class MetadataFactory : IMetadataFactory
    {
        private readonly ConcurrentDictionary<Type, TypeMetadata> metadatas = new ConcurrentDictionary<Type, TypeMetadata>();

        public ISelector Selector { get; set; } = new Selector();

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public TypeMetadata GetMetadata(Type type)
        {
            TypeMetadata metadata;
            if (metadatas.TryGetValue(type, out metadata))
            {
                return metadata;
            }

            var constructor = Selector.SelectConstructor(type);
            var constructorConstraint = constructor?.GetParameters()
                .Select(_ => CreateConstraint(_.GetCustomAttributes<ConstraintAttribute>()))
                .ToList();

            var properties = Selector.SelectProperties(type)
                .Select(_ => new PropertyMetadata(_.ToAccessor(), CreateConstraint(_.GetCustomAttributes<ConstraintAttribute>())))
                .ToList();

            metadata = new TypeMetadata(new ConstructorMetadata(constructor, constructorConstraint), properties);

            metadatas[type] = metadata;

            return metadata;
        }

        protected virtual IConstraint CreateConstraint(IEnumerable<ConstraintAttribute> attributes)
        {
            var constraints = attributes
                .Select(_ => _.CreateConstraint())
                .ToArray();

            if (constraints.Length == 0)
            {
                return null;
            }

            if (constraints.Length == 1)
            {
                return constraints[0];
            }

            return new ChainConstraint(constraints);
        }
    }
}
