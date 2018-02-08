namespace PerformanceBenchmark
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    using Smart.Reflection;
    using Smart.Reflection.Emit;

    public sealed class EmitDelegateFactory : IDelegateFactory
    {
        public Func<int, Array> CreateArrayAllocator(Type type)
        {
            var arrayOperator = CreateArrayOperator(type);
            return arrayOperator.Create;
        }

        public Func<object[], object> CreateFactory(ConstructorInfo ci)
        {
            var activator = CreateActivator(ci);
            return activator.Create;
        }

        public Func<object> CreateFactory0(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator0>(ci);
            return activator.Create;
        }

        public Func<object, object> CreateFactory1(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator1>(ci);
            return activator.Create;
        }

        public Func<object, object, object> CreateFactory2(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator2>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object> CreateFactory3(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator3>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object> CreateFactory4(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator4>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object, object> CreateFactory5(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator5>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object, object, object> CreateFactory6(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator6>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object, object, object, object> CreateFactory7(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator7>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object, object, object, object, object> CreateFactory8(ConstructorInfo ci)
        {
            var activator = CreateActivator<IActivator8>(ci);
            return activator.Create;
        }

        public Func<object, object, object, object, object, object, object, object, object, object> CreateFactory9(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object> CreateFactory10(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory11(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory12(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory13(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory14(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory15(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> CreateFactory16(ConstructorInfo ci)
        {
            throw new NotImplementedException();
        }

        public Func<object, object> CreateGetter(PropertyInfo pi)
        {
            throw new NotImplementedException();
        }

        public Func<object, object> CreateGetter(PropertyInfo pi, bool extension)
        {
            throw new NotImplementedException();
        }

        public Action<object, object> CreateSetter(PropertyInfo pi)
        {
            throw new NotImplementedException();
        }

        public Action<object, object> CreateSetter(PropertyInfo pi, bool extension)
        {
            throw new NotImplementedException();
        }

        public Type GetExtendedPropertyType(PropertyInfo pi)
        {
            throw new NotImplementedException();
        }

        private const string AssemblyName = "SmartDynamicActivatorAssembly";

        private const string ModuleName = "SmartDynamicActivatorModule";

        private sealed class ActivatorInfo
        {
            public Type Type { get; }

            public MethodInfo CreateMethodInfo { get; }

            public Type[] CreateArgumentTypes { get; }

            public ActivatorInfo(Type type, int length)
            {
                Type = type;
                CreateMethodInfo = type.GetMethod("Create");
                CreateArgumentTypes = new Type[length];
                for (var i = 0; i < length; i++)
                {
                    CreateArgumentTypes[i] = typeof(object);
                }
            }
        }

        private static readonly Dictionary<int, ActivatorInfo> SupportedActiavators = new Dictionary<int, ActivatorInfo>
        {
            { 0, new ActivatorInfo(typeof(IActivator0), 0) },
            { 1, new ActivatorInfo(typeof(IActivator1), 1) },
            { 2, new ActivatorInfo(typeof(IActivator2), 2) },
            { 3, new ActivatorInfo(typeof(IActivator3), 3) },
            { 4, new ActivatorInfo(typeof(IActivator4), 4) },
            { 5, new ActivatorInfo(typeof(IActivator5), 5) },
            { 6, new ActivatorInfo(typeof(IActivator6), 6) },
            { 7, new ActivatorInfo(typeof(IActivator7), 7) },
            { 8, new ActivatorInfo(typeof(IActivator8), 8) }
        };

        private static readonly Type ObjectType = typeof(object);

        private static readonly Type VoidType = typeof(void);

        private static readonly Type BoolType = typeof(bool);

        private static readonly Type StringType = typeof(string);

        private static readonly Type ArrayType = typeof(Array);

        private static readonly Type CtorType = typeof(ConstructorInfo);

        private static readonly Type TypeType = typeof(Type);

        private static readonly Type PropertyInfoType = typeof(PropertyInfo);

        private static readonly ConstructorInfo ObjectCotor = typeof(object).GetConstructor(Type.EmptyTypes);

        private static readonly ConstructorInfo NotSupportedExceptionCtor = typeof(NotSupportedException).GetConstructor(Type.EmptyTypes);

        private static readonly MethodInfo PropertyInfoNameGetMethod =
            typeof(PropertyInfo).GetProperty(nameof(PropertyInfo.Name)).GetGetMethod();

        private static readonly MethodInfo PropertyInfoPropertyTypeGetMethod =
            typeof(PropertyInfo).GetProperty(nameof(PropertyInfo.PropertyType)).GetGetMethod();

        // Activator

        private static readonly Type ActivatorType = typeof(IActivator);

        private static readonly MethodInfo ActivatorCreateMethodInfo = typeof(IActivator).GetMethod(nameof(IActivator.Create));

        private static readonly Type[] ActivatorConstructorArgumentTypes = { typeof(ConstructorInfo) };

        private static readonly Type[] ActivatorCreateArgumentTypes = { typeof(object[]) };

        // ArrayOperator

        private static readonly Type ArrayOperatorType = typeof(IArrayOperator);

        private static readonly MethodInfo ArrayOperatorCreateMethodInfo = typeof(IArrayOperator).GetMethod(nameof(IArrayOperator.Create));

        private static readonly MethodInfo ArrayOperatorGetValueMethodInfo = typeof(IArrayOperator).GetMethod(nameof(IArrayOperator.GetValue));

        private static readonly MethodInfo ArrayOperatorSetValueMethodInfo = typeof(IArrayOperator).GetMethod(nameof(IArrayOperator.SetValue));

        private static readonly Type[] ArrayOperatorConstructorArgumentTypes = { typeof(Type) };

        private static readonly Type[] ArrayOperatorCreateArgumentTypes = { typeof(int) };

        private static readonly Type[] ArrayOperatorGetValueArgumentTypes = { typeof(Array), typeof(int) };

        private static readonly Type[] ArrayOperatorSetValueArgumentTypes = { typeof(Array), typeof(int), typeof(object) };

        // Member

        private readonly object sync = new object();

        private readonly Dictionary<ConstructorInfo, IActivator> activatorCache = new Dictionary<ConstructorInfo, IActivator>();

        private readonly Dictionary<Type, IArrayOperator> arrayOperatorCache = new Dictionary<Type, IArrayOperator>();

        private AssemblyBuilder assemblyBuilder;

        private ModuleBuilder moduleBuilder;

        /// <summary>
        ///
        /// </summary>
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

        public IActivator CreateActivator(ConstructorInfo ci)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            lock (sync)
            {
                if (!activatorCache.TryGetValue(ci, out var activator))
                {
                    activator = CreateActivatorInternal(ci, null);
                    activatorCache[ci] = activator;
                }

                return activator;
            }
        }

        public TActivator CreateActivator<TActivator>(ConstructorInfo ci)
            where TActivator : IActivator
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            if (!SupportedActiavators.TryGetValue(ci.GetParameters().Length, out var activatorInfo) ||
                activatorInfo.Type != typeof(TActivator))
            {
                throw new ArgumentException(
                    $"Constructor is unmatched for activator. length = [{ci.GetParameters().Length}], type = {typeof(TActivator)}");
            }

            lock (sync)
            {
                if (!activatorCache.TryGetValue(ci, out var activator))
                {
                    activator = CreateActivatorInternal(ci, activatorInfo);
                    activatorCache[ci] = activator;
                }

                return (TActivator)activator;
            }
        }

        private IActivator CreateActivatorInternal(ConstructorInfo ci, ActivatorInfo activatorInfo)
        {
            var typeBuilder = ModuleBuilder.DefineType(
                $"{ci.DeclaringType.FullName}_DynamicActivator{Array.IndexOf(ci.DeclaringType.GetConstructors(), ci)}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            typeBuilder.AddInterfaceImplementation(ActivatorType);
            if (activatorInfo != null)
            {
                typeBuilder.AddInterfaceImplementation(activatorInfo.Type);
            }

            // Field
            var sourceField = typeBuilder.DefineField(
                "source",
                CtorType,
                FieldAttributes.Private | FieldAttributes.InitOnly);

            // Property
            DefineActivatorPropertySource(typeBuilder, sourceField);

            // Constructor
            DefineActivatorConstructor(typeBuilder, sourceField);

            // Method
            DefineActivatorMethodCreate(typeBuilder, ci);
            if (activatorInfo != null)
            {
                DefineActivatorMethodCreate(typeBuilder, ci, activatorInfo);
            }

            var typeInfo = typeBuilder.CreateTypeInfo();

            return (IActivator)Activator.CreateInstance(typeInfo.AsType(), ci);
        }

        private static void DefineActivatorPropertySource(TypeBuilder typeBuilder, FieldBuilder sourceField)
        {
            var sourceProperty = typeBuilder.DefineProperty(
                "Source",
                PropertyAttributes.None,
                CtorType,
                null);
            var getSourceProperty = typeBuilder.DefineMethod(
                "get_Source",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.Final,
                CtorType,
                Type.EmptyTypes);
            sourceProperty.SetGetMethod(getSourceProperty);

            var ilGenerator = getSourceProperty.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, sourceField);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineActivatorConstructor(TypeBuilder typeBuilder, FieldBuilder sourceField)
        {
            var ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                ActivatorConstructorArgumentTypes);

            var ilGenerator = ctor.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, ObjectCotor);

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, sourceField);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineActivatorMethodCreate(TypeBuilder typeBuilder, ConstructorInfo ci)
        {
            var method = typeBuilder.DefineMethod(
                nameof(IActivator.Create),
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
                ObjectType,
                ActivatorCreateArgumentTypes);
            typeBuilder.DefineMethodOverride(method, ActivatorCreateMethodInfo);

            var ilGenerator = method.GetILGenerator();

            for (var i = 0; i < ci.GetParameters().Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.EmitLdcI4(i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                ilGenerator.EmitTypeConversion(ci.GetParameters()[i].ParameterType);
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            ilGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="ci"></param>
        /// <param name="activatorInfo"></param>
        private static void DefineActivatorMethodCreate(TypeBuilder typeBuilder, ConstructorInfo ci, ActivatorInfo activatorInfo)
        {
            var method = typeBuilder.DefineMethod(
                "Create",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
                ObjectType,
                activatorInfo.CreateArgumentTypes);
            typeBuilder.DefineMethodOverride(method, activatorInfo.CreateMethodInfo);

            var ilGenerator = method.GetILGenerator();

            for (var i = 0; i < ci.GetParameters().Length; i++)
            {
                ilGenerator.EmitLdarg(i + 1);
                ilGenerator.EmitTypeConversion(ci.GetParameters()[i].ParameterType);
            }

            ilGenerator.Emit(OpCodes.Newobj, ci);

            ilGenerator.Emit(OpCodes.Ret);
        }

        public IArrayOperator CreateArrayOperator(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            lock (sync)
            {
                if (!arrayOperatorCache.TryGetValue(type, out var arrayOperator))
                {
                    arrayOperator = CreateOperatorInternal(type);
                    arrayOperatorCache[type] = arrayOperator;
                }

                return arrayOperator;
            }
        }

        private IArrayOperator CreateOperatorInternal(Type type)
        {
            var arrayType = type.MakeArrayType();

            var typeBuilder = ModuleBuilder.DefineType(
                $"{type.FullName}_ArrayOperator",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            typeBuilder.AddInterfaceImplementation(ArrayOperatorType);

            var typeField = typeBuilder.DefineField(
                "type",
                TypeType,
                FieldAttributes.Private | FieldAttributes.InitOnly);

            // Property
            DefineArrayOperatorPropertyType(typeBuilder, typeField);

            // Constructor
            DefineArrayOperatorConstructor(typeBuilder, typeField);

            // Method
            DefineArrayOperatorMethodCreate(typeBuilder, type);
            DefineArrayOperatorMethodGetValue(typeBuilder, type, arrayType);
            DefineArrayOperatorMethodSetValue(typeBuilder, type, arrayType);

            var typeInfo = typeBuilder.CreateTypeInfo();

            return (IArrayOperator)Activator.CreateInstance(typeInfo.AsType(), type);
        }

        private static void DefineArrayOperatorPropertyType(TypeBuilder typeBuilder, FieldBuilder typeField)
        {
            var typeProperty = typeBuilder.DefineProperty(
                "Type",
                PropertyAttributes.None,
                TypeType,
                null);
            var getTypeProperty = typeBuilder.DefineMethod(
                "get_Type",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.Final,
                TypeType,
                Type.EmptyTypes);
            typeProperty.SetGetMethod(getTypeProperty);

            var ilGenerator = getTypeProperty.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, typeField);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineArrayOperatorConstructor(TypeBuilder typeBuilder, FieldBuilder typeField)
        {
            var ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                ArrayOperatorConstructorArgumentTypes);

            var ilGenerator = ctor.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, ObjectCotor);

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, typeField);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineArrayOperatorMethodCreate(TypeBuilder typeBuilder, Type type)
        {
            var method = typeBuilder.DefineMethod(
                nameof(IArrayOperator.Create),
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
                ArrayType,
                ArrayOperatorCreateArgumentTypes);
            typeBuilder.DefineMethodOverride(method, ArrayOperatorCreateMethodInfo);

            var ilGenerator = method.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_1);

            ilGenerator.Emit(OpCodes.Newarr, type);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineArrayOperatorMethodGetValue(TypeBuilder typeBuilder, Type type, Type arrayType)
        {
            var method = typeBuilder.DefineMethod(
                nameof(IArrayOperator.GetValue),
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
                ObjectType,
                ArrayOperatorGetValueArgumentTypes);
            typeBuilder.DefineMethodOverride(method, ArrayOperatorGetValueMethodInfo);

            var ilGenerator = method.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Castclass, arrayType);

            ilGenerator.Emit(OpCodes.Ldarg_2);

            ilGenerator.EmitLdelem(type);

            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, type);
            }

            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void DefineArrayOperatorMethodSetValue(TypeBuilder typeBuilder, Type type, Type arrayType)
        {
            var method = typeBuilder.DefineMethod(
                nameof(IArrayOperator.SetValue),
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
                VoidType,
                ArrayOperatorSetValueArgumentTypes);
            typeBuilder.DefineMethodOverride(method, ArrayOperatorSetValueMethodInfo);

            var ilGenerator = method.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Castclass, arrayType);

            ilGenerator.Emit(OpCodes.Ldarg_2);

            ilGenerator.Emit(OpCodes.Ldarg_3);
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, type);
            }

            ilGenerator.EmitStelem(type);

            ilGenerator.Emit(OpCodes.Ret);
        }
    }

    public interface IArrayOperator
    {
        Type Type { get; }

        Array Create(int length);

        object GetValue(Array array, int index);

        void SetValue(Array array, int index, object value);
    }

    public interface IActivator
    {
        ConstructorInfo Source { get; }

        object Create(params object[] arguments);
    }

    public interface IActivator0 : IActivator
    {
        object Create();
    }

    public interface IActivator1 : IActivator
    {
        object Create(
            object artument1);
    }

    public interface IActivator2 : IActivator
    {
        object Create(
            object artument1,
            object artument2);
    }

    public interface IActivator3 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3);
    }

    public interface IActivator4 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3,
            object artument4);
    }

    public interface IActivator5 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3,
            object artument4,
            object artument5);
    }

    public interface IActivator6 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3,
            object artument4,
            object artument5,
            object artument6);
    }

    public interface IActivator7 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3,
            object artument4,
            object artument5,
            object artument6,
            object artument7);
    }

    public interface IActivator8 : IActivator
    {
        object Create(
            object artument1,
            object artument2,
            object artument3,
            object artument4,
            object artument5,
            object artument6,
            object artument7,
            object artument8);
    }
}
