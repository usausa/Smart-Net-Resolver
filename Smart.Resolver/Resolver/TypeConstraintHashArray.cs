namespace Smart.Resolver
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Smart.Resolver.Constraints;

    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    internal class TypeConstraintHashArray<T>
    {
        private const int InitialSize = 64;

        private const int Factor = 3;

        private static readonly Node EmptyNode = new Node(typeof(EmptyKey), null, default);

        private readonly object sync = new object();

        private Node[] nodes;

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
        private static int CalculateHash(Type type, IConstraint constraint)
        {
            var hash = type.GetHashCode();
            if (constraint != null)
            {
                hash ^= constraint.GetHashCode();
            }

            return hash;
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
            while (node.Next != null)
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

                    UpdateLink(ref nodes[CalculateHash(node.Type, node.Constraint) & (nodes.Length - 1)], node);

                    node = next;
                }
                while (node != null);
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

                UpdateLink(ref newNodes[CalculateHash(node.Type, node.Constraint) & (newNodes.Length - 1)], node);

                Interlocked.MemoryBarrier();

                nodes = newNodes;

                count++;
            }
            else
            {
                Interlocked.MemoryBarrier();

                UpdateLink(ref nodes[CalculateHash(node.Type, node.Constraint) & (nodes.Length - 1)], node);

                count++;
            }
        }

        //--------------------------------------------------------------------------------
        // Public
        //--------------------------------------------------------------------------------

        public int Count
        {
            get
            {
                lock (sync)
                {
                    return count;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Performance")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(Type type, IConstraint constraint, out T value)
        {
            var node = nodes[CalculateHash(type, constraint) & (nodes.Length - 1)];
            do
            {
                if ((node.Type == type) && node.Constraint.Equals(constraint))
                {
                    value = node.Value;
                    return true;
                }
                node = node.Next;
            }
            while (node != null);

            value = default;
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Performance")]
        public T AddIfNotExist(Type type, IConstraint constraint, Func<Type, IConstraint, T> valueFactory)
        {
            lock (sync)
            {
                // Double checked locking
                if (TryGetValue(type, constraint, out var currentValue))
                {
                    return currentValue;
                }

                var value = valueFactory(type, constraint);

                // Check if added by recursive
                if (TryGetValue(type, constraint, out currentValue))
                {
                    return currentValue;
                }

                AddNode(new Node(type, constraint, value));

                return value;
            }
        }

        //--------------------------------------------------------------------------------
        // Inner
        //--------------------------------------------------------------------------------

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Framework only")]
        private sealed class EmptyKey
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Performance")]
        private sealed class Node
        {
            public readonly Type Type;

            public readonly IConstraint Constraint;

            public readonly T Value;

            public Node Next;

            public Node(Type type, IConstraint constraint, T value)
            {
                Type = type;
                Constraint = constraint;
                Value = value;
            }
        }
    }
}
