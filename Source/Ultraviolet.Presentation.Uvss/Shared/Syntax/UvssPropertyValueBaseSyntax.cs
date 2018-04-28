using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for property values.
    /// </summary>
    public abstract class UvssPropertyValueBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssPropertyValueBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueBaseSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPropertyValueBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public abstract String Value
        {
            get;
        }
    }
}
