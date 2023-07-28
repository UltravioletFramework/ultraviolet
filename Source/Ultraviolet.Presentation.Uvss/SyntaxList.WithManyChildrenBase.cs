using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents the base class for syntax lists with many children.
        /// </summary>
        internal abstract class WithManyChildrenBase : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithManyChildrenBase"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithManyChildrenBase(ArrayElement<SyntaxNode>[] children)
            {
                SlotCount = children.Length;

                this.children = children;
                for (int i = 0; i < children.Length; i++)
                    ChangeParent(children[i]);
            }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithManyChildrenBase"/> class from
            /// the specified binary reader.
            /// </summary>
            /// <param name="reader">The binary reader with which to deserialize the object.</param>
            /// <param name="version">The file version of the data being read.</param>
            internal WithManyChildrenBase(BinaryReader reader, Int32 version)
                : base(reader, version)
            {
                var children = reader.ReadSyntaxNodeArray(version);
                if (children != null)
                {
                    for (int i = 0; i < children.Length; i++)
                        ChangeParent(children[i]);
                }
                this.children = children;
            }

            /// <inheritdoc/>
            public override void Serialize(BinaryWriter writer, Int32 version)
            {
                base.Serialize(writer, version);

                writer.Write(children, version);
            }

            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                return children[index].Value;
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                Array.Copy(children, 0, array, offset, children.Length);
            }

            // List children.
            protected readonly ArrayElement<SyntaxNode>[] children;
        }
    }
}
