namespace NewDynamicFactoryBenchmark
{
    using System;
    using System.Linq;
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
            Add(Job.MediumRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private const int N = 1_000;

        private static readonly ConstructorInfo ci0 = typeof(Data0).GetConstructors()[0];
        private static readonly ConstructorInfo ci1 = typeof(Data1).GetConstructors()[0];
        private static readonly ConstructorInfo ci2 = typeof(Data2).GetConstructors()[0];
        private static readonly ConstructorInfo ci4 = typeof(Data4).GetConstructors()[0];

        private readonly Func<IResolver, object> stringFactory = r => string.Empty;
        private readonly Func<IResolver, object> intFactory = r => 0;

        private Func<IResolver, object> dynamicFactory0;
        private Func<IResolver, object> dynamicFactory1;
        private Func<IResolver, object> dynamicFactory2;
        private Func<IResolver, object> dynamicFactory4;

        private Func<IResolver, object> classFactory0;
        private Func<IResolver, object> classFactory1;
        private Func<IResolver, object> classFactory2;
        private Func<IResolver, object> classFactory4;

        [GlobalSetup]
        public void Setup()
        {
            var builder = new Builder();

            dynamicFactory0 = builder.CreateDynamicFactory(ci0);
            dynamicFactory1 = builder.CreateDynamicFactory(ci1, typeof(ParameterContainer1), new[] { stringFactory });
            dynamicFactory2 = builder.CreateDynamicFactory(ci2, typeof(ParameterContainer2), new[] { stringFactory, intFactory });
            dynamicFactory4 = builder.CreateDynamicFactory(ci4, typeof(ParameterContainer4), new[] { stringFactory, intFactory, stringFactory, intFactory });

            classFactory0 = builder.CreateFactory(ci0, new Func<IResolver, object>[0]);
            classFactory1 = builder.CreateFactory(ci1, new[] { stringFactory });
            classFactory2 = builder.CreateFactory(ci2, new[] { stringFactory, intFactory });
            classFactory4 = builder.CreateFactory(ci4, new[] { stringFactory, intFactory, stringFactory, intFactory });
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void DynamicFactory0()
        {
            for (var i = 0; i < N; i++)
            {
                dynamicFactory0(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFactory0()
        {
            for (var i = 0; i < N; i++)
            {
                classFactory0(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void DynamicFactory1()
        {
            for (var i = 0; i < N; i++)
            {
                dynamicFactory1(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFactory1()
        {
            for (var i = 0; i < N; i++)
            {
                classFactory1(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void DynamicFactory2()
        {
            for (var i = 0; i < N; i++)
            {
                dynamicFactory2(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFactory2()
        {
            for (var i = 0; i < N; i++)
            {
                classFactory2(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void DynamicFactory4()
        {
            for (var i = 0; i < N; i++)
            {
                dynamicFactory4(null);
            }
        }

        [Benchmark(OperationsPerInvoke = N)]
        public void ClassFactory4()
        {
            for (var i = 0; i < N; i++)
            {
                classFactory4(null);
            }
        }
    }

    // Context

    public interface IResolver
    {
    }

    // Data

    public sealed class Data0
    {
    }

    public sealed class Data1
    {
        public string Parameter1 { get; }

        public Data1(string parameter1)
        {
            Parameter1 = parameter1;
        }
    }

    public sealed class Data2
    {
        public string Parameter1 { get; }

        public int Parameter2 { get; }

        public Data2(string parameter1, int parameter2)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }
    }

    public sealed class Data4
    {
        public string Parameter1 { get; }

        public int Parameter2 { get; }

        public string Parameter3 { get; }

        public int Parameter4 { get; }

        public Data4(string parameter1, int parameter2, string parameter3, int parameter4)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
            Parameter4 = parameter4;
        }
    }

    // Container

    public sealed class ParameterContainer1
    {
        public Func<IResolver, object> ParameterFactory0;
    }

    public sealed class ParameterContainer2
    {
        public Func<IResolver, object> ParameterFactory0;
        public Func<IResolver, object> ParameterFactory1;
    }

    public sealed class ParameterContainer4
    {
        public Func<IResolver, object> ParameterFactory0;
        public Func<IResolver, object> ParameterFactory1;
        public Func<IResolver, object> ParameterFactory2;
        public Func<IResolver, object> ParameterFactory3;
    }

    // Builder

    public class Builder
    {
        private AssemblyBuilder assemblyBuilder;

        private ModuleBuilder moduleBuilder;

        private int nextId = 1;

        private ModuleBuilder ModuleBuilder
        {
            get
            {
                if (moduleBuilder == null)
                {
                    assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName("EmitBuilderAssembly"),
                        AssemblyBuilderAccess.Run);
                    moduleBuilder = assemblyBuilder.DefineDynamicModule(
                        "EmitBuilderModule");
                }

                return moduleBuilder;
            }
        }

        private int GenerateId()
        {
            lock (assemblyBuilder)
            {
                return nextId++;
            }
        }

        // TODO dynamic create container type
        public Func<IResolver, object> CreateDynamicFactory(ConstructorInfo ci, Type containerType, Func<IResolver, object>[] factories)
        {
            var returnType = ci.DeclaringType.IsValueType ? typeof(object) : ci.DeclaringType;

            var dynamicMethod = new DynamicMethod(string.Empty, returnType, new[] { containerType, typeof(IResolver) }, true);
            var ilGenerator = dynamicMethod.GetILGenerator();

            var container = Activator.CreateInstance(containerType);

            for (var i = 0; i < factories.Length; i++)
            {
                var invokeMethod = factories[i].GetType().GetMethod("Invoke");

                var field = containerType.GetField($"ParameterFactory{i}");
                field.SetValue(container, factories[i]);

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
                if (ci.GetParameters()[i].ParameterType.IsValueType)
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, ci.GetParameters()[i].ParameterType);
                }
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            if (ci.DeclaringType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, ci.DeclaringType);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), returnType);
            return (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, container);
        }

        // TODO for combine
        public Func<IResolver, object> CreateDynamicFactory(ConstructorInfo ci)
        {
            var returnType = ci.DeclaringType.IsValueType ? typeof(object) : ci.DeclaringType;

            var dynamicMethod = new DynamicMethod(string.Empty, returnType, new[] { typeof(object), typeof(IResolver) }, true);
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.Emit(OpCodes.Newobj, ci);

            if (ci.DeclaringType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, ci.DeclaringType);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), returnType);
            return (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, null);
        }

        public Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories)
        {
            var returnType = ci.DeclaringType.IsValueType ? typeof(object) : ci.DeclaringType;

            // Define type
            var typeBuilder = ModuleBuilder.DefineType(
                $"{ci.DeclaringType.FullName}_Factory_{GenerateId()}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            // Define field
            var factoryFields = factories
                .Select((t, i) => typeBuilder.DefineField(
                    $"factory{i}",
                    t.GetType(),
                    FieldAttributes.Public | FieldAttributes.InitOnly))
                .ToList();

            // Define method
            var method = typeBuilder.DefineMethod(
                "Create",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                returnType,
                new[] { typeof(IResolver) });

            var ilGenerator = method.GetILGenerator();

            for (var i = 0; i < factories.Length; i++)
            {
                var invokeMethod = factories[i].GetType().GetMethod("Invoke");
                if ((invokeMethod == null) ||
                    (invokeMethod.GetParameters().Length != 1) ||
                    (invokeMethod.GetParameters()[0].ParameterType != typeof(IResolver)))
                {
                    throw new ArgumentException($"Invalid factory[{i}]");
                }

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, factoryFields[i]);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
                if (ci.GetParameters()[i].ParameterType.IsValueType)
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, ci.GetParameters()[i].ParameterType);
                }
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            if (ci.DeclaringType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, ci.DeclaringType);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var factoryType = typeInfo.AsType();

            // Prepare instance
            var instance = Activator.CreateInstance(factoryType);

            for (var i = 0; i < factories.Length; i++)
            {
                var fi = factoryType.GetField($"factory{i}");
                fi.SetValue(instance, factories[i]);
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), returnType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return (Func<IResolver, object>)Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }
    }
}
