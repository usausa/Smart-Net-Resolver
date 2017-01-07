namespace Smart.Resolver.Builder.Handlers.Activators
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ConstructorArgHandler : ElementHandlerBase
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
            var type = context.ElementInfo.GetAttributeAsType("type");
            if (String.IsNullOrEmpty(name) && (type == null))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor element need name or type attribute. type = [{0}]", activator.TargetType));
            }

            if (type == null)
            {
                var types = TypeHelper.ResolveConstructorArtumentType(activator.TargetType, name);
                if (types.Length == 0)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor parameter is not found. type = [{0}], name = [{1}]", activator.TargetType, name));
                }

                if (types.Length > 1)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor parameter is matched multiple types. type = [{0}], name = [{1}]", activator.TargetType, name));
                }

                type = types[0];
            }

            var value = context.ElementInfo.GetAttribute("value");

            var parameter = new ParameterStack(name, type);

            if (!String.IsNullOrEmpty(value))
            {
                var converter = context.Components.Get<IObjectConverter>();
                parameter.Value = converter.Convert(value, type);
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
            activator.AddConstructorArgument(parameter.Name, parameter.Value);
        }
    }
}
