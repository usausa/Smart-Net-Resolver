namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ObjectPropertyRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/object/property", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext context)
        {
            var @object = context.PeekStack<ObjectStack>();
            if (@object == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            string name;
            if (!context.ElementInfo.Attributes.TryGetValue("name", out name))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Property element need key attribute. path = [{0}]", context.Path));
            }

            var pi = @object.TargetType.GetProperty(name);
            if (pi == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Property is not found. type = [{0}], name = [{1}]", @object.TargetType, name));
            }

            context.PushStack(new ParameterStack(pi.Name, pi.PropertyType));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var parameter = context.PopStack<ParameterStack>();
            var @object = context.PeekStack<ObjectStack>();
            @object.PropertyValues[parameter.Name] = parameter.Value;
        }
    }
}
