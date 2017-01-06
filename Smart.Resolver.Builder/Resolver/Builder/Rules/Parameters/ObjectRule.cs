namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    public class ObjectRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/object", StringComparison.OrdinalIgnoreCase);
        }

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

            string value;
            var targetType = context.ElementInfo.Attributes.TryGetValue("type", out value)
                ? Type.GetType(value, true)
                : parameter.ParameterType;
            if (targetType == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Object element need type attribute. path = [{0}]", context.Path));
            }

            context.PushStack(new ObjectStack(targetType));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var @object = context.PopStack<ObjectStack>();
            var parameter = context.PeekStack<ParameterStack>();
            parameter.Value = TypeHelper.ActivateInstance(@object.TargetType, @object.ConstructorArguments, @object.PropertyValues);
        }
    }
}
