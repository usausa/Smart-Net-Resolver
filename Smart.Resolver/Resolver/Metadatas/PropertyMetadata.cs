﻿namespace Smart.Resolver.Metadatas
{
    using System;

    using Smart.Resolver.Constraints;

    /// <summary>
    ///
    /// </summary>
    public class PropertyMetadata
    {
        public string Name { get; }

        public Type PropertyType { get; }

        public Action<object, object> Setter { get; }

        public IConstraint Constraint { get; }

        public PropertyMetadata(string name, Type propertyType, Action<object, object> setter, IConstraint constraint)
        {
            Name = name;
            PropertyType = propertyType;
            Setter = setter;
            Constraint = constraint;
        }
    }
}
