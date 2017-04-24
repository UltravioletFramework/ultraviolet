using System;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a node in a UVML template.
    /// </summary>
    public abstract class UvmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlNode"/> class.
        /// </summary>
        internal UvmlNode() { }

        /// <summary>
        /// Instantiates the value represented by the node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="context">The instantiation context for this node.</param>
        /// <returns>The object which was instantiated by the node.</returns>
        public abstract Object Instantiate(UltravioletContext uv, UvmlInstantiationContext context);
    }
}
