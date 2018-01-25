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
    using Smart.Resolver.Helpers;

    /// <summary>
    ///
    /// </summary>
    public sealed class MetadataFactory : IMetadataFactory
    {
        private static readonly Type InjectType = typeof(InjectAttribute);

        private readonly IDelegateFactory delegateFactory;

        private readonly ConcurrentDictionary<Type, TypeMetadata> metadatas = new ConcurrentDictionary<Type, TypeMetadata>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="delegateFactory"></param>
        public MetadataFactory(IDelegateFactory delegateFactory)
        {
            this.delegateFactory = delegateFactory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public TypeMetadata GetMetadata(Type type)
        {
            if (metadatas.TryGetValue(type, out var metadata))
            {
                return metadata;
            }

            var constructors = type.GetConstructors()
                .Where(c => !c.IsStatic)
                .OrderByDescending(c => c.IsDefined(InjectType) ? 1 : 0)
                .ThenByDescending(c => c.GetParameters().Length)
                .ThenByDescending(c => c.GetParameters().Count(p => p.HasDefaultValue))
                .Select(CreateConstructorMetadata)
                .ToArray();

            var properties = type.GetProperties()
                .Where(p => p.IsDefined(InjectType))
                .Select(CreatePropertyMetadata)
                .ToArray();

            metadata = new TypeMetadata(constructors, properties);

            metadatas[type] = metadata;

            return metadata;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        private static ConstructorMetadata CreateConstructorMetadata(ConstructorInfo ci)
        {
            var parameters = ci.GetParameters()
                .Select(CreateParameterMetadata)
                .ToArray();

            var constraints = ci.GetParameters()
                .Select(p => CreateConstraint(p.GetCustomAttributes<ConstraintAttribute>()))
                .ToArray();

            return new ConstructorMetadata(ci, parameters, constraints);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static ParameterMetadata CreateParameterMetadata(ParameterInfo pi)
        {
            return new ParameterMetadata(pi, TypeHelper.GetEnumerableElementType(pi.ParameterType));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private PropertyMetadata CreatePropertyMetadata(PropertyInfo pi)
        {
            return new PropertyMetadata(
                pi.Name,
                delegateFactory.GetExtendedPropertyType(pi),
                delegateFactory.CreateSetter(pi, true),
                CreateConstraint(pi.GetCustomAttributes<ConstraintAttribute>()));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private static IConstraint CreateConstraint(IEnumerable<ConstraintAttribute> attributes)
        {
            var constraints = attributes
                .Select(a => a.CreateConstraint())
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
