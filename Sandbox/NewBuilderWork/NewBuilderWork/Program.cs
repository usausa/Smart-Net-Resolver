namespace NewBuilderWork
{
    using System;
    using Smart.Resolver.Builders;

    public static class Program
    {
        private static readonly Func<IResolver, object>[] EmptyFactories = new Func<IResolver, object>[0];

        private static readonly Action<IResolver, object>[] EmptyActions = new Action<IResolver, object>[0];

        public static void Main()
        {
            var builder = new EmitFactoryBuilder();

            var factoryP1 = (Func<IResolver, object>)(x => "a");
            var factoryP2 = (Func<IResolver, object>)(x => 1);
            var factoryData2 = builder.CreateFactory(typeof(Data2).GetConstructors()[0], new[] { factoryP1, factoryP2 }, EmptyActions);
            var data2 = factoryData2(null);

            // Actions
            var factoryData = builder.CreateFactory(typeof(Data).GetConstructors()[0], EmptyFactories, new[]
            {
                ActionBuilder.Create1(), ActionBuilder.Create2()
            });
            var data = factoryData(null);

            // Factory
            var factorySingleton = Scope.ToSingleton(null, builder.CreateFactory(typeof(Singleton).GetConstructors()[0], EmptyFactories, EmptyActions));
            var singleton = factorySingleton(null);
            var singleton2 = factorySingleton(null);

            var factoryTransient = builder.CreateFactory(typeof(Transient).GetConstructors()[0], EmptyFactories, EmptyActions);
            var transient = factoryTransient(null);

            var factoryCombined = builder.CreateFactory(typeof(Combined).GetConstructors()[0], new[] { factorySingleton }, EmptyActions);
            var combined = factoryCombined(null);

            var factoryComplex = builder.CreateFactory(typeof(Complex).GetConstructors()[0], new[] { factoryTransient, factoryCombined }, EmptyActions);
            var complex = factoryComplex(null);
            var complex2 = factoryComplex(null);

            // Array
            var fs1 = (Func<IResolver, Service1>)(c => new Service1());
            var fs2 = (Func<IResolver, Service2>)(c => new Service2());
            var fs3 = (Func<IResolver, Service3>)(c => new Service3());
            var factoryServiceArray = builder.CreateArrayFactory(typeof(IService), new Func<IResolver, object>[] { fs1, fs2, fs3 });
            var services = factoryServiceArray(null);
        }
    }
}
