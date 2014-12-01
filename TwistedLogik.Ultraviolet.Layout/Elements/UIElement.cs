using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// The base class for all UI elements.
    /// </summary>
    public abstract class UIElement : DependencyObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public UIElement(UltravioletContext uv, String id)
        {
            Contract.Require(uv, "uv");

            this.uv = uv;
            this.id = id;
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Digest();
        }

        /// <summary>
        /// Gets the Ultraviolet context that created the element.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets the element's unique identifier within its layout.
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the <see cref="UIContainer"/> that contains this element.
        /// </summary>
        public UIContainer Container
        {
            get { return container; }
            internal set { container = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an anonymous element.
        /// </summary>
        /// <remarks>An anonymous element is one which has no explicit identifier.</remarks>
        public Boolean IsAnonymous
        {
            get { return String.IsNullOrEmpty(id); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled.
        /// </summary>
        public Boolean IsEnabled
        {
            get { return GetValue<Boolean>(dpIsEnabled); }
            set { SetValue<Boolean>(dpIsEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is visible.
        /// </summary>
        public Boolean IsVisible
        {
            get { return GetValue<Boolean>(dpIsVisible); }
            set { SetValue<Boolean>(dpIsVisible, value); }
        }

        /// <summary>
        /// Gets the element's absolute area after layout has been performed.
        /// </summary>
        protected internal Rectangle AbsoluteLayoutArea
        {
            get { return new Rectangle(absoluteX, absoluteY, absoluteWidth, absoluteHeight); }
            set
            {
                absoluteX = value.X;
                absoluteY = value.Y;
                absoluteWidth = value.Width;
                absoluteHeight = value.Height;
            }
        }

        /// <summary>
        /// Gets the absolute x-coordinate of the element after layout has been performed.
        /// </summary>
        protected internal Int32 AbsoluteX
        {
            get { return absoluteX; }
            internal set { absoluteX = value; }
        }

        /// <summary>
        /// Gets the absolute y-coordinate of the element after layout has been performed.
        /// </summary>
        protected internal Int32 AbsoluteY
        {
            get { return absoluteY; }
            internal set { absoluteY = value; }
        }

        /// <summary>
        /// Gets the absolute width of the element after layout has been performed.
        /// </summary>
        protected internal Int32 AbsoluteWidth
        {
            get { return absoluteWidth; }
            internal set { absoluteWidth = value; }
        }

        /// <summary>
        /// Gets the absolute height of the element after layout has been performed.
        /// </summary>
        protected internal Int32 AbsoluteHeight
        {
            get { return absoluteHeight; }
            internal set { absoluteHeight = value; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {

        }

        // Dependency properties.
        private static readonly DependencyProperty dpIsEnabled = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(UIElement));
        private static readonly DependencyProperty dpIsVisible = DependencyProperty.Register("IsVisible", typeof(Boolean), typeof(UIElement));

        // Property values.
        private readonly UltravioletContext uv;
        private readonly String id;
        private UIContainer container;
        private Int32 absoluteX;
        private Int32 absoluteY;
        private Int32 absoluteWidth;
        private Int32 absoluteHeight;
    }
}
