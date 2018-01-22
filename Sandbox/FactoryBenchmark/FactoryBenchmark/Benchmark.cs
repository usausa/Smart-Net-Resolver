using System.Runtime.CompilerServices;

namespace FactoryBenchmark
{
    using System;

    using BenchmarkDotNet.Attributes;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly IObjectFactory objectFactory1 = CreateComplexActivator1();

        private readonly IObjectFactory objectFactory2 = CreateComplexActivator2();

        private readonly Func<object> objectFactory3 = CreateComplexActivator3();

        private readonly Func<object> objectFactory4 = CreateComplexActivator4();

        [Benchmark]
        public object Factory1()
        {
            return objectFactory1.Create();
        }

        [Benchmark]
        public object Factory2()
        {
            return objectFactory2.Create();
        }

        [Benchmark]
        public object Factory3()
        {
            return objectFactory3();
        }

        [Benchmark]
        public object Factory4()
        {
            return objectFactory4();
        }

        public static IObjectFactory CreateComplexActivator1()
        {
            var factoryService1 = new ConstantObjectFactory(new Service1());
            var factoryService2 = new ConstantObjectFactory(new Service2());

            var factoryUsecase1 = new ActivatorObjectFactory(
                new Usecase1Activator(),
                factoryService1);
            var factoryUsecase2 = new ActivatorObjectFactory(
                new Usecase2Activator(),
                factoryService2);

            var factoryTransient1 = new ActivatorObjectFactory(
                new Transient1Activator());
            var factoryTransient2 = new ActivatorObjectFactory(
                new Transient2Activator());

            var complexFactory = new ActivatorObjectFactory(
                new ComplexActivator(),
                factoryService1,
                factoryService2,
                factoryUsecase1,
                factoryUsecase2,
                factoryTransient1,
                factoryTransient2);

            return complexFactory;
        }

        public static IObjectFactory CreateComplexActivator2()
        {
            var factoryService1 = new ConstantObjectFactory(new Service1());
            var factoryService2 = new ConstantObjectFactory(new Service2());

            var factoryUsecase1 = new ActivatorObjectFactory1(
                new Usecase1Activator1(),
                factoryService1);
            var factoryUsecase2 = new ActivatorObjectFactory1(
                new Usecase2Activator1(),
                factoryService2);

            var factoryTransient1 = new ActivatorObjectFactory0(
                new Transient1Activator0());
            var factoryTransient2 = new ActivatorObjectFactory0(
                new Transient2Activator0());

            var complexFactory = new ActivatorObjectFactory6(
                new ComplexActivator6(),
                factoryService1,
                factoryService2,
                factoryUsecase1,
                factoryUsecase2,
                factoryTransient1,
                factoryTransient2);

            return complexFactory;
        }

        public static Func<object> CreateComplexActivator3()
        {
            var service1 = new Service1();
            var service2 = new Service2();

            var factoryService1 = (Func<object>)(() => service1);
            var factoryService2 = (Func<object>)(() => service2);

            var usecase1Activator = new Usecase1Activator1();
            var factoryUsecase1 = (Func<object>)(() => usecase1Activator.Create(factoryService1()));
            var usecase2Activator = new Usecase2Activator1();
            var factoryUsecase2 = (Func<object>)(() => usecase2Activator.Create(factoryService2()));

            var transient1Activator = new Transient1Activator0();
            var factoryTransient1 = (Func<object>)(() => transient1Activator.Create());
            var transient2Activator = new Transient2Activator0();
            var factoryTransient2 = (Func<object>)(() => transient2Activator.Create());

            var complexActivator = new ComplexActivator6();
            var complexFactory = (Func<object>) (() => complexActivator.Create(
                factoryService1(),
                factoryService2(),
                factoryUsecase1(),
                factoryUsecase2(),
                factoryTransient1(),
                factoryTransient2()));

            return complexFactory;
        }

        public static Func<object> CreateComplexActivator4()
        {
            var service1 = new Service1();
            var service2 = new Service2();

            var factoryService1 = (Func<object>)(() => service1);
            var factoryService2 = (Func<object>)(() => service2);

            var factoryUsecase1 = (Func<object>)(() => new Usecase1((Service1)factoryService1()));
            var factoryUsecase2 = (Func<object>)(() => new Usecase2((Service2)factoryService2()));

            var factoryTransient1 = (Func<object>)(() => new Transient1());
            var factoryTransient2 = (Func<object>)(() => new Transient2());

            var complexFactory = (Func<object>)(() => new Complex(
                (Service1)factoryService1(),
                (Service2)factoryService2(),
                (Usecase1)factoryUsecase1(),
                (Usecase2)factoryUsecase2(),
                (Transient1)factoryTransient1(),
                (Transient2)factoryTransient2()));

            return complexFactory;
        }
    }

    //------------------------------------------------------------

    public sealed class Usecase1Activator1 : IActivator1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(object argument1)
        {
            return new Usecase1((Service1)argument1);
        }

        public object Create(params object[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class Usecase2Activator1 : IActivator1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(object argument1)
        {
            return new Usecase2((Service2)argument1);
        }

        public object Create(params object[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class Transient1Activator0 : IActivator0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create()
        {
            return new Transient1();
        }

        public object Create(params object[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class Transient2Activator0 : IActivator0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create()
        {
            return new Transient2();
        }

        public object Create(params object[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class ComplexActivator6 : IActivator6
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Complex(
                (Service1)arguments[0],
                (Service2)arguments[1],
                (Usecase1)arguments[2],
                (Usecase2)arguments[3],
                (Transient1)arguments[4],
                (Transient2)arguments[5]);
        }

        public object Create(
            object argument1,
            object argument2,
            object argument3,
            object argument4,
            object argument5,
            object argument6)
        {
            return new Complex(
                (Service1)argument1,
                (Service2)argument2,
                (Usecase1)argument3,
                (Usecase2)argument4,
                (Transient1)argument5,
                (Transient2)argument6);
        }
    }

    //------------------------------------------------------------

    public sealed class Usecase1Activator : IActivator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Usecase1((Service1)arguments[0]);
        }
    }

    public sealed class Usecase2Activator : IActivator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Usecase2((Service2)arguments[0]);
        }
    }

    public sealed class Transient1Activator : IActivator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Transient1();
        }
    }

    public sealed class Transient2Activator : IActivator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Transient2();
        }
    }

    public sealed class ComplexActivator : IActivator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(params object[] arguments)
        {
            return new Complex(
                (Service1)arguments[0],
                (Service2)arguments[1],
                (Usecase1)arguments[2],
                (Usecase2)arguments[3],
                (Transient1)arguments[4],
                (Transient2)arguments[5]);
        }
    }
}
