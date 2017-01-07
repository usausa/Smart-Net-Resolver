namespace Smart.Resolver.Builder.Handlers.Activators
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class PropertyHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var activator = context.PeekStack<ActivatorStack>();
            if (activator == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var name = context.ElementInfo.GetAttribute("name");
            if (String.IsNullOrEmpty(name))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Property element need name attribute. path = [{0}]", context.Path));
            }

            var pi = activator.TargetType.GetProperty(name);
            if (pi == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Property is not found. type = [{0}], name = [{1}]", activator.TargetType, name));
            }

            var propertyType = context.ElementInfo.GetAttributeAsType("type") ?? pi.PropertyType;

            var parameter = new ParameterStack(pi.Name, propertyType);

            var value = context.ElementInfo.GetAttribute("value");
            if (!String.IsNullOrEmpty(value))
            {
                var converter = context.Components.Get<IObjectConverter>();
                parameter.Value = converter.Convert(value, propertyType);
            }

            context.PushStack(parameter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var parameter = context.PopStack<ParameterStack>();
            var activator = context.PeekStack<ActivatorStack>();
            activator.PropertyValues[parameter.Name] = parameter.Value;
        }
    }
}
