namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class DictionaryRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/dictionary", StringComparison.OrdinalIgnoreCase);
        }

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

            var genericKeyType = parameter.ParameterType.GetIsGenericType()
                ? parameter.ParameterType.GenericTypeArguments[0]
                : null;

            string keyTypeValue;
            var keyType = context.ElementInfo.Attributes.TryGetValue("keyType", out keyTypeValue)
                ? Type.GetType(keyTypeValue, true)
                : genericKeyType;
            if (keyType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "List element need keyType attribute. path = [{0}]", context.Path));
            }

            var genericValueType = parameter.ParameterType.GetIsGenericType()
                ? parameter.ParameterType.GenericTypeArguments[1]
                : null;

            string valueTypeValue;
            var valueType = context.ElementInfo.Attributes.TryGetValue("valueType", out valueTypeValue)
                ? Type.GetType(valueTypeValue, true)
                : genericValueType;
            if (valueType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "List element need valueType attribute. path = [{0}]", context.Path));
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
