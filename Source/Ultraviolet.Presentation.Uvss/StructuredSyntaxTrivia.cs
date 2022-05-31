using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents structured trivia.
    /// </summary>
    public abstract class StructuredTriviaSyntax : SyntaxTrivia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredTriviaSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        public StructuredTriviaSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredTriviaSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public StructuredTriviaSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }

        /// <inheritdoc/>
        public sealed override Boolean HasStructure => true;
    }
}
