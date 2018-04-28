using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a missing syntax list.
        /// </summary>
        [SyntaxNodeTypeID((Byte)SyntaxNodeType.SyntaxListMissing)]
        internal sealed class MissingList : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.MissingList"/> class.
            /// </summary>
            internal MissingList()
            {
                IsMissing = true;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.MissingList"/> class from
            /// the specified binary reader.
            /// </summary>
            /// <param name="reader">The binary reader with which to deserialize the object.</param>
            /// <param name="version">The file version of the data being read.</param>
            internal MissingList(BinaryReader reader, Int32 version)
                : base(reader, version)
            {

            }
            
            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                return null;
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                throw new InvalidOperationException();
            }
        }
    }
}