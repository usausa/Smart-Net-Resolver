namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ObjectConstructorArgRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/object/constructor-arg", StringComparison.OrdinalIgnoreCase);
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
            context.ElementInfo.Attributes.TryGetValue("name", out name);

            string type;
            context.ElementInfo.Attributes.TryGetValue("type", out type);

            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(type))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor element need name or type attribute. type = [{0}]", @object.TargetType));
            }

            Type parameterType;
            if (!String.IsNullOrEmpty(type))
            {
                parameterType = Type.GetType(type, true);
            }
            else
            {
                var types = TypeHelper.ResolveConstructorArtumentType(@object.TargetType, name);
                if (types.Length == 0)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor parameter is not found. type = [{0}], name = [{1}]", @object.TargetType, name));
                }

                if (types.Length > 1)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Constructor parameter is matched multiple types. type = [{0}], name = [{1}]", @object.TargetType, name));
                }

                parameterType = types[0];
            }

            context.PushStack(new ParameterStack(name, parameterType));
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
            @object.ConstructorArguments.Add(new KeyValuePair<string, object>(parameter.Name, parameter.Value));
        }
    }
}
