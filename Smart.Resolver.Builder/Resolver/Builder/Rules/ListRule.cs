namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ListRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/list", StringComparison.OrdinalIgnoreCase);
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

            var genericType = parameter.ParameterType.GetIsGenericType()
                ? parameter.ParameterType.GenericTypeArguments[0]
                : null;

            string valueTypeName;
            var valueType = context.ElementInfo.Attributes.TryGetValue("valueType", out valueTypeName)
                ? Type.GetType(valueTypeName, true)
                : genericType;
            if (valueType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "List element need valueType attribute. path = [{0}]", context.Path));
            }

            context.PushStack(new ListStack(
                TypeHelper.CreateList(genericType ?? valueType),
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
            var list = context.PopStack<ListStack>();
            var parameter = context.PeekStack<ParameterStack>();
            parameter.Value = list.List;
        }
    }
}
