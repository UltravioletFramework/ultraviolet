using System;
using System.IO;
using Ultraviolet.Presentation.Uvss.Diagnostics;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains extension methods used for serializing abstract syntax trees.
    /// </summary>
    internal static class SerializationExtensions
    {
        /// <summary>
        /// Writes a <see cref="SyntaxNode"/> to the stream.
        /// </summary>
        /// <param name="this">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void Write(this BinaryWriter @this, SyntaxNode value, Int32 version)
        {
            @this.Write(value != null);

            if (value != null)
                SyntaxSerializer.ToStream(@this, value, version);
        }

        /// <summary>
        /// Reads a <see cref="SyntaxNode"/> from the stream.
        /// </summary>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static SyntaxNode ReadSyntaxNode(this BinaryReader @this, Int32 version)
        {
            var exists = @this.ReadBoolean();

            if (exists)
                return SyntaxSerializer.FromStream(@this, version);

            return null;
        }

        /// <summary>
        /// Reads a <see cref="SyntaxNode"/> from the stream.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of syntax node to read.</typeparam>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static TSyntaxNode ReadSyntaxNode<TSyntaxNode>(this BinaryReader @this, Int32 version)
            where TSyntaxNode : SyntaxNode
        {
            return (TSyntaxNode)ReadSyntaxNode(@this, version);
        }

        /// <summary>
        /// Writes a <see cref="SyntaxList{TNode}"/> to the stream.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of syntax node contained by the list.</typeparam>
        /// <param name="this">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void Write<TSyntaxNode>(this BinaryWriter @this, SyntaxList<TSyntaxNode> value, Int32 version)
            where TSyntaxNode : SyntaxNode
        {
            Write(@this, value.Node, version);
        }

        /// <summary>
        /// Reads a <see cref="SyntaxList{TNode}"/> from the stream.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of syntax contained by the list.</typeparam>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static SyntaxList<TSyntaxNode> ReadSyntaxList<TSyntaxNode>(this BinaryReader @this, Int32 version)
            where TSyntaxNode : SyntaxNode
        {
            var node = ReadSyntaxNode(@this, version);
            return new SyntaxList<TSyntaxNode>(node);
        }

        /// <summary>
        /// Writes a <see cref="SeparatedSyntaxList{TNode}"/> to the stream.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of syntax node contained by the list.</typeparam>
        /// <param name="this">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void Write<TSyntaxNode>(this BinaryWriter @this, SeparatedSyntaxList<TSyntaxNode> value, Int32 version)
            where TSyntaxNode : SyntaxNode
        {
            Write(@this, value.Node, version);
        }

        /// <summary>
        /// Reads a <see cref="SeparatedSyntaxList{TNode}"/> from the stream.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of syntax contained by the list.</typeparam>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static SeparatedSyntaxList<TSyntaxNode> ReadSeparatedSyntaxList<TSyntaxNode>(this BinaryReader @this, Int32 version)
            where TSyntaxNode : SyntaxNode
        {
            var node = ReadSyntaxNode(@this, version);
            return new SeparatedSyntaxList<TSyntaxNode>(node);
        }

        /// <summary>
        /// Writes an array of <see cref="SyntaxNode"/> instances to the stream.
        /// </summary>
        /// <param name="this">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void Write(this BinaryWriter @this, ArrayElement<SyntaxNode>[] value, Int32 version)
        {
            @this.Write(value != null);
            if (value != null)
            {
                @this.Write(value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    @this.Write(value[i].Value, version);
                }
            }
        }

        /// <summary>
        /// Reads an array of <see cref="SyntaxNode"/> instances from the stream.
        /// </summary>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static ArrayElement<SyntaxNode>[] ReadSyntaxNodeArray(this BinaryReader @this, Int32 version)
        {
            var exists = @this.ReadBoolean();
            if (exists)
            {
                var length = @this.ReadInt32();
                var array = new ArrayElement<SyntaxNode>[length];
                for (int i = 0; i < length; i++)
                {
                    array[i].Value = @this.ReadSyntaxNode(version);
                }
                return array;
            }
            return null;
        }

        /// <summary>
        /// Writes an array of diagnostic information to the stream.
        /// </summary>
        /// <param name="this">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write to the stream.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void Write(this BinaryWriter @this, DiagnosticInfo[] value, Int32 version)
        {
            @this.Write(value != null);
            if (value != null)
            {
                @this.Write(value.Length);
                if (value != null)
                {
                    foreach (var diagnostic in value)
                        diagnostic.Serialize(@this, version);
                }
            }
        }

        /// <summary>
        /// Reads an array of diagnostic information from the stream.
        /// </summary>
        /// <param name="this">The binary reader with which to read the value.</param>
        /// <param name="node">The node that owns the diagnostics.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The value that was read.</returns>
        public static DiagnosticInfo[] ReadDiagnosticInfoArray(this BinaryReader @this, SyntaxNode node, Int32 version)
        {
            var exists = @this.ReadBoolean();
            if (exists)
            {
                var length = @this.ReadInt32();
                var array = (length == 0) ? null : new DiagnosticInfo[length];
                for (int i = 0; i < length; i++)
                {
                    var item = new DiagnosticInfo(node, @this, version);
                    array[i] = item;
                }
                return array;
            }
            return null;
        }
    }
}
