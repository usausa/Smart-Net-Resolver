namespace NewFactoryWork
{
    using System;

    using NewFactoryLib;

    class Program
    {
        private static readonly object[] EmptyFactories = new object[0];

        private static readonly object[] EmptyActions = new object[0];

        static void Main()
        {
            var builder = new Builder();

            var factoryP1 = (Func<IContainer, object>)(x => "a");
            var factoryP2 = (Func<IContainer, object>)(x => 1);
            var factoryData2 = builder.To(typeof(Data2).GetConstructors()[0], new[] { factoryP1, factoryP2 }, EmptyActions);
            var data2 = ((Func<IContainer, Data2>)factoryData2)(null);

            // Actions
            var factoryData = builder.To(typeof(Data).GetConstructors()[0], EmptyFactories, new[]
            {
                ActionBuilder.Create1(), ActionBuilder.Create2()
            });
            var data = ((Func<IContainer, Data>)factoryData)(null);

            // Factory
            var factorySingleton = builder.To(typeof(Singleton).GetConstructors()[0], EmptyFactories, EmptyActions);
            var singleton = ((Func<IContainer, Singleton>)factorySingleton)(null);
            var singleton2 = ((Func<IContainer, ISingleton>)factorySingleton)(null);

            var factoryTransient = builder.To(typeof(Transient).GetConstructors()[0], EmptyFactories, EmptyActions);
            var transient = ((Func<IContainer, Transient>)factoryTransient)(null);

            var factoryCombined = builder.To(typeof(Combined).GetConstructors()[0], new[] { factorySingleton }, EmptyActions);
            var combined = ((Func<IContainer, Combined>)factoryCombined)(null);

            var factoryComplex = builder.To(typeof(Complex).GetConstructors()[0], new[] { factoryTransient, factoryCombined }, EmptyActions);
            var complex = ((Func<IContainer, Complex>)factoryComplex)(null);
            var complex2 = ((Func<IContainer, object>)factoryComplex)(null);

            // Array
            var fs1 = (Func<IContainer, Service1>)(c => new Service1());
            var fs2 = (Func<IContainer, Service2>)(c => new Service2());
            var fs3 = (Func<IContainer, Service3>)(c => new Service3());
            var factoryServiceArray = builder.ToArray(typeof(IService), fs1, fs2, fs3);
            var services = ((Func<IContainer, IService[]>)factoryServiceArray)(null);
        }
    }
}
