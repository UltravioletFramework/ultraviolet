using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for directive syntax nodes.
    /// </summary>
    [Preserve(AllMembers = true)]
    public abstract class UvssDirectiveSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDirectiveSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssDirectiveSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDirectiveSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssDirectiveSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
    }
}
