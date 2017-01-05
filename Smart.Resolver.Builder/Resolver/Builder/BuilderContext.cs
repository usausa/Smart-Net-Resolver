namespace Smart.Resolver.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public class BuilderContext : IBuilderContext
    {
        private readonly Stack<ElementInfo> elements = new Stack<ElementInfo>();

        private readonly Stack<object> objects = new Stack<object>();

        /// <summary>
        ///
        /// </summary>
        public string Path { get; private set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public ElementInfo ElementInfo => elements.Peek();

        /// <summary>
        ///
        /// </summary>
        public ResolverConfig ResolverConfig { get; }

        /// <summary>
        ///
        /// </summary>
        public IComponentContainer Components { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolverConfig"></param>
        /// <param name="components"></param>
        public BuilderContext(ResolverConfig resolverConfig, IComponentContainer components)
        {
            ResolverConfig = resolverConfig;
            Components = components;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public void AddElement(ElementInfo element)
        {
            elements.Push(element);

            var sb = new StringBuilder(Path.Length + 1 + element.Name.Length);
            sb.Append(Path);
            sb.Append("/");
            sb.Append(element.Name);
            Path = sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        public void RemoveElement()
        {
            elements.Pop();

            var index = Path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);
            Path = index >= 0 ? Path.Substring(0, index) : string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object PeekObject()
        {
            return objects.Count > 0 ? objects.Peek() : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object PopObject()
        {
            return objects.Count > 0 ? objects.Pop() : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public object FindObject(Func<object, bool> predicate)
        {
            return objects.FirstOrDefault(predicate);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void PushObject(object value)
        {
            objects.Push(value);
        }
    }
}
