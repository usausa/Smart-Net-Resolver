namespace Smart.Resolver.Builder.Handlers.Configs
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ComponentHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var componentType = context.ElementInfo.GetAttributeAsType("component");
            if (componentType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Component element need component attribute. path = [{0}]", context.Path));
            }

            var implementType = context.ElementInfo.GetAttributeAsType("implement");

            context.PushStack(new ComponentStack(componentType, implementType));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var component = context.PopStack<ComponentStack>();

            if ((component.ConstructorArguments.Count > 0) || (component.PropertyValues.Count > 0))
            {
                var implement = TypeHelper.ActivateInstance(component.TargetType, component.ConstructorArguments, component.PropertyValues);
                context.ResolverConfig.Components.Add(component.ComponentType, implement);
            }
            else
            {
                if (component.ImplementType == null)
                {
                    context.ResolverConfig.Components.Add(component.ComponentType);
                }
                else
                {
                    context.ResolverConfig.Components.Add(component.ComponentType, component.ImplementType);
                }
            }
        }
    }
}
