namespace NewFactoryLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Smart.Reflection.Emit;

    public class Builder
    {
        private const string AssemblyName = "ResolverAssembly";

        private const string ModuleName = "ResolverModule";

        private readonly Dictionary<Type, int> arrayFactoryIds = new Dictionary<Type, int>();

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


        public object ToConstant<T>(T value)
        {
            return (Func<IContainer, T>)(c => value);
        }

        public object ToMethod<T>(Func<IContainer, T> func)
        {
            return func;
        }

        public object To(ConstructorInfo ci, params object[] factories)
        {
            if (ci.GetParameters().Length != factories.Length)
            {
                throw new ArgumentException("Invalid factories length.");
            }

            // Define type
            var typeBuilder = ModuleBuilder.DefineType(
                $"{ci.DeclaringType.FullName}_Resolver{Array.IndexOf(ci.DeclaringType.GetConstructors(), ci)}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            // Define field
            var fields = factories
                .Select((t, i) => typeBuilder.DefineField(
                    $"factory{i}",
                    t.GetType(),
                    FieldAttributes.Public | FieldAttributes.InitOnly))
                .ToList();

            // Define method
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
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            ilGenerator.Emit(OpCodes.Ret);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var type = typeInfo.AsType();

            // Prepare instance
            var instance = Activator.CreateInstance(type);

            for (var i = 0; i < factories.Length; i++)
            {
                var fi = type.GetField($"factory{i}");
                fi.SetValue(instance, factories[i]);
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), ci.DeclaringType);
            return Delegate.CreateDelegate(funcType, instance, type.GetMethod("Create"));
        }

        private int GenerateArrayFactoryId(Type type)
        {
            lock (arrayFactoryIds)
            {
                if (!arrayFactoryIds.TryGetValue(type, out var id))
                {
                    arrayFactoryIds[type] = 2;
                    return 1;
                }

                arrayFactoryIds[type] = id + 1;
                return id;
            }
        }

        public object ToArray(Type baseType, params object[] factories)
        {
            var arrayType = baseType.MakeArrayType();

            // Define type
            var typeBuilder = ModuleBuilder.DefineType(
                $"{arrayType.FullName}_Resolver{GenerateArrayFactoryId(baseType)}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            // Define field
            var fields = factories
                .Select((t, i) => typeBuilder.DefineField(
                    $"factory{i}",
                    t.GetType(),
                    FieldAttributes.Public | FieldAttributes.InitOnly))
                .ToList();

            // Define method
            var method = typeBuilder.DefineMethod(
                "Create",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                arrayType,
                new[] { typeof(IContainer) });

            var ilGenerator = method.GetILGenerator();

            ilGenerator.EmitLdcI4(factories.Length);
            ilGenerator.Emit(OpCodes.Newarr, baseType);

            for (var i = 0; i < factories.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Dup);
                ilGenerator.EmitLdcI4(i);

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fields[i]);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                var invokeMethod = factories[i].GetType().GetMethod("Invoke", new[] { typeof(IContainer) });
                ilGenerator.Emit(OpCodes.Call, invokeMethod);

                ilGenerator.Emit(OpCodes.Stelem_Ref);
            }

            ilGenerator.Emit(OpCodes.Ret);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var type = typeInfo.AsType();

            // Prepare instance
            var instance = Activator.CreateInstance(type);

            for (var i = 0; i < factories.Length; i++)
            {
                var fi = type.GetField($"factory{i}");
                fi.SetValue(instance, factories[i]);
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), arrayType);
            return Delegate.CreateDelegate(funcType, instance, type.GetMethod("Create"));
        }

        // TODO Reflection version * 2
    }
}
