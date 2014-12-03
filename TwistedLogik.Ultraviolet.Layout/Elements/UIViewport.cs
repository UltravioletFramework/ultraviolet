using System;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents the top-level container for UI elements.
    /// </summary>
    public sealed class UIViewport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIViewport"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UIViewport(UltravioletContext uv)
        {
            this.canvas = new Canvas(uv, null);
        }

        /// <summary>
        /// Updates the viewport's state and the state of its contained elements.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Canvas.Update(time);
        }

        /// <summary>
        /// Sets the content managers used to load UI assets.
        /// </summary>
        /// <param name="global">The content manager used to load globally-sourced assets.</param>
        /// <param name="local">The content manager used to load locally-sourced assets.</param>
        public void SetContentManagers(ContentManager global, ContentManager local)
        {
            this.globalContent = global;
            this.localContent  = local;

            Canvas.ReloadContent();
        }

        /// <summary>
        /// Gets the content manager used to load globally-sourced assets.
        /// </summary>
        public ContentManager GlobalContent
        {
            get { return globalContent; }
        }

        /// <summary>
        /// Gets the content manager used to load locally-sourced assets.
        /// </summary>
        public ContentManager LocalContent
        {
            get { return localContent; }
        }

        /// <summary>
        /// Gets or sets the area on the screen that the UI viewport occupies.
        /// </summary>
        public Rectangle ScreenArea
        {
            get { return screenArea; }
            set
            {
                if (!screenArea.Equals(value))
                {
                    screenArea = value;
                    Canvas.ContainerRelativeLayout = new Rectangle(0, 0, value.Width, value.Height);
                    Canvas.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the viewport's top left corner.
        /// </summary>
        public Int32 X
        {
            get { return screenArea.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the viewport's top left corner.
        /// </summary>
        public Int32 Y
        {
            get { return screenArea.Y; }
        }

        /// <summary>
        /// Gets the viewport's width on the screen.
        /// </summary>
        public Int32 Width
        {
            get { return screenArea.Width; }
        }

        /// <summary>
        /// Gets the viewport's height on the screen.
        /// </summary>
        public Int32 Height
        {
            get { return screenArea.Height; }
        }

        /// <summary>
        /// Gets the <see cref="Canvas"/> that contains all of the UI's elements.
        /// </summary>
        public Canvas Canvas
        {
            get { return canvas; }
        }

        // Property values.
        private ContentManager globalContent;
        private ContentManager localContent;
        private Rectangle screenArea;
        private Canvas canvas;
    }
}
