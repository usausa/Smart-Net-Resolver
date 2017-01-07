namespace Smart.Resolver.Builder.Handlers.Bindings
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;
    using Smart.Resolver.Parameters;

    /// <summary>
    ///
    /// </summary>
    public class WithConstructorArgHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var binding = context.PeekStack<BindingStack>();
            if (binding == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var name = context.ElementInfo.GetAttribute("name");
            if (String.IsNullOrEmpty(name))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "With-Constructo-Arg element need name attribute. path = [{0}]", context.Path));
            }

            var type = context.ElementInfo.GetAttributeAsType("type");
            var value = context.ElementInfo.GetAttribute("value");

            var parameter = new ParameterStack(name, type);

            if (!String.IsNullOrEmpty(value))
            {
                if (type == null)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "With-Constructo-Arg element need type attribute. path = [{0}]", context.Path));
                }

                var converter = context.Components.Get<IObjectConverter>();
                parameter.Value = new ConstantParameter(converter.Convert(value, type));
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
            var binding = context.PopStack<BindingStack>();

            var parameterValue = parameter.Value as IParameter;
            if (parameterValue != null)
            {
                binding.ConstructorArgumentFactories[parameter.Name] = c => parameterValue;
                return;
            }

            var factory = parameter.Value as IParameterFactory;
            if (factory != null)
            {
                binding.ConstructorArgumentFactories[parameter.Name] = factory.Create;
                return;
            }

            binding.ConstructorArgumentFactories[parameter.Name] = c => new ConstantParameter(parameter.Value);
        }
    }
}
