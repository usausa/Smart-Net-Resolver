// <auto-generated />
namespace Smart.Resolver.Providers
{
    using System;

    public sealed partial class StandardProvider
    {
        private static Func<IResolver, object> CreateActivator1(
            Func<object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            return r =>
            {
                var instance = activator(f1(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator1(
            Func<object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            return r => activator(f1(r));
        }

        private static Func<IResolver, object> CreateActivator2(
            Func<object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            return r =>
            {
                var instance = activator(f1(r), f2(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator2(
            Func<object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            return r => activator(f1(r), f2(r));
        }

        private static Func<IResolver, object> CreateActivator3(
            Func<object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator3(
            Func<object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            return r => activator(f1(r), f2(r), f3(r));
        }

        private static Func<IResolver, object> CreateActivator4(
            Func<object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator4(
            Func<object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            return r => activator(f1(r), f2(r), f3(r), f4(r));
        }

        private static Func<IResolver, object> CreateActivator5(
            Func<object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator5(
            Func<object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r));
        }

        private static Func<IResolver, object> CreateActivator6(
            Func<object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator6(
            Func<object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r));
        }

        private static Func<IResolver, object> CreateActivator7(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator7(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r));
        }

        private static Func<IResolver, object> CreateActivator8(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator8(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r));
        }

        private static Func<IResolver, object> CreateActivator9(
            Func<object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator9(
            Func<object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r));
        }

        private static Func<IResolver, object> CreateActivator10(
            Func<object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator10(
            Func<object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r));
        }

        private static Func<IResolver, object> CreateActivator11(
            Func<object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator11(
            Func<object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r));
        }

        private static Func<IResolver, object> CreateActivator12(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator12(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r));
        }

        private static Func<IResolver, object> CreateActivator13(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator13(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r));
        }

        private static Func<IResolver, object> CreateActivator14(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator14(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r));
        }

        private static Func<IResolver, object> CreateActivator15(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            var f15 = factories[14];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r), f15(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator15(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            var f15 = factories[14];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r), f15(r));
        }

        private static Func<IResolver, object> CreateActivator16(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            var f15 = factories[14];
            var f16 = factories[15];
            return r =>
            {
                var instance = activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r), f15(r), f16(r));

                for(var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator16(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<IResolver, object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            var f9 = factories[8];
            var f10 = factories[9];
            var f11 = factories[10];
            var f12 = factories[11];
            var f13 = factories[12];
            var f14 = factories[13];
            var f15 = factories[14];
            var f16 = factories[15];
            return r => activator(f1(r), f2(r), f3(r), f4(r), f5(r), f6(r), f7(r), f8(r), f9(r), f10(r), f11(r), f12(r), f13(r), f14(r), f15(r), f16(r));
        }

    }
}
