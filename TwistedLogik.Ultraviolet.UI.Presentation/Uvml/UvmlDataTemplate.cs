using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML node which produces a data template.
    /// </summary>
    public sealed class UvmlDataTemplate : UvmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlDataTemplate"/> class.
        /// </summary>
        /// <param name="template">The data template which is represented by this node.</param>
        internal UvmlDataTemplate(DataTemplate template)
        {
            Contract.Require(template, nameof(template));

            this.Template = template;
        }

        /// <inheritdoc/>
        public override Object Instantiate(UltravioletContext uv, UvmlInstantiationContext context)
        {
            return Template;
        }

        /// <summary>
        /// Gets the literal value which is represented by this node.
        /// </summary>
        public DataTemplate Template { get; }
    }
}
