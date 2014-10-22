using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a user interface screen.
    /// </summary>
    public abstract class UIScreen : UIPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreen"/> class.
        /// </summary>
        /// <param name="definition">The asset path of the screen's definition file.</param>
        /// <param name="rootDirectory">The root directory of the screen's <see cref="ContentManager"/>.</param>
        protected UIScreen(String definition, String rootDirectory = null)
            : this(null, definition, rootDirectory)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreen"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="definition">The asset path of the screen's definition file.</param>
        /// <param name="rootDirectory">The root directory of the screen's <see cref="ContentManager"/>.</param>
        protected UIScreen(UltravioletContext uv, String definition, String rootDirectory = null)
            : base(uv)
        {
            this.content    = ContentManager.Create(rootDirectory);
            this.definition = String.IsNullOrEmpty(definition) ? null : content.Load<UIPanelDefinition>(definition);

            if (this.definition != null)
            {
                DefaultOpenTransitionDuration  = this.definition.DefaultOpenTransitionDuration;
                DefaultCloseTransitionDuration = this.definition.DefaultCloseTransitionDuration;
            }
        }

        /// <summary>
        /// Updates the screen's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update()"/>.</param>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            UpdateLayout(time);
            UpdateTransition(time);

            OnUpdating(time);
        }

        /// <summary>
        /// Draws the screen using the specified sprite batch.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw()"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the screen.</param>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.EnsureNotDisposed(this, Disposed);

            if (!IsOnCurrentWindow)
                return;

            OnDrawingBackground(time, spriteBatch);
            DrawLayout(time, spriteBatch);
            OnDrawingForeground(time, spriteBatch);
        }

        /// <summary>
        /// Gets the screen's size in pixels.
        /// </summary>
        public override Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowSize;
            }
        }

        /// <summary>
        /// Gets the screen's width in pixels.
        /// </summary>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowWidth;
            }
        }

        /// <summary>
        /// Gets the screen's height in pixels.
        /// </summary>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowHeight;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this panel is ready for input.
        /// </summary>
        public override Boolean IsReadyForInput
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsOpen)
                    return false;

                var screens = WindowScreens;
                if (screens == null)
                    return false;

                return screens.Peek() == this;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this panel is ready for input which does
        /// not require the panel to be topmost on the window.
        /// </summary>
        public override Boolean IsReadyForBackgroundInput
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return IsOpen;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this screen is opaque.
        /// </summary>
        /// <remarks>Marking a screen as opaque is a performance optimization. If a screen is opaque, then Ultraviolet
        /// will not render any screens below it in the screen stack.</remarks>
        public Boolean IsOpaque
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.isOpaque;
            }
            protected set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.isOpaque = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this screen is the topmost screen on its current window.
        /// </summary>
        public Boolean IsTopmost
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var screens = WindowScreens;
                if (screens == null)
                    return false;

                return screens.Peek() == this;
            }
        }

        /// <summary>
        /// Gets the screen's content manager.
        /// </summary>
        public ContentManager Content
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return content;
            }
        }

        /// <summary>
        /// Raises the <see cref="Opened"/> event.
        /// </summary>
        internal override void HandleOpened()
        {
            Focus();
            base.HandleOpened();
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        internal override void HandleClosed()
        {
            Blur();
            base.HandleClosed();
        }

        /// <summary>
        /// Gets the screen's definition.
        /// </summary>
        internal UIPanelDefinition Definition
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return definition; 
            }
        }

        /// <summary>
        /// Raises the <see cref="LayoutInitialized"/> event.
        /// </summary>
        protected override void OnLayoutInitialized()
        {
            if (this.definition != null)
            {
                LoadLayout(this.content, this.definition);
            }
            base.OnLayoutInitialized();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                this.content.Dispose();
            }
            base.Dispose(disposing);
        }

        // Property values.
        private readonly UIPanelDefinition definition;
        private readonly ContentManager content;
        private Boolean isOpaque;
    }
}
