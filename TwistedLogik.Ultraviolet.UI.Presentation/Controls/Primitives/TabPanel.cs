using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the part of a <see cref="TabControl"/> which displays the tabbed list of items.
    /// </summary>
    public class TabPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TabPanel(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawBlank(dc, null, Color.Lime);

            base.DrawOverride(time, dc);
        }
    }
}
