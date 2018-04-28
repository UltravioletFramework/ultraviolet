using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS identifiers.
    /// </summary>
    public abstract class UvssIdentifierBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIdentifierBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssIdentifierBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIdentifierBaseSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssIdentifierBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
        
        /// <summary>
        /// Gets the identifier's text.
        /// </summary>
        public abstract String Text { get; }
    }
}
