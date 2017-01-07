namespace Smart.Resolver.Builder.Handlers.Bindings
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Smart.Resolver.Builder.Scopes;
    using Smart.Resolver.Builder.Stacks;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class BindingHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var bindType = context.ElementInfo.GetAttributeAsType("bind");
            if (bindType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Binding element need bind attribute. path = [{0}]", context.Path));
            }

            var binding = new BindingStack(bindType);

            var toType = context.ElementInfo.GetAttributeAsType("to");
            if (toType != null)
            {
                binding.ProviderFactory = c => new StandardProvider(toType, c);
            }

            var scope = context.ElementInfo.GetAttribute("scope");
            if (!String.IsNullOrEmpty(scope))
            {
                var processor = context.Components.GetAll<IScopeProcessor>()
                    .FirstOrDefault(p => String.Equals(p.Name, scope, StringComparison.OrdinalIgnoreCase));
                if (processor == null)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Binding element scope attribute is invalid. scope = [{0}]", scope));
                }

                binding.ScopeProcessor = processor;
            }

            var name = context.ElementInfo.GetAttribute("name");
            if (!String.IsNullOrEmpty(name))
            {
                binding.Name = name;
            }

            context.PushStack(binding);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var binding = context.PopStack<BindingStack>();

            var toSyntax = context.ResolverConfig.Bind(binding.ComponentType);

            var inSyntax = binding.ProviderFactory != null
                ? toSyntax.ToProvider(binding.ProviderFactory)
                : toSyntax.ToSelf();

            var withSyntax = binding.ScopeProcessor != null
                ? binding.ScopeProcessor.Process(inSyntax)
                : inSyntax;

            if (!String.IsNullOrEmpty(binding.Name))
            {
                withSyntax.Named(binding.Name);
            }

            if (binding.Metadatas != null)
            {
                foreach (var key in binding.Metadatas.Keys)
                {
                    withSyntax.WithMetadata(key, binding.Metadatas[key]);
                }
            }

            if (binding.ConstructorArgumentFactories != null)
            {
                foreach (var name in binding.ConstructorArgumentFactories.Keys)
                {
                    withSyntax.WithConstructorArgument(name, binding.ConstructorArgumentFactories[name]);
                }
            }

            if (binding.PropertyValueFactories != null)
            {
                foreach (var name in binding.PropertyValueFactories.Keys)
                {
                    withSyntax.WithPropertyValue(name, binding.PropertyValueFactories[name]);
                }
            }
        }
    }
}
