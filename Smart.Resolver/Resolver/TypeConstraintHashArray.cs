namespace Smart.Resolver;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[DebuggerDisplay("{" + nameof(Diagnostics) + "}")]
internal sealed class TypeConstraintHashArray<T>
{
    private const int InitialSize = 64;

    private const int Factor = 3;

    private static readonly Node EmptyNode = new(typeof(EmptyKey), null!, default!);

    private readonly object sync = new();

    private Node[] nodes;

    private int depth;

    private int count;

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public TypeConstraintHashArray()
    {
        nodes = CreateInitialTable();
    }

    //--------------------------------------------------------------------------------
    // Private
    //--------------------------------------------------------------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateHash(Type type, object? key)
    {
        return type.GetHashCode() ^ CalcKeyHash(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalcKeyHash(object? key)
    {
        if (key is string str)
        {
            unchecked
            {
                var hash = 2166136261u;
                foreach (var c in str)
                {
                    hash = (c ^ hash) * 16777619;
                }
                return (int)hash;
            }
        }

        if (key is null)
        {
            return 0;
        }

        return key.GetHashCode();
    }

    private static int CalculateDepth(Node node)
    {
        var length = 1;
        var next = node.Next;
        while (next is not null)
        {
            length++;
            next = next.Next;
        }

        return length;
    }

    private static int CalculateDepth(Node[] targetNodes)
    {
        var depth = 0;

        for (var i = 0; i < targetNodes.Length; i++)
        {
            var node = targetNodes[i];
            if (node != EmptyNode)
            {
                depth = Math.Max(CalculateDepth(node), depth);
            }
        }

        return depth;
    }

    private static int CalculateSize(int requestSize)
    {
        uint size = 0;

        for (var i = 1L; i < requestSize; i *= 2)
        {
            size = (size << 1) + 1;
        }

        return (int)(size + 1);
    }

    private static Node[] CreateInitialTable()
    {
        var newNodes = new Node[InitialSize];

        for (var i = 0; i < newNodes.Length; i++)
        {
            newNodes[i] = EmptyNode;
        }

        return newNodes;
    }

    private static Node FindLastNode(Node node)
    {
        while (node.Next is not null)
        {
            node = node.Next;
        }

        return node;
    }

    private static void UpdateLink(ref Node node, Node addNode)
    {
        if (node == EmptyNode)
        {
            node = addNode;
        }
        else
        {
            var last = FindLastNode(node);
            last.Next = addNode;
        }
    }

    private static void RelocateNodes(Node[] nodes, Node[] oldNodes)
    {
        for (var i = 0; i < oldNodes.Length; i++)
        {
            var node = oldNodes[i];
            if (node == EmptyNode)
            {
                continue;
            }

            do
            {
                var next = node.Next;
                node.Next = null;

                UpdateLink(ref nodes[CalculateHash(node.Type, node.Key) & (nodes.Length - 1)], node);

                node = next;
            }
            while (node is not null);
        }
    }

    private void AddNode(Node node)
    {
        var requestSize = Math.Max(InitialSize, (count + 1) * Factor);
        var size = CalculateSize(requestSize);
        if (size > nodes.Length)
        {
            var newNodes = new Node[size];
            for (var i = 0; i < newNodes.Length; i++)
            {
                newNodes[i] = EmptyNode;
            }

            RelocateNodes(newNodes, nodes);

            UpdateLink(ref newNodes[CalculateHash(node.Type, node.Key) & (newNodes.Length - 1)], node);

            Interlocked.MemoryBarrier();

            nodes = newNodes;
            depth = CalculateDepth(newNodes);
            count++;
        }
        else
        {
            Interlocked.MemoryBarrier();

            var hash = CalculateHash(node.Type, node.Key);

            UpdateLink(ref nodes[hash & (nodes.Length - 1)], node);

            depth = Math.Max(CalculateDepth(nodes[hash & (nodes.Length - 1)]), depth);
            count++;
        }
    }

    //--------------------------------------------------------------------------------
    // Public
    //--------------------------------------------------------------------------------

    public DiagnosticsInfo Diagnostics
    {
        get
        {
            lock (sync)
            {
                return new DiagnosticsInfo(nodes.Length, depth, count);
            }
        }
    }

    public void Clear()
    {
        lock (sync)
        {
            var newNodes = CreateInitialTable();

            Interlocked.MemoryBarrier();

            nodes = newNodes;
            depth = 0;
            count = 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(Type type, object? key, [MaybeNullWhen(false)] out T value)
    {
        var temp = nodes;
        var node = temp[CalculateHash(type, key) & (temp.Length - 1)];
        do
        {
            if (node.Match(type, key))
            {
                value = node.Value;
                return true;
            }
            node = node.Next;
        }
        while (node is not null);

        value = default;
        return false;
    }

    public T AddIfNotExist(Type type, object? key, Func<Type, object?, T> valueFactory)
    {
        lock (sync)
        {
            // Double-checked locking
            if (TryGetValue(type, key, out var currentValue))
            {
                return currentValue;
            }

            var value = valueFactory(type, key);

            // Check if added by recursive
            if (TryGetValue(type, key, out currentValue))
            {
                return currentValue;
            }

            AddNode(new Node(type, key, value));

            return value;
        }
    }

    //--------------------------------------------------------------------------------
    // Inner
    //--------------------------------------------------------------------------------

#pragma warning disable CA1812
    private sealed class EmptyKey
    {
    }
#pragma warning restore CA1812

#pragma warning disable SA1401
    private sealed class Node
    {
        public readonly Type Type;

        public readonly object? Key;

        public readonly T Value;

        public Node? Next;

        public Node(Type type, object? key, T value)
        {
            Type = type;
            Key = key;
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Match(Type type, object? key)
        {
            if (Type != type)
            {
                return false;
            }

            if (Key is null)
            {
                return key is null;
            }

            return Key.Equals(key);
        }
    }
#pragma warning restore SA1401

    //--------------------------------------------------------------------------------
    // Diagnostics
    //--------------------------------------------------------------------------------

    public sealed class DiagnosticsInfo
    {
        public int Width { get; }

        public int Depth { get; }

        public int Count { get; }

        public DiagnosticsInfo(int width, int depth, int count)
        {
            Width = width;
            Depth = depth;
            Count = count;
        }

        public override string ToString() => $"Count={Count}, Width={Width}, Depth={Depth}";
    }
}
