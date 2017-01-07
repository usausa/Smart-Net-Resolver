namespace Smart.Resolver.Builder.Handlers.Bindings
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class ToProviderHandler : ElementHandlerBase
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

            var type = context.ElementInfo.GetAttributeAsType("type");

            context.PushStack(new ActivatorStack(type));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var activator = context.PopStack<ActivatorStack>();
            var binding = context.PeekStack<BindingStack>();

            var value = TypeHelper.ActivateInstance(activator.TargetType, activator.ConstructorArguments, activator.PropertyValues);

            var providerValue = value as IProvider;
            if (providerValue != null)
            {
                binding.ProviderFactory = c => providerValue;
                return;
            }

            var factory = value as IProviderFactory;
            if (factory != null)
            {
                binding.ProviderFactory = factory.Create;
                return;
            }

            binding.ProviderFactory = c => new ConstantProvider(value);
        }
    }
}
