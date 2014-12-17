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
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="rootDirectory">The root directory of the screen's <see cref="ContentManager"/>.</param>
        protected UIScreen(String definitionAsset, String rootDirectory = null)
            : this(null, definitionAsset, rootDirectory)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreen"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="definitionAsset">The asset path of the screen's definition file.</param>
        /// <param name="rootDirectory">The root directory of the screen's <see cref="ContentManager"/>.</param>
        protected UIScreen(UltravioletContext uv, String definitionAsset, String rootDirectory = null)
            : base(uv)
        {
            this.content = ContentManager.Create(rootDirectory);
            
            var definition = String.IsNullOrEmpty(definitionAsset) ? null : content.Load<UIPanelDefinition>(definitionAsset);
            if (definition != null)
            {
                DefaultOpenTransitionDuration  = definition.DefaultOpenTransitionDuration;
                DefaultCloseTransitionDuration = definition.DefaultCloseTransitionDuration;

                LoadView(definition);
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            UpdateView(time);
            UpdateTransition(time);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.EnsureNotDisposed(this, Disposed);

            if (!IsOnCurrentWindow)
                return;

            OnDrawingBackground(time, spriteBatch);
            DrawView(time, spriteBatch);
            OnDrawingForeground(time, spriteBatch);
        }

        /// <inheritdoc/>
        public override Int32 X
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public override Int32 Y
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public override Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowSize;
            }
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowWidth;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return WindowHeight;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        internal override void HandleOpened()
        {
            Focus();
            base.HandleOpened();
        }

        /// <inheritdoc/>
        internal override void HandleClosed()
        {
            Blur();
            base.HandleClosed();
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                this.content.Dispose();
            }
            base.Dispose(disposing);
        }

        // Property values.
        private readonly ContentManager content;
        private Boolean isOpaque;
    }
}
