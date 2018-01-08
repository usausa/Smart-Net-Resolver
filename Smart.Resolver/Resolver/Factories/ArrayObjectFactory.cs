namespace Smart.Resolver.Factories
{
    using Smart.Reflection;

    public sealed class ArrayObjectFactory : IObjectFactory
    {
        private readonly IArrayOperator arrayOperator;

        private readonly IObjectFactory[] objectFactories;

        public ArrayObjectFactory(IArrayOperator arrayOperator, IObjectFactory[] objectFactories)
        {
            this.arrayOperator = arrayOperator;
            this.objectFactories = objectFactories;
        }

        public object Create()
        {
            var array = arrayOperator.Create(objectFactories.Length);
            var objs = (object[])array;
            for (var i = 0; i < objectFactories.Length; i++)
            {
                objs[i] = objectFactories[i].Create();
            }

            return array;
        }
    }
}
