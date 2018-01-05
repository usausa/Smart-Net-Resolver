namespace Smart.Resolver.Factories
{
    using System;

    public sealed class ArrayObjectFactory : IObjectFactory
    {
        private readonly Type elementType;

        private readonly IObjectFactory[] objectFactories;

        public ArrayObjectFactory(Type elementType, IObjectFactory[] objectFactories)
        {
            this.elementType = elementType;
            this.objectFactories = objectFactories;
        }

        public object Create()
        {
            var array = Array.CreateInstance(elementType, objectFactories.Length);
            for (var i = 0; i < objectFactories.Length; i++)
            {
                array.SetValue(objectFactories[i].Create(), i);
            }

            return array;
        }
    }
}
