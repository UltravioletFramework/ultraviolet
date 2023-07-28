using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with two children.
        /// </summary>
        [SyntaxNodeTypeID((Byte)SyntaxNodeType.SyntaxListWithTwoChildren)]
        internal sealed class WithTwoChildren : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithTwoChildren"/> class.
            /// </summary>
            /// <param name="child0">The list's first child.</param>
            /// <param name="child1">The list's second child.</param>
            internal WithTwoChildren(SyntaxNode child0, SyntaxNode child1)
            {
                SlotCount = 2;

                this.child0 = child0;
                ChangeParent(child0);

                this.child1 = child1;
                ChangeParent(child1);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithTwoChildren"/> class from
            /// the specified binary reader.
            /// </summary>
            /// <param name="reader">The binary reader with which to deserialize the object.</param>
            /// <param name="version">The file version of the data being read.</param>
            private WithTwoChildren(BinaryReader reader, Int32 version)
                : base(reader, version)
            {
                this.child0 = reader.ReadSyntaxNode(version);
                ChangeParent(this.child0);

                this.child1 = reader.ReadSyntaxNode(version);
                ChangeParent(this.child1);
            }

            /// <inheritdoc/>
            public override void Serialize(BinaryWriter writer, Int32 version)
            {
                base.Serialize(writer, version);

                writer.Write(child0, version);
                writer.Write(child1, version);
            }

            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                switch (index)
                {
                    case 0: return child0;
                    case 1: return child1;
                }
                return null;
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                array[offset].Value = child0;
                array[offset + 1].Value = child1;
            }

            // List children.
            private SyntaxNode child0;
            private SyntaxNode child1;
        }
    }
}
