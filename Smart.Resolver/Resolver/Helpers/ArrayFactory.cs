namespace Smart.Resolver.Helpers
{
    using System;

    public static class ArrayFactory
    {
        public static Func<IKernel, object> Create(Func<int, Array> arrayAllocator, Func<IKernel, object>[] factories)
        {
            return k =>
            {
                var array = arrayAllocator(factories.Length);
                var objects = (object[])array;
                for (var i = 0; i < factories.Length; i++)
                {
                    objects[i] = factories[i](k);
                }

                return array;
            };
        }
    }
}
