namespace Smart.Resolver.Builder.Handlers.Bindings
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class NamedHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var parameter = context.PeekStack<ParameterStack>();
            if (parameter == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var type = context.ElementInfo.GetAttributeAsType("type") ?? parameter.ParameterType;
            if (type == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Named element need type attribute. path = [{0}]", context.Path));
            }

            var value = context.ElementInfo.GetAttribute("value");
            if (String.IsNullOrEmpty(value))
            {
                value = context.ElementInfo.Body;
            }

            parameter.Value = new NamedParameter(type, value);
        }
    }
}
