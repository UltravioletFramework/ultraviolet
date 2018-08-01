using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to push a link onto the link stack.
    /// </summary>
    public struct TextLayoutLinkCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutLinkCommand"/> structure.
        /// </summary>
        /// <param name="linkTargetIndex">The index of the link target within the command stream's link target registry.</param>
        public TextLayoutLinkCommand(Int16 linkTargetIndex)
        {
            this.CommandType = TextLayoutCommandType.PushLink;
            this.LinkTargetIndex = linkTargetIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the index of the link target within the command stream's link target registry.
        /// </summary>
        public Int16 LinkTargetIndex { get; private set; }
    }
}
