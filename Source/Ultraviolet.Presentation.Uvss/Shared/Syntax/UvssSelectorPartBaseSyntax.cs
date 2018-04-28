using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base type for UVSS selector parts.
    /// </summary>
    public abstract class UvssSelectorPartBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssSelectorPartBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorPartBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
    }
}
