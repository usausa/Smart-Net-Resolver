namespace Smart.Resolver.Builder.Handlers.Parameters
{
    using System;
    using System.Globalization;

    using Smart.Collections.Generic;
    using Smart.Converter;
    using Smart.Functional;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class DictionaryHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var parameter = context.PeekStack<ParameterStack>();
            if (parameter == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var genericKeyType = parameter.ParameterType?.GetIsGenericType() ?? false
                ? parameter.ParameterType.GenericTypeArguments[0]
                : null;
            var genericValueType = parameter.ParameterType?.GetIsGenericType() ?? false
                ? parameter.ParameterType.GenericTypeArguments[1]
                : null;

            var keyType = context.ElementInfo.GetAttributeAsType("keyType") ?? genericKeyType;
            if (keyType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Dictionary element need keyType attribute. path = [{0}]", context.Path));
            }

            var valueType = context.ElementInfo.GetAttributeAsType("valueType") ?? genericValueType;
            if (valueType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Dictionary element need valueType attribute. path = [{0}]", context.Path));
            }

            context.PushStack(new DictionaryStack(
                TypeHelper.CreateDictionary(genericKeyType ?? keyType, genericValueType ?? valueType),
                keyType,
                valueType,
                context.Components.Get<IObjectConverter>()));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var dictionary = context.PopStack<DictionaryStack>();
            var parameter = context.PeekStack<ParameterStack>();
            parameter.Value = dictionary.Dictionary;
        }
    }
}
