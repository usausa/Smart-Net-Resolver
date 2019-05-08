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

        private Func<IResolver, object> dynamicFactory0;

        private Func<IResolver, object> classFactory0;

        // TODO combined, 1, 2, 8 ?

        [GlobalSetup]
        public void Setup()
        {
            var builder = new Builder();
            dynamicFactory0 = builder.CreateDynamicFactory(ci0);
            classFactory0 = builder.CreateFactory(ci0, new Func<IResolver, object>[0]);
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

        // TODO Other
    }

    // Context

    public interface IResolver
    {
    }

    // Data

    public sealed class Data0
    {
    }

    //

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
