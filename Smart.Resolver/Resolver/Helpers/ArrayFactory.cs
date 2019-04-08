namespace Smart.Resolver.Helpers
{
    using System;

    public static class ArrayFactory
    {
        public static Func<object> Create(Func<int, Array> arrayAllocator, Func<object>[] factories)
        {
            return () =>
            {
                var array = arrayAllocator(factories.Length);
                var objects = (object[])array;
                for (var i = 0; i < factories.Length; i++)
                {
                    objects[i] = factories[i]();
                }

                return array;
            };
        }
    }
}
