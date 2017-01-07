namespace Smart.Resolver.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Smart.ComponentModel;
    using Smart.Converter;
    using Smart.Resolver.Builder.Handlers;
    using Smart.Resolver.Builder.Handlers.Activators;
    using Smart.Resolver.Builder.Handlers.Bindings;
    using Smart.Resolver.Builder.Handlers.Configs;
    using Smart.Resolver.Builder.Handlers.Parameters;
    using Smart.Resolver.Builder.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class XmlConfigBuilder
    {
        /// <summary>
        ///
        /// </summary>
        public ResolverConfig ResolverConfig { get; }

        /// <summary>
        ///
        /// </summary>
        public ComponentConfig ComponentConfig { get; } = new ComponentConfig();

        /// <summary>
        ///
        /// </summary>
        public IList<HandlerEntry> Entries { get; } = new List<HandlerEntry>();

        /// <summary>
        ///
        /// </summary>
        public XmlConfigBuilder()
            : this(new ResolverConfig())
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolverConfig"></param>
        public XmlConfigBuilder(ResolverConfig resolverConfig)
        {
            ResolverConfig = resolverConfig;

            // Components
            ComponentConfig.Add<IObjectConverter>(ObjectConverter.Default);

            ComponentConfig.Add<IScopeProcessor, TransientScopeProcessor>();
            ComponentConfig.Add<IScopeProcessor, SingletonScopeProcessor>();

            // Entries
            AddHandler(Mathcers.EndWith("/array"), new ArrayHandler());
            AddHandler(Mathcers.EndWith("/array/value"), new CollectionValueHandler());
            AddHandler(Mathcers.EndWith("/list"), new ListHandler());
            AddHandler(Mathcers.EndWith("/list/value"), new CollectionValueHandler());
            AddHandler(Mathcers.EndWith("/array"), new DictionaryHandler());
            AddHandler(Mathcers.EndWith("/array/entry"), new DictionaryEntryHandler());

            AddHandler(Mathcers.EndWith("/object"), new ObjectHandler());
            AddHandler(Mathcers.EndWith("/object/constructor-arg"), new ConstructorArgHandler());
            AddHandler(Mathcers.EndWith("/object/property"), new PropertyHandler());

            AddHandler(Mathcers.Equals("/config/component"), new ComponentHandler());
            AddHandler(Mathcers.Equals("/config/component/constructor-arg"), new ConstructorArgHandler());
            AddHandler(Mathcers.Equals("/config/component/property"), new PropertyHandler());

            AddHandler(Mathcers.Equals("/config/binding"), new BindingHandler());
            AddHandler(Mathcers.Equals("/config/binding/to-provider"), new ToProviderHandler());
            AddHandler(Mathcers.Equals("/config/binding/to-provider/constructor-arg"), new ConstructorArgHandler());
            AddHandler(Mathcers.Equals("/config/binding/to-provider/property"), new PropertyHandler());
            AddHandler(Mathcers.Equals("/config/binding/with-metadata"), new WithMetadataHandler());
            AddHandler(Mathcers.Equals("/config/binding/with-constructor-arg"), new WithConstructorArgHandler());
            AddHandler(Mathcers.Equals("/config/binding/with-constructor-arg/named"), new NamedHandler());
            AddHandler(Mathcers.Equals("/config/binding/with-proverty-value"), new WithPropertyValueHandler());
            AddHandler(Mathcers.Equals("/config/binding/with-proverty-value/named"), new NamedHandler());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var context = new BuilderContext(ResolverConfig, ComponentConfig.ToContainer());

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var isEmpty = reader.IsEmptyElement;

                    var name = reader.LocalName;
                    var attributes = new Dictionary<string, string>();
                    while (reader.MoveToNextAttribute())
                    {
                        attributes[reader.Name] = reader.Value;
                    }

                    context.AddElement(new ElementInfo(name, attributes));

                    var handler = FindHandler(context.Path);
                    handler?.OnBegin(context);

                    if (isEmpty)
                    {
                        handler?.OnEnd(context);

                        context.RemoveElement();
                    }
                }
                else if ((reader.NodeType == XmlNodeType.Text) || (reader.NodeType == XmlNodeType.CDATA))
                {
                    context.ElementInfo.AddBody(reader.Value);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    var handler = FindHandler(context.Path);
                    handler?.OnEnd(context);

                    context.RemoveElement();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="matcher"></param>
        /// <param name="handler"></param>
        private void AddHandler(Func<string, bool> matcher, IElementHandler handler)
        {
            Entries.Add(new HandlerEntry(matcher, handler));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IElementHandler FindHandler(string path)
        {
            return Entries.FirstOrDefault(handler => handler.Matcher(path))?.Handler;
        }
    }
}
