using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with many children.
        /// </summary>
        [SyntaxNodeTypeID((Byte)SyntaxNodeType.SyntaxListWithManyChildren)]
        internal sealed class WithManyChildren : WithManyChildrenBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithManyChildren"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithManyChildren(ArrayElement<SyntaxNode>[] children)
                : base(children)
            {

            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SyntaxList.WithManyChildren"/> class from
            /// the specified binary reader.
            /// </summary>
            /// <param name="reader">The binary reader with which to deserialize the object.</param>
            /// <param name="version">The file version of the data being read.</param>
            internal WithManyChildren(BinaryReader reader, Int32 version)
                : base(reader, version)
            {

            }
        }
    }
}
