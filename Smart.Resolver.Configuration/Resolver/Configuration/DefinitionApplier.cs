namespace Smart.Resolver.Configuration;

using System.ComponentModel;

using Smart.Resolver.Constraints;
using Smart.Resolver.Expressions;

internal static class DefinitionApplier
{
    //--------------------------------------------------------------------------------
    // Apply
    //--------------------------------------------------------------------------------

    private static Type DefaultTypeResolver(string typeName) =>
        Type.GetType(typeName) ?? throw new InvalidOperationException($"Type could not be resolved. type=[{typeName}]");

    internal static void Apply(
        ResolverConfig config,
        ResolverDefinition definition,
        Func<string, Type>? typeResolver)
    {
        var resolveType = typeResolver ?? DefaultTypeResolver;

        for (var i = 0; i < definition.Bindings.Count; i++)
        {
            ApplyBinding(config, definition.Bindings[i], i, resolveType);
        }
    }

    private static void ApplyBinding(
        ResolverConfig config,
        BindingDefinition binding,
        int index,
        Func<string, Type> resolveType)
    {
        if (String.IsNullOrWhiteSpace(binding.ServiceType))
        {
            throw new ResolverDefinitionException(index, "ServiceType", "ServiceType is required.");
        }

        Type serviceType;
        try
        {
            serviceType = resolveType(binding.ServiceType);
        }
        catch (Exception ex)
        {
            throw new ResolverDefinitionException(index, "ServiceType", $"Type could not be resolved. type=[{binding.ServiceType}].", ex);
        }

        // To
        var toSyntax = binding.TargetKind switch
        {
            BindingTargetKind.Self => config.Bind(serviceType).ToSelf(),
            BindingTargetKind.Type => ResolveTypeTo(config, binding, index, serviceType, resolveType),
            BindingTargetKind.Constant => ResolveConstantTo(config, binding, index, serviceType, resolveType),
            _ => config.Bind(serviceType).ToSelf()
        };

        // In (Scope)
        var constraintSyntax = binding.Scope switch
        {
            ScopeKind.Singleton => toSyntax.InSingletonScope(),
            ScopeKind.Container => toSyntax.InContainerScope(),
            _ => toSyntax.InTransientScope()
        };

        // Constraint
        var withSyntax = binding.ConstraintKey is not null ?
            constraintSyntax.Constraint(new KeyConstraint(binding.ConstraintKey)) :
            constraintSyntax;

        // Metadata
        for (var i = 0; i < binding.Metadata.Count; i++)
        {
            var entry = binding.Metadata[i];
            if (String.IsNullOrWhiteSpace(entry.Key))
            {
                throw new ResolverDefinitionException(index, $"Metadata[{i}].Key", "Key is required.");
            }

            var metaValue = entry.ValueType is not null
                ? ConvertValue(entry.Value, ResolveValueType(resolveType, entry.ValueType, index, $"Metadata[{i}].ValueType"), index, $"Metadata[{i}].Value")
                : entry.Value;
            withSyntax = withSyntax.WithMetadata(entry.Key, metaValue);
        }

        // ConstructorArguments
        withSyntax = ApplyParameters(withSyntax, binding.ConstructorArguments, "ConstructorArguments", index, resolveType,
            static (s, n, v) => s.WithConstructorArgument(n, v),
            static (s, n, f) => s.WithConstructorArgument(n, f));

        // PropertyValues
        ApplyParameters(withSyntax, binding.PropertyValues, "PropertyValues", index, resolveType,
            static (s, n, v) => s.WithPropertyValue(n, v),
            static (s, n, f) => s.WithPropertyValue(n, f));
    }

    //--------------------------------------------------------------------------------
    // To
    //--------------------------------------------------------------------------------

