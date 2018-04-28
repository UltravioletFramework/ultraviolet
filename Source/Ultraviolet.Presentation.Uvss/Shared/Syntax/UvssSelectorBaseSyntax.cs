using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base type for UVSS selectors.
    /// </summary>
    public abstract class UvssSelectorBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssSelectorBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorBaseSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
        
        /// <summary>
        /// Gets the selector's list of components.
        /// </summary>
        public abstract SyntaxList<SyntaxNode> Components { get; }

        /// <summary>
        /// Gets a collection containing the selector's parts.
        /// </summary>
        public abstract IEnumerable<UvssSelectorPartSyntax> Parts { get; }

        /// <summary>
        /// Gets a collection containing the selector's combinators.
        /// </summary>
        public abstract IEnumerable<SyntaxToken> Combinators { get; }
    }
}
