using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the result of laying out formatted text.
    /// </summary>
    public sealed class TextLayoutResult : TextResult<TextLayoutToken>
    {
        /// <summary>
        /// Clears the result.
        /// </summary>
        public override void Clear()
        {
            ActualWidth = 0;
            ActualHeight = 0;
            base.Clear();
        }

        /// <summary>
        /// Gets the bounds of the laid-out text relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the layout's actual width.
        /// </summary>
        public Int32 ActualWidth
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the layout's actual height.
        /// </summary>
        public Int32 ActualHeight
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the layout settings used to create this layout.
        /// </summary>
        public TextLayoutSettings Settings
        {
            get;
            internal set;
        }
    }
}
