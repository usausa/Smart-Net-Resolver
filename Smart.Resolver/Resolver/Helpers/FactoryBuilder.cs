namespace Smart.Resolver.Helpers
{
    using System;

    using Smart.Resolver.Parameters;

    public static class FactoryBuilder
    {
        public static Func<object> Constant(object value)
        {
            return () => value;
        }

        public static Func<object> Callback(IKernel kernel, Func<IKernel, object> factory)
        {
            return () => factory(kernel);
        }

        public static Func<object> Parameter(IKernel kernel, IParameter parameter)
        {
            return () => parameter.Resolve(kernel);
        }

        public static Func<object> Array(Func<int, Array> arrayAllocator, Func<object>[] objectFactories)
        {
            return () =>
            {
                var array = arrayAllocator(objectFactories.Length);
                var objs = (object[])array;
                for (var i = 0; i < objectFactories.Length; i++)
                {
                    objs[i] = objectFactories[i]();
                }

                return array;
            };
        }

        public static Func<object> Activator0(
            Action<object> processor,
            Func<object> activator)
        {
            if (processor != null)
            {
                return () =>
                {
                    var instance = activator();
                    processor(instance);
                    return instance;
                };
            }

            return activator;
        }

        public static Func<object> Activator1(
            Action<object> processor,
            Func<object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator1WithProcess(
                    processor,
                    activator,
                    argumentFactories[0]);
            }

            return Activator1WithoutProcess(
                activator,
                argumentFactories[0]);
        }

        private static Func<object> Activator1WithProcess(
            Action<object> processor,
            Func<object, object> activator,
            Func<object> f1)
        {
            return () =>
            {
                var instance = activator(f1());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator1WithoutProcess(
            Func<object, object> activator,
            Func<object> f1)
        {
            return () => activator(f1());
        }

        public static Func<object> Activator2(
            Action<object> processor,
            Func<object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator2WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1]);
            }

            return Activator2WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1]);
        }

        private static Func<object> Activator2WithProcess(
            Action<object> processor,
            Func<object, object, object> activator,
            Func<object> f1,
            Func<object> f2)
        {
            return () =>
            {
                var instance = activator(f1(), f2());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator2WithoutProcess(
            Func<object, object, object> activator,
            Func<object> f1,
            Func<object> f2)
        {
            return () => activator(f1(), f2());
        }

        public static Func<object> Activator3(
            Action<object> processor,
            Func<object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator3WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2]);
            }

            return Activator3WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2]);
        }

        private static Func<object> Activator3WithProcess(
            Action<object> processor,
            Func<object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator3WithoutProcess(
            Func<object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3)
        {
            return () => activator(f1(), f2(), f3());
        }

        public static Func<object> Activator4(
            Action<object> processor,
            Func<object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator4WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3]);
            }

            return Activator4WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3]);
        }

        private static Func<object> Activator4WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator4WithoutProcess(
            Func<object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4)
        {
            return () => activator(f1(), f2(), f3(), f4());
        }

        public static Func<object> Activator5(
            Action<object> processor,
            Func<object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator5WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4]);
            }

            return Activator5WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4]);
        }

        private static Func<object> Activator5WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator5WithoutProcess(
            Func<object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5());
        }

        public static Func<object> Activator6(
            Action<object> processor,
            Func<object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator6WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5]);
            }

            return Activator6WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5]);
        }

        private static Func<object> Activator6WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator6WithoutProcess(
            Func<object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6());
        }

        public static Func<object> Activator7(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator7WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5],
                    argumentFactories[6]);
            }

            return Activator7WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5],
                argumentFactories[6]);
        }

        private static Func<object> Activator7WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator7WithoutProcess(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());
        }

        public static Func<object> Activator8(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator8WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5],
                    argumentFactories[6],
                    argumentFactories[7]);
            }

            return Activator8WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5],
                argumentFactories[6],
                argumentFactories[7]);
        }

        private static Func<object> Activator8WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7,
            Func<object> f8)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator8WithoutProcess(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7,
            Func<object> f8)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());
        }

        public static Func<object> Activator(
            Action<object> processor,
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return processor != null
                ? ActivatorWithProcess(processor, activator, argumentFactories)
                : ActivatorWithoutProcess(activator, argumentFactories);
        }

        private static Func<object> ActivatorWithProcess(
            Action<object> processor,
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return () =>
            {
                var arguments = new object[argumentFactories.Length];
                for (var i = 0; i < argumentFactories.Length; i++)
                {
                    arguments[i] = argumentFactories[i]();
                }

                var instance = activator(arguments);
                processor(instance);
                return instance;
            };
        }

        private static Func<object> ActivatorWithoutProcess(
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return () =>
            {
                var arguments = new object[argumentFactories.Length];
                for (var i = 0; i < argumentFactories.Length; i++)
                {
                    arguments[i] = argumentFactories[i]();
                }

                return activator(arguments);
            };
        }
    }
}
