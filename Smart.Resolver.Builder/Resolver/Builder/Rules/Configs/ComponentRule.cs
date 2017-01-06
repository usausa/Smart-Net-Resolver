namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ComponentRule : RuleBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path == "/config/component";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            string value;
            if (!context.ElementInfo.Attributes.TryGetValue("component", out value))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Component element need component attribute. path = [{0}]", context.Path));
            }

            var componentType = Type.GetType(value, true);

            var implementType = context.ElementInfo.Attributes.TryGetValue("implement", out value)
                ? Type.GetType(value, true)
                : null;

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
