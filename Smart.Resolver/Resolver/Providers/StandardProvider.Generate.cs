﻿// <auto-generated />
namespace Smart.Resolver.Providers
{
    using System;

    public sealed partial class StandardProvider
    {
        private static Func<object> CreateActivator1(
            Func<object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            return () =>
            {
                var instance = activator(f1());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator1(
            Func<object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            return () => activator(f1());
        }

        private static Func<object> CreateActivator2(
            Func<object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            return () =>
            {
                var instance = activator(f1(), f2());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator2(
            Func<object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            return () => activator(f1(), f2());
        }

        private static Func<object> CreateActivator3(
            Func<object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            return () =>
            {
                var instance = activator(f1(), f2(), f3());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator3(
            Func<object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            return () => activator(f1(), f2(), f3());
        }

        private static Func<object> CreateActivator4(
            Func<object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator4(
            Func<object, object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            return () => activator(f1(), f2(), f3(), f4());
        }

        private static Func<object> CreateActivator5(
            Func<object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator5(
            Func<object, object, object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            return () => activator(f1(), f2(), f3(), f4(), f5());
        }

        private static Func<object> CreateActivator6(
            Func<object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator6(
            Func<object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6());
        }

        private static Func<object> CreateActivator7(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator7(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());
        }

        private static Func<object> CreateActivator8(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator8(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
        {
            var f1 = factories[0];
            var f2 = factories[1];
            var f3 = factories[2];
            var f4 = factories[3];
            var f5 = factories[4];
            var f6 = factories[5];
            var f7 = factories[6];
            var f8 = factories[7];
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());
        }

        private static Func<object> CreateActivator9(
            Func<object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator9(
            Func<object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9());
        }

        private static Func<object> CreateActivator10(
            Func<object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator10(
            Func<object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10());
        }

        private static Func<object> CreateActivator11(
            Func<object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator11(
            Func<object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11());
        }

        private static Func<object> CreateActivator12(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator12(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12());
        }

        private static Func<object> CreateActivator13(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator13(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13());
        }

        private static Func<object> CreateActivator14(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator14(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14());
        }

        private static Func<object> CreateActivator15(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14(), f15());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator15(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14(), f15());
        }

        private static Func<object> CreateActivator16(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories,
			Action<object>[] actions)
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
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14(), f15(), f16());

				for(var i = 0; i < actions.Length; i++)
				{
				    actions[i](instance);
				}

				return instance;
            };
        }

        private static Func<object> CreateActivator16(
            Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] factories)
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
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8(), f9(), f10(), f11(), f12(), f13(), f14(), f15(), f16());
        }

    }
}