    private static IBindingInConstraintWithSyntax ResolveTypeTo(
        ResolverConfig config,
        BindingDefinition binding,
        int index,
        Type serviceType,
        Func<string, Type> resolveType)
    {
        if (binding.ImplementationType is null)
        {
            throw new ResolverDefinitionException(index, "ImplementationType", "ImplementationType is required when TargetKind is Type.");
        }

        try
        {
            var type = resolveType(binding.ImplementationType);
            return config.Bind(serviceType).To(type);
        }
        catch (Exception ex)
        {
            throw new ResolverDefinitionException(index, "ImplementationType", $"Type could not be resolved. type=[{binding.ImplementationType}].", ex);
        }
    }

    private static IBindingInConstraintWithSyntax ResolveConstantTo(
        ResolverConfig config,
        BindingDefinition binding,
        int index,
        Type serviceType,
        Func<string, Type> resolveType)
    {
        if (binding.ConstantValue is null)
        {
            throw new ResolverDefinitionException(index, "ConstantValue", "ConstantValue is required when TargetKind is Constant.");
        }

        var targetType = binding.ConstantValueType is not null
            ? ResolveValueType(resolveType, binding.ConstantValueType, index, "ConstantValueType")
            : serviceType;

        var converted = ConvertValue(binding.ConstantValue, targetType, index, "ConstantValue");
        return config.Bind(serviceType).ToConstant(converted);
    }

    //--------------------------------------------------------------------------------
    // Parameter
    //--------------------------------------------------------------------------------

    private static IBindingWithSyntax ApplyParameters(
        IBindingWithSyntax syntax,
        List<ParameterEntry> entries,
        string fieldPrefix,
        int index,
        Func<string, Type> resolveType,
        Func<IBindingWithSyntax, string, object?, IBindingWithSyntax> applyConstant,
        Func<IBindingWithSyntax, string, Func<IResolver, object?>, IBindingWithSyntax> applyReference)
    {
        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            if (String.IsNullOrWhiteSpace(entry.Name))
            {
                throw new ResolverDefinitionException(index, $"{fieldPrefix}[{i}].Name", "Name is required.");
            }

            if (entry.Kind == ParameterKind.Reference)
            {
                if (entry.Value is null)
                {
                    throw new ResolverDefinitionException(index, $"{fieldPrefix}[{i}].Value", "Value (reference type name) is required for Reference kind.");
                }

                var refTypeName = entry.Value;
                Type refType;
                try
                {
                    refType = resolveType(refTypeName);
                }
                catch (Exception ex)
                {
                    throw new ResolverDefinitionException(index, $"{fieldPrefix}[{i}].Value", $"Type could not be resolved. type=[{refTypeName}].", ex);
                }

                syntax = applyReference(syntax, entry.Name, x => x.Get(refType));
            }
            else
            {
                var value = entry.ValueType is not null
                    ? ConvertValue(entry.Value, ResolveValueType(resolveType, entry.ValueType, index, $"{fieldPrefix}[{i}].ValueType"), index, $"{fieldPrefix}[{i}].Value")
                    : entry.Value;
                syntax = applyConstant(syntax, entry.Name, value);
            }
        }

        return syntax;
    }

    //--------------------------------------------------------------------------------
    // Value
    //--------------------------------------------------------------------------------

    private static Type ResolveValueType(Func<string, Type> resolveType, string typeName, int index, string fieldName)
    {
        try
        {
            return resolveType(typeName);
        }
        catch (Exception ex)
        {
            throw new ResolverDefinitionException(index, fieldName, $"Type could not be resolved. type=[{typeName}].", ex);
        }
    }

    private static object ConvertValue(string? value, Type targetType, int index, string fieldName)
    {
        if (targetType == typeof(string))
        {
            return value ?? string.Empty;
        }

        if (value is null)
        {
            throw new ResolverDefinitionException(index, fieldName, $"Value is required for type. targetType=[{targetType}].");
        }

        try
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(typeof(string)))
            {
                return converter.ConvertFromInvariantString(value) ??
                       throw new ResolverDefinitionException(index, fieldName, $"Conversion of '{value}' to '{targetType}' returned null.");
            }

            return Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (ResolverDefinitionException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ResolverDefinitionException(index, fieldName, $"Failed to convert '{value}' to type '{targetType}'.", ex);
        }
    }
}
