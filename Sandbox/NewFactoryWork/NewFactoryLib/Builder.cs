namespace NewFactoryLib
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

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


        public Func<IContainer, T> ToConstant<T>(T value)
        {
            return c => value;
        }

        public static Func<IContainer, T> ToMethod<T>(Func<IContainer, T> func)
        {
            return func;
        }

        // TODO return to object ?
        public Func<IContainer, T> To<T>(ConstructorInfo ci, params object[] factories)
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

            // TODO Final?
            // Method
            var method = typeBuilder.DefineMethod(
                "Create",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                typeof(T),
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

            // ReSharper disable once AssignNullToNotNullAttribute
            return (Func<IContainer, T>)Delegate.CreateDelegate(typeof(Func<IContainer, T>), instance, type.GetMethod("Create"));
        }

        // TODO Reflection version

        // TODO Array factory
    }
}
