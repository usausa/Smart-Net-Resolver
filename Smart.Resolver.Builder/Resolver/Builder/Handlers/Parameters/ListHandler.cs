namespace Smart.Resolver.Builder.Handlers.Parameters
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ListHandler : ElementHandlerBase
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

            var genericType = parameter.ParameterType?.GetIsGenericType() ?? false
                ? parameter.ParameterType.GenericTypeArguments[0]
                : null;

            var valueType = context.ElementInfo.GetAttributeAsType("valueType") ?? genericType;
            if (valueType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "List element need valueType attribute. path = [{0}]", context.Path));
            }

            context.PushStack(new CollectionStack(
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
            var list = context.PopStack<CollectionStack>();
            var parameter = context.PeekStack<ParameterStack>();
            parameter.Value = list.List;
        }
    }
}
