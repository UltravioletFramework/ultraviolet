using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS triggers.
    /// </summary>
    public abstract class UvssTriggerBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTriggerBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssTriggerBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTriggerBaseSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssTriggerBaseSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }
        
        /// <summary>
        /// Gets the trigger's collection of actions.
        /// </summary>
        public abstract IEnumerable<UvssTriggerActionBaseSyntax> Actions { get; }
    }
}
