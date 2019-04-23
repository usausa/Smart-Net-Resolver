namespace Smart.Resolver.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public sealed class EmitBuilder
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

        public object CreateFactory(ConstructorInfo ci, object[] factories, object[] actions)
        {
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

            var actionFields = actions
                .Select((t, i) => typeBuilder.DefineField(
                    $"actions{i}",
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
                var invokeMethod = factories[i].GetType().GetMethod("Invoke");
                if ((invokeMethod == null) ||
                    (invokeMethod.GetParameters().Length != 1) ||
                    (invokeMethod.GetParameters()[0].ParameterType != typeof(IContainer)))
                {
                    throw new ArgumentException($"Invalid factory[{i}]");
                }

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, factoryFields[i]);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            if (actions.Length > 0)
            {
                ilGenerator.DeclareLocal(ci.DeclaringType);

                ilGenerator.Emit(OpCodes.Stloc_0);

                for (var i = 0; i < actions.Length; i++)
                {
                    var invokeMethod = actions[i].GetType().GetMethod("Invoke");
                    if ((invokeMethod == null) ||
                        (invokeMethod.GetParameters().Length != 2) ||
                        (invokeMethod.GetParameters()[0].ParameterType != typeof(IContainer)))
                    {
                        throw new ArgumentException($"Invalid actions[{i}]");
                    }

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, actionFields[i]);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Ldloc_0);
                    ilGenerator.Emit(OpCodes.Callvirt, invokeMethod);
                }

                ilGenerator.Emit(OpCodes.Ldloc_0);
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

            for (var i = 0; i < actions.Length; i++)
            {
                var fi = type.GetField($"actions{i}");
                fi.SetValue(instance, actions[i]);
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), ci.DeclaringType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, type.GetMethod("Create"));
        }
    }
}
