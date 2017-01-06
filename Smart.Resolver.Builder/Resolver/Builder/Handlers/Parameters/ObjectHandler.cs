namespace Smart.Resolver.Builder.Handlers.Parameters
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ObjectHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var parameter = context.PeekStack<IParameterStack>();
            if (parameter == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var type = context.ElementInfo.GetAttributeAsType("type") ?? parameter.ParameterType;
            if (type == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Object element need type attribute. path = [{0}]", context.Path));
            }

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
            var parameter = context.PeekStack<IParameterStack>();
            parameter.SetValue(TypeHelper.ActivateInstance(activator.TargetType, activator.ConstructorArguments, activator.PropertyValues));
        }
    }
}
