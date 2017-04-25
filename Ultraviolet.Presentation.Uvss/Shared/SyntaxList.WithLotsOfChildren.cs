using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with lots of children.
        /// </summary>
        [Preserve(AllMembers = true)]
        [SyntaxNodeTypeID((Byte)SyntaxNodeType.SyntaxListWithLotsOfChildren)]
        internal sealed class WithLotsOfChildren : WithManyChildrenBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithLotsOfChildren"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithLotsOfChildren(ArrayElement<SyntaxNode>[] children)
                : base(children)
            {

            }

            /// <summary>
            /// Initializes a new instance of the <see cref="WithLotsOfChildren"/> class from
            /// the specified binary reader.
            /// </summary>
            /// <param name="reader">The binary reader with which to deserialize the object.</param>
            /// <param name="version">The file version of the data being read.</param>
            internal WithLotsOfChildren(BinaryReader reader, Int32 version)
                : base(reader, version)
            {

            }
        }
    }
}
