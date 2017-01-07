﻿namespace Smart.Resolver.Builder.Handlers.Bindings
{
    using System;
    using System.Globalization;

    using Smart.Converter;
    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class WithMetadataHandler : ElementHandlerBase
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

            var key = context.ElementInfo.GetAttribute("key");
            if (key == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "With-Metadata element need key attribute. path = [{0}]", context.Path));
            }

            var type = context.ElementInfo.GetAttributeAsType("type");
            var value = context.ElementInfo.GetAttribute("value");

            var parameter = new ParameterStack(key, type);

            if (!String.IsNullOrEmpty(value))
            {
                if (type == null)
                {
                    throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "With-Metadata element need type attribute. path = [{0}]", context.Path));
                }

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
            var binding = context.PopStack<BindingStack>();
            binding.AddMetadata(parameter.Name, parameter.Value);
        }
    }
}
