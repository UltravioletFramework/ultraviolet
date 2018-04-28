using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS trigger actions.
    /// </summary>
    public abstract class UvssTriggerActionBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerConditionSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssTriggerActionBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlaySfxTriggerActionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssTriggerActionBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
    }
}
