namespace NewFactoryBenchmark
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;

    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }

    public sealed class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.Default, MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(Job.LongRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private const int N = 1_000;

        private Func<Data0> funcFunc0;
        private Func<Data1> funcFunc1;
        private Func<Data2> funcFunc2;

        private Func<Data0> classFunc0;
        private Func<Data1> classFunc1;
        private Func<Data2> classFunc2;

        private Func<Data0> staticFunc0;
        private Func<Data1> staticFunc1;
        private Func<Data2> staticFunc2;

        // Dynamic with context
        private Func<IContainer, Data0> dynamicFunc0;
        private Func<IContainer, Data1> dynamicFunc1;
        private Func<IContainer, Data2> dynamicFunc2;

        [GlobalSetup]
        public void Setup()
        {
            funcFunc0 = CreateFunc0();
            funcFunc1 = CreateFunc1(funcFunc0);
            funcFunc2 = CreateFunc2(funcFunc0, funcFunc1);

            var classResolver0 = new ClassResolver0();
            classFunc0 = classResolver0.Resolve;
            var classResolver1 = new ClassResolver1 { arg1Resolver = classFunc0 };
            classFunc1 = classResolver1.Resolve;
            var classResolver2 = new ClassResolver2 { arg1Resolver = classFunc0, arg2Resolver = classFunc1 };
            classFunc2 = classResolver2.Resolve;

            staticFunc0 = StaticResolver0.Resolve;
            StaticResolver1.arg1Resolver = staticFunc0;
            staticFunc1 = StaticResolver1.Resolve;
            StaticResolver2.arg1Resolver = staticFunc0;
            StaticResolver2.arg2Resolver = staticFunc1;
            staticFunc2 = StaticResolver2.Resolve;

            var builder = new Builder();
            dynamicFunc0 = (Func<IContainer, Data0>)builder.Build(typeof(Data0).GetConstructors()[0]);
            dynamicFunc1 = (Func<IContainer, Data1>)builder.Build(typeof(Data1).GetConstructors()[0], dynamicFunc0);
            dynamicFunc2 = (Func<IContainer, Data2>)builder.Build(typeof(Data2).GetConstructors()[0], dynamicFunc0, dynamicFunc1);
        }

        private static Func<Data0> CreateFunc0() => () => new Data0();

        private static Func<Data1> CreateFunc1(Func<Data0> f0) => () => new Data1(f0());

        private static Func<Data2> CreateFunc2(Func<Data0> f0, Func<Data1> f1) => () => new Data2(f0(), f1());

        [Benchmark(OperationsPerInvoke = N)]
        public void FuncFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                funcFunc2();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                classFunc2();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void StaticFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                staticFunc2();
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void DynamicFunc2()
        {
            for (var i = 0; i < N; i++)
            {
                dynamicFunc2(null);
            }
        }
    }

    // Context

    public interface IContainer
    {
    }

    // Resolver

    public sealed class ClassResolver0
    {
        public Data0 Resolve() => new Data0();
    }

    public sealed class ClassResolver1
    {
        public Func<Data0> arg1Resolver;

        public Data1 Resolve() => new Data1(arg1Resolver());
    }

    public sealed class ClassResolver2
    {
        public Func<Data0> arg1Resolver;

        public Func<Data1> arg2Resolver;

        public Data2 Resolve() => new Data2(arg1Resolver(), arg2Resolver());
    }

    // Resolver(static)

    public static class StaticResolver0
    {
        public static Data0 Resolve() => new Data0();
    }

    public static class StaticResolver1
    {
        public static Func<Data0> arg1Resolver;

        public static Data1 Resolve() => new Data1(arg1Resolver());
    }

    public static class StaticResolver2
    {
        public static Func<Data0> arg1Resolver;

        public static Func<Data1> arg2Resolver;

        public static Data2 Resolve() => new Data2(arg1Resolver(), arg2Resolver());
    }

    // Builder

    public class Builder
    {
        private const string AssemblyName = "ResolverAssembly";

        private const string ModuleName = "ResolverModule";

        private AssemblyBuilder assemblyBuilder;

        private ModuleBuilder moduleBuilder;

        private ModuleBuilder ModuleBuilder
        {
            get
            {
                if (moduleBuilder == null)
                {
                    assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName(AssemblyName),
                        AssemblyBuilderAccess.Run);
                    moduleBuilder = assemblyBuilder.DefineDynamicModule(
                        ModuleName);
                }
                return moduleBuilder;
            }
        }

        public object Build(ConstructorInfo ci, params object[] factories)
        {
            if (ci.GetParameters().Length != factories.Length)
            {
                throw new ArgumentException("Invalid factories length.");
            }

            // Type
            var typeBuilder = ModuleBuilder.DefineType(
                $"{ci.DeclaringType.FullName}_DynamicActivator{Array.IndexOf(ci.DeclaringType.GetConstructors(), ci)}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            // Field
            var fields = new List<FieldBuilder>();
            for (var i = 0; i < factories.Length; i++)
            {
                fields.Add(typeBuilder.DefineField(
                    $"factory{i}",
                    factories[i].GetType(),
                    FieldAttributes.Public | FieldAttributes.InitOnly));
            }

            // Method
            var method = typeBuilder.DefineMethod(
                "Create",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                ci.DeclaringType,
                new[] { typeof(IContainer) });

            var ilGenerator = method.GetILGenerator();

            for (var i = 0; i < factories.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fields[i]);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                var invokeMethod = factories[i].GetType().GetMethod("Invoke", new[] { typeof(IContainer) });
                // [MEMO] hack
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            ilGenerator.Emit(OpCodes.Ret);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var type = typeInfo.AsType();

            var instance = Activator.CreateInstance(type);

            for (var i = 0; i < factories.Length; i++)
            {
                var fi = type.GetField($"factory{i}");
                fi.SetValue(instance, factories[i]);
            }

            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), ci.DeclaringType);

            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, type.GetMethod("Create"));
        }
    }

    // Data

    public sealed class Data0
    {
    }

    public sealed class Data1
    {
        public Data0 Arg1 { get; }

        public Data1(Data0 arg1)
        {
            Arg1 = arg1;
        }
    }

    public sealed class Data2
    {
        public Data0 Arg1 { get; }

        public Data1 Arg2 { get; }

        public Data2(Data0 arg1, Data1 arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
    }
}
