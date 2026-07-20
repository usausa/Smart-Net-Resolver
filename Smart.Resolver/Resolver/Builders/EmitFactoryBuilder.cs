namespace Smart.Resolver.Builders;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using Smart.Reflection.Emit;

[RequiresDynamicCode("EmitFactoryBuilder uses Reflection.Emit which is not supported in AOT environments.")]
public sealed class EmitFactoryBuilder : IFactoryBuilder
{
    private static readonly Action<IResolver, object>[] EmptyActions = [];

    private static readonly HolderBuilder DefaultHolderBuilder = new();

    private static readonly LeafFactoryBuilder DefaultLeafFactoryBuilder = new();

    private static readonly ConditionalWeakTable<object, Recipe> Recipes = [];

    private const int InlineBudget = 32;

    public bool UseLeafFactory { get; init; } = true;

    public bool UseConstantEmbedding { get; init; } = true;

    public bool UseInlineExpansion { get; init; } = true;

    public Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object?>[] factories, object?[] constants, Action<IResolver, object>[] actions)
    {
        var budget = InlineBudget;
        var nodes = new Node[factories.Length];
        var direct = true;
        for (var i = 0; i < factories.Length; i++)
        {
            nodes[i] = BuildNode(UseConstantEmbedding ? constants[i] : null, factories[i], ref budget);
            direct &= nodes[i] is FactoryNode;
        }

        if (direct)
        {
            return CreateDirectFactory(ci, factories, actions);
        }

        return CreatePlannedFactory(ci, factories, constants, actions, nodes);
    }

    private Func<IResolver, object> CreateDirectFactory(ConstructorInfo ci, Func<IResolver, object?>[] factories, Action<IResolver, object>[] actions)
    {
        var holder = DefaultHolderBuilder.CreateHolder(factories, actions);

        if (UseLeafFactory && (holder is null) && !ci.DeclaringType!.IsValueType && (ci.GetParameters().Length == 0) && ci.IsPublic && ci.DeclaringType.IsVisible)
        {
            var leafFactory = DefaultLeafFactoryBuilder.CreateFactory(ci);
            Recipes.Add(leafFactory, new Recipe(ci, [], []));
            return leafFactory;
        }

        var holderType = holder?.GetType() ?? typeof(object);

        var returnType = ci.DeclaringType!.IsValueType ? typeof(object) : ci.DeclaringType;

        var dynamicMethod = new DynamicMethod(string.Empty, returnType, [holderType, typeof(IResolver)], true);
        var ilGenerator = dynamicMethod.GetILGenerator();

        for (var i = 0; i < factories.Length; i++)
        {
            var invokeMethod = factories[i].GetType().GetMethod("Invoke");
            if ((invokeMethod is null) ||
                (invokeMethod.GetParameters().Length != 1) ||
                (invokeMethod.GetParameters()[0].ParameterType != typeof(IResolver)))
            {
                throw new ArgumentException($"Invalid factory[{i}]");
            }

            var field = holderType.GetField($"factory{i}");

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, field!);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Call, invokeMethod);
            ilGenerator.EmitTypeConversion(ci.GetParameters()[i].ParameterType);
        }

        ilGenerator.Emit(OpCodes.Newobj, ci);

        if (actions.Length > 0)
        {
            ilGenerator.DeclareLocal(ci.DeclaringType);

            ilGenerator.Emit(OpCodes.Stloc_0);

            for (var i = 0; i < actions.Length; i++)
            {
                var invokeMethod = actions[i].GetType().GetMethod("Invoke");
                if ((invokeMethod is null) ||
                    (invokeMethod.GetParameters().Length != 2) ||
                    (invokeMethod.GetParameters()[0].ParameterType != typeof(IResolver)))
                {
                    throw new ArgumentException($"Invalid actions[{i}]");
                }

                var field = holderType.GetField($"action{i}");

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field!);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
            }

            ilGenerator.Emit(OpCodes.Ldloc_0);
        }

        if (ci.DeclaringType.IsValueType)
        {
            ilGenerator.Emit(OpCodes.Box, ci.DeclaringType);
        }

        ilGenerator.Emit(OpCodes.Ret);

        var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), returnType);
        var factory = (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, holder);
        if ((actions.Length == 0) && !ci.DeclaringType.IsValueType)
        {
            Recipes.Add(factory, new Recipe(ci, new object?[factories.Length], factories));
        }

        return factory;
    }

    public Func<IResolver, object> CreateArrayFactory(Type type, Func<IResolver, object>[] factories)
    {
        if (UseInlineExpansion && (factories.Length > 0))
        {
            var budget = InlineBudget;
            var nodes = new Node[factories.Length];
            var expanded = false;
            for (var i = 0; i < factories.Length; i++)
            {
                nodes[i] = BuildNode(null, factories[i], ref budget);
                expanded |= nodes[i] is not FactoryNode;
            }

            if (expanded)
            {
                return CreateArrayFactoryFromNodes(type, nodes);
            }
        }

        var holder = DefaultHolderBuilder.CreateHolder(factories, EmptyActions);
        var holderType = holder?.GetType() ?? typeof(object);

        var arrayType = type.MakeArrayType();

        var dynamicMethod = new DynamicMethod(string.Empty, arrayType, [holderType, typeof(IResolver)], true);
        var ilGenerator = dynamicMethod.GetILGenerator();

        ilGenerator.EmitLdcI4(factories.Length);
        ilGenerator.Emit(OpCodes.Newarr, type);

        for (var i = 0; i < factories.Length; i++)
        {
            var field = holderType.GetField($"factory{i}");

            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.EmitLdcI4(i);

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, field!);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            var invokeMethod = factories[i].GetType().GetMethod("Invoke", [typeof(IResolver)]);
            ilGenerator.Emit(OpCodes.Call, invokeMethod!);

            ilGenerator.Emit(OpCodes.Stelem_Ref);
        }

        ilGenerator.Emit(OpCodes.Ret);

        var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), arrayType);
        return (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, holder);
    }

    private static Func<IResolver, object> CreateArrayFactoryFromNodes(Type type, Node[] nodes)
    {
        var constantValues = new List<object>();
        var factoryValues = new List<Func<IResolver, object?>>();
        foreach (var node in nodes)
        {
            CollectSlots(node, constantValues, factoryValues);
        }

        var holder = DefaultHolderBuilder.CreateMixedHolder([.. constantValues], [.. factoryValues], EmptyActions);
        var holderType = holder.GetType();

        var arrayType = type.MakeArrayType();

        var dynamicMethod = new DynamicMethod(string.Empty, arrayType, [holderType, typeof(IResolver)], true);
        var ilGenerator = dynamicMethod.GetILGenerator();

        ilGenerator.EmitLdcI4(nodes.Length);
        ilGenerator.Emit(OpCodes.Newarr, type);

        var constantIndex = 0;
        var factoryIndex = 0;
        for (var i = 0; i < nodes.Length; i++)
        {
            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.EmitLdcI4(i);
            EmitNode(ilGenerator, holderType, nodes[i], typeof(object), ref constantIndex, ref factoryIndex);
            ilGenerator.Emit(OpCodes.Stelem_Ref);
        }

        ilGenerator.Emit(OpCodes.Ret);

        var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), arrayType);
        return (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, holder);
    }

    private Func<IResolver, object> CreatePlannedFactory(ConstructorInfo ci, Func<IResolver, object?>[] factories, object?[] constants, Action<IResolver, object>[] actions, Node[] nodes)
    {
        var parameters = ci.GetParameters();

        var constantValues = new List<object>();
        var factoryValues = new List<Func<IResolver, object?>>();
        foreach (var node in nodes)
        {
            CollectSlots(node, constantValues, factoryValues);
        }

        var holder = DefaultHolderBuilder.CreateMixedHolder([.. constantValues], [.. factoryValues], actions);
        var holderType = holder.GetType();

        var returnType = ci.DeclaringType!.IsValueType ? typeof(object) : ci.DeclaringType;

        var dynamicMethod = new DynamicMethod(string.Empty, returnType, [holderType, typeof(IResolver)], true);
        var ilGenerator = dynamicMethod.GetILGenerator();

        var constantIndex = 0;
        var factoryIndex = 0;
        for (var i = 0; i < nodes.Length; i++)
        {
            EmitNode(ilGenerator, holderType, nodes[i], parameters[i].ParameterType, ref constantIndex, ref factoryIndex);
        }

        ilGenerator.Emit(OpCodes.Newobj, ci);

        if (actions.Length > 0)
        {
            ilGenerator.DeclareLocal(ci.DeclaringType);

            ilGenerator.Emit(OpCodes.Stloc_0);

            for (var i = 0; i < actions.Length; i++)
            {
                var invokeMethod = actions[i].GetType().GetMethod("Invoke");
                if ((invokeMethod is null) ||
                    (invokeMethod.GetParameters().Length != 2) ||
                    (invokeMethod.GetParameters()[0].ParameterType != typeof(IResolver)))
                {
                    throw new ArgumentException($"Invalid actions[{i}]");
                }

                var field = holderType.GetField($"action{i}");

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field!);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
            }

            ilGenerator.Emit(OpCodes.Ldloc_0);
        }

        if (ci.DeclaringType.IsValueType)
        {
            ilGenerator.Emit(OpCodes.Box, ci.DeclaringType);
        }

        ilGenerator.Emit(OpCodes.Ret);

        var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), returnType);
        var factory = (Func<IResolver, object>)dynamicMethod.CreateDelegate(funcType, holder);
        if ((actions.Length == 0) && !ci.DeclaringType.IsValueType)
        {
            Recipes.Add(factory, new Recipe(ci, UseConstantEmbedding ? constants : new object?[factories.Length], factories));
        }

        return factory;
    }

    private Node BuildNode(object? constant, Func<IResolver, object?> factory, ref int budget)
    {
        if (constant is not null)
        {
            return new ConstantNode(constant);
        }

        if (UseInlineExpansion && (budget > 0) && Recipes.TryGetValue(factory, out var recipe))
        {
            budget--;

            var parameters = recipe.Ci.GetParameters();
            var args = new Node[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                args[i] = BuildNode(recipe.Constants[i], recipe.Factories[i], ref budget);
            }

            return new NewNode(recipe.Ci, args);
        }

        return new FactoryNode(factory);
    }

    private static void CollectSlots(Node node, List<object> constants, List<Func<IResolver, object?>> factories)
    {
        switch (node)
        {
            case ConstantNode constantNode:
                constants.Add(constantNode.Value);
                break;
            case FactoryNode factoryNode:
                factories.Add(factoryNode.Factory);
                break;
            case NewNode newNode:
                foreach (var arg in newNode.Args)
                {
                    CollectSlots(arg, constants, factories);
                }

                break;
        }
    }

    private static void EmitNode(ILGenerator ilGenerator, Type holderType, Node node, Type parameterType, ref int constantIndex, ref int factoryIndex)
    {
        switch (node)
        {
            case ConstantNode:
                var constantField = holderType.GetField($"const{constantIndex}");
                constantIndex++;

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, constantField!);
                ilGenerator.EmitTypeConversion(parameterType);
                break;

            case FactoryNode factoryNode:
                var invokeMethod = factoryNode.Factory.GetType().GetMethod("Invoke");
                if ((invokeMethod is null) ||
                    (invokeMethod.GetParameters().Length != 1) ||
                    (invokeMethod.GetParameters()[0].ParameterType != typeof(IResolver)))
                {
                    throw new ArgumentException($"Invalid factory[{factoryIndex}]");
                }

                var factoryField = holderType.GetField($"factory{factoryIndex}");
                factoryIndex++;

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, factoryField!);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Call, invokeMethod);
                ilGenerator.EmitTypeConversion(parameterType);
                break;

            case NewNode newNode:
                var parameters = newNode.Ci.GetParameters();
                for (var i = 0; i < newNode.Args.Length; i++)
                {
                    EmitNode(ilGenerator, holderType, newNode.Args[i], parameters[i].ParameterType, ref constantIndex, ref factoryIndex);
                }

                // The concrete type is assignable to the parameter type; no conversion.
                ilGenerator.Emit(OpCodes.Newobj, newNode.Ci);
                break;
        }
    }

    private sealed class Recipe
    {
        public ConstructorInfo Ci { get; }

        public object?[] Constants { get; }

        public Func<IResolver, object?>[] Factories { get; }

        public Recipe(ConstructorInfo ci, object?[] constants, Func<IResolver, object?>[] factories)
        {
            Ci = ci;
            Constants = constants;
            Factories = factories;
        }
    }

    private abstract class Node
    {
    }

    private sealed class ConstantNode : Node
    {
        public object Value { get; }

        public ConstantNode(object value)
        {
            Value = value;
        }
    }

    private sealed class FactoryNode : Node
    {
        public Func<IResolver, object?> Factory { get; }

        public FactoryNode(Func<IResolver, object?> factory)
        {
            Factory = factory;
        }
    }

    private sealed class NewNode : Node
    {
        public ConstructorInfo Ci { get; }

        public Node[] Args { get; }

        public NewNode(ConstructorInfo ci, Node[] args)
        {
            Ci = ci;
            Args = args;
        }
    }

    private sealed class LeafFactoryBuilder
    {
#if NET9_0_OR_GREATER
        private readonly Lock sync = new();
#else
        private readonly object sync = new();
#endif

        private AssemblyBuilder? assemblyBuilder;

        private ModuleBuilder? moduleBuilder;

        private int typeNo;

        public Func<IResolver, object> CreateFactory(ConstructorInfo ci)
        {
            Type type;
            lock (sync)
            {
                if (moduleBuilder is null)
                {
                    assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName("LeafFactoryAssembly"),
                        AssemblyBuilderAccess.Run);
                    moduleBuilder = assemblyBuilder.DefineDynamicModule(
                        "LeafFactoryModule");
                }

                var typeBuilder = moduleBuilder.DefineType(
                    $"LeafFactory_{typeNo}",
                    TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                typeNo++;

                var method = typeBuilder.DefineMethod(
                    "Create",
                    MethodAttributes.Public | MethodAttributes.HideBySig,
                    typeof(object),
                    [typeof(IResolver)]);
                var ilGenerator = method.GetILGenerator();
                ilGenerator.Emit(OpCodes.Newobj, ci);
                ilGenerator.Emit(OpCodes.Ret);

                var typeInfo = typeBuilder.CreateTypeInfo();
                type = typeInfo.AsType();
            }

            var instance = Activator.CreateInstance(type)!;
            return (Func<IResolver, object>)Delegate.CreateDelegate(typeof(Func<IResolver, object>), instance, "Create");
        }
    }

    private sealed class HolderBuilder
    {
#if NET9_0_OR_GREATER
        private readonly Lock sync = new();
#else
        private readonly object sync = new();
#endif

        private readonly Dictionary<Tuple<int, int>, Type> cache = [];

        private readonly Dictionary<Tuple<int, int, int>, Type> mixedCache = [];

        private AssemblyBuilder? assemblyBuilder;

        private ModuleBuilder? moduleBuilder;

        public object CreateMixedHolder(object[] constants, Func<IResolver, object?>[] factories, Action<IResolver, object>[] actions)
        {
            var type = ResolveMixedType(constants.Length, factories.Length, actions.Length);
            var holder = Activator.CreateInstance(type)!;

            for (var i = 0; i < constants.Length; i++)
            {
                var field = type.GetField($"const{i}");
                field!.SetValue(holder, constants[i]);
            }

            for (var i = 0; i < factories.Length; i++)
            {
                var field = type.GetField($"factory{i}");
                field!.SetValue(holder, factories[i]);
            }

            for (var i = 0; i < actions.Length; i++)
            {
                var field = type.GetField($"action{i}");
                field!.SetValue(holder, actions[i]);
            }

            return holder;
        }

        private Type ResolveMixedType(int constantCount, int factoryCount, int actionCount)
        {
            var key = Tuple.Create(constantCount, factoryCount, actionCount);
            lock (mixedCache)
            {
                if (!mixedCache.TryGetValue(key, out var type))
                {
                    type = CreateMixedType(constantCount, factoryCount, actionCount);
                    mixedCache[key] = type;
                }

                return type;
            }
        }

        private Type CreateMixedType(int constantCount, int factoryCount, int actionCount)
        {
            // Define type
            var typeBuilder = DefineType($"MixedHolder_{constantCount}_{factoryCount}_{actionCount}");

            // Define constant fields
            for (var i = 0; i < constantCount; i++)
            {
                typeBuilder.DefineField(
                    $"const{i}",
                    typeof(object),
                    FieldAttributes.Public);
            }

            // Define factory fields
            for (var i = 0; i < factoryCount; i++)
            {
                typeBuilder.DefineField(
                    $"factory{i}",
                    typeof(Func<IResolver, object>),
                    FieldAttributes.Public);
            }

            // Define action fields
            for (var i = 0; i < actionCount; i++)
            {
                typeBuilder.DefineField(
                    $"action{i}",
                    typeof(Action<IResolver, object>),
                    FieldAttributes.Public);
            }

            var typeInfo = typeBuilder.CreateTypeInfo();
            return typeInfo.AsType();
        }

        public object? CreateHolder(Func<IResolver, object?>[] factories, Action<IResolver, object>[] actions)
        {
            if ((factories.Length == 0) && (actions.Length == 0))
            {
                return null;
            }

            var type = ResolveFactoryType(factories, actions);
            var holder = Activator.CreateInstance(type);

            for (var i = 0; i < factories.Length; i++)
            {
                var field = type.GetField($"factory{i}");
                field!.SetValue(holder, factories[i]);
            }

            for (var i = 0; i < actions.Length; i++)
            {
                var field = type.GetField($"action{i}");
                field!.SetValue(holder, actions[i]);
            }

            return holder;
        }

        private Type ResolveFactoryType(Func<IResolver, object?>[] factories, Action<IResolver, object>[] actions)
        {
            var key = Tuple.Create(factories.Length, actions.Length);
            lock (cache)
            {
                if (!cache.TryGetValue(key, out var type))
                {
                    type = CreateType(factories.Length, actions.Length);
                    cache[key] = type;
                }

                return type;
            }
        }

        private TypeBuilder DefineType(string name)
        {
            lock (sync)
            {
                if (moduleBuilder is null)
                {
                    assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName("EmitBuilderAssembly"),
                        AssemblyBuilderAccess.Run);
                    moduleBuilder = assemblyBuilder.DefineDynamicModule(
                        "EmitBuilderModule");
                }

                var typeBuilder = moduleBuilder.DefineType(
                    name,
                    TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                return typeBuilder;
            }
        }

        private Type CreateType(int factoryCount, int actionCount)
        {
            // Define type
            var typeBuilder = DefineType($"Holder_{factoryCount}_{actionCount}");

            // Define factory fields
            for (var i = 0; i < factoryCount; i++)
            {
                typeBuilder.DefineField(
                    $"factory{i}",
                    typeof(Func<IResolver, object>),
                    FieldAttributes.Public);
            }

            // Define action fields
            for (var i = 0; i < actionCount; i++)
            {
                typeBuilder.DefineField(
                    $"action{i}",
                    typeof(Action<IResolver, object>),
                    FieldAttributes.Public);
            }

            var typeInfo = typeBuilder.CreateTypeInfo();
            // ReSharper disable once RedundantSuppressNullableWarningExpression <= net6.0
            return typeInfo!.AsType();
        }
    }
}
