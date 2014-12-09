using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("Button")]
    public class Button : TextualElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Button(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets a value indicating whether the button is in the "depressed" state.
        /// </summary>
        public Boolean Depressed
        {
            get { return depressed; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in both the "hovering" and "depressed" states.
        /// </summary>
        public Boolean HoveringDepressed
        {
            get { return Hovering && Depressed; }
        }

        /// <summary>
        /// Gets or sets the element's font color while in the "depressed" state.
        /// </summary>
        public Color? FontColorDepressed
        {
            get { return GetValue<Color?>(dpFontColorDepressed); }
            set { SetValue<Color?>(dpFontColorDepressed, value); }
        }

        /// <summary>
        /// Gets or sets the element's font color while in the "hover-depressed" state.
        /// </summary>
        public Color? FontColorHoverDepressed
        {
            get { return GetValue<Color?>(dpFontColorHoverDepressed); }
            set { SetValue<Color?>(dpFontColorHoverDepressed, value); }
        }

        /// <summary>
        /// Gets or sets the element's background color while in the "depressed" state.
        /// </summary>
        public Color? BackgroundColorDepressed
        {
            get { return GetValue<Color?>(dpBackgroundColorDepressed); }
            set { SetValue<Color?>(dpBackgroundColorDepressed, value); }
        }

        /// <summary>
        /// Gets or sets the element's background color while in the "hover-depressed" state.
        /// </summary>
        public Color? BackgroundColorHoverDepressed
        {
            get { return GetValue<Color?>(dpBackgroundColorHoverDepressed); }
            set { SetValue<Color?>(dpBackgroundColorHoverDepressed, value); }
        }

        /// <summary>
        /// Gets or sets the element's background image while in the "depressed" state.
        /// </summary>
        public SourcedRef<StretchableImage9>? BackgroundImageDepressed
        {
            get { return GetValue<SourcedRef<StretchableImage9>?>(dpBackgroundImageDepressed); }
            set { SetValue<SourcedRef<StretchableImage9>?>(dpBackgroundImageDepressed, value); }
        }

        /// <summary>
        /// Gets or sets the element's background image while in the "hover-depressed" state.
        /// </summary>
        public SourcedRef<StretchableImage9>? BackgroundImageHoverDepressed
        {
            get { return GetValue<SourcedRef<StretchableImage9>?>(dpBackgroundImageHoverDepressed); }
            set { SetValue<SourcedRef<StretchableImage9>?>(dpBackgroundImageHoverDepressed, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FontColorDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler FontColorDepressedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FontColorHoverDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler FontColorHoverDepressedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColorDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundColorDepressedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColorHoverDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundColorHoverDepressedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImageDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundImageDepressedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImageHoverDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundImageHoverDepressedChanged;

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event UIElementEventHandler Click;

        /// <inheritdoc/>
        protected internal override void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                depressed = true;
            }
            base.OnMouseButtonPressed(device, button);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (depressed)
                {
                    OnClick();
                }
                depressed = false;
            }
            base.OnMouseButtonReleased(device, button);
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawText(spriteBatch);

            base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnReloadingContent()
        {
            base.OnReloadingContent();

            ReloadBackgroundImageDepressed();
            ReloadBackgroundImageHoverDepressed();
        }

        /// <inheritdoc/>
        protected override StretchableImage9 GetCurrentBackgroundImage()
        {
            if (Depressed)
            {
                if (Hovering)
                {
                    return BackgroundImageHoverDepressed ?? BackgroundImageDepressed ?? BackgroundImage;
                }
                return BackgroundImageDepressed ?? BackgroundImage;
            }
            return base.GetCurrentBackgroundImage();
        }

        /// <inheritdoc/>
        protected override Color GetCurrentBackgroundColor()
        {
            if (Depressed)
            {
                if (Hovering)
                {
                    return BackgroundColorHoverDepressed ?? BackgroundColorDepressed ?? BackgroundColor;
                }
                return BackgroundColorDepressed ?? BackgroundColor;
            }
            return base.GetCurrentBackgroundColor();
        }

        /// <inheritdoc/>
        protected override Color GetCurrentFontColor()
        {
            if (Depressed)
            {
                if (Hovering)
                {
                    return FontColorHoverDepressed ?? FontColorDepressed ?? FontColor;
                }
                return FontColorDepressed ?? FontColor;
            }
            return base.GetCurrentFontColor();
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick()
        {
            var temp = Click;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FontColorDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnFontColorDepressedChanged()
        {
            var temp = FontColorDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FontColorHoverDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnFontColorHoverDepressedChanged()
        {
            var temp = FontColorHoverDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundColorDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundColorDepressedChanged()
        {
            var temp = BackgroundColorDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundColorHoverDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundColorHoverDepressedChanged()
        {
            var temp = BackgroundColorHoverDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundImageDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundImageDepressedChanged()
        {
            var temp = BackgroundImageDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundImageHoverDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundImageHoverDepressedChanged()
        {
            var temp = BackgroundImageHoverDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Reloads the element's background image for the "depressed" state.
        /// </summary>
        protected void ReloadBackgroundImageDepressed()
        {
            if (BackgroundImageDepressed != null)
            {
                LoadContent(BackgroundImageDepressed.Value);
            }
        }

        /// <summary>
        /// Reloads the element's background image for the "hover-depressed" state.
        /// </summary>
        protected void ReloadBackgroundImageHoverDepressed()
        {
            if (BackgroundImageHoverDepressed != null)
            {
                LoadContent(BackgroundImageHoverDepressed.Value);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FontColorDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFontColorDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.OnFontColorDepressedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FontColorHoverDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFontColorHoverDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.OnFontColorHoverDepressedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColorDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundColorDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.OnBackgroundColorDepressedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColorHoverDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundColorHoverDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.OnBackgroundColorHoverDepressedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImageDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundImageDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.ReloadBackgroundImageDepressed();
            element.OnBackgroundImageDepressedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImageHoverDepressed"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundImageHoverDepressedChanged(DependencyObject dobj)
        {
            var element = (Button)dobj;
            element.ReloadBackgroundImageHoverDepressed();
            element.OnBackgroundImageHoverDepressedChanged();
        }

        // Dependency properties.
        [Styled("font-color", "depressed")]
        private static readonly DependencyProperty dpFontColorDepressed = DependencyProperty.Register("FontColorDepressed", typeof(Color?), typeof(Button),
            new DependencyPropertyMetadata(HandleFontColorDepressedChanged, null, DependencyPropertyOptions.Inherited));
        [Styled("font-color", "hover-depressed")]
        private static readonly DependencyProperty dpFontColorHoverDepressed = DependencyProperty.Register("FontColorHoverDepressed", typeof(Color?), typeof(Button),
            new DependencyPropertyMetadata(HandleFontColorHoverDepressedChanged, null, DependencyPropertyOptions.Inherited));

        [Styled("background-color", "depressed")]
        private static readonly DependencyProperty dpBackgroundColorDepressed = DependencyProperty.Register("BackgroundColorDepressed", typeof(Color?), typeof(Button),
            new DependencyPropertyMetadata(HandleBackgroundColorDepressedChanged, null, DependencyPropertyOptions.None));
        [Styled("background-color", "hover-depressed")]
        private static readonly DependencyProperty dpBackgroundColorHoverDepressed = DependencyProperty.Register("BackgroundColorHoverDepressed", typeof(Color?), typeof(Button),
            new DependencyPropertyMetadata(HandleBackgroundColorHoverDepressedChanged, null, DependencyPropertyOptions.None));
        [Styled("background-image", "depressed")]
        private static readonly DependencyProperty dpBackgroundImageDepressed = DependencyProperty.Register("BackgroundImageDepressed", typeof(SourcedRef<StretchableImage9>?), typeof(Button),
            new DependencyPropertyMetadata(HandleBackgroundImageDepressedChanged, null, DependencyPropertyOptions.None));
        [Styled("background-image", "hover-depressed")]
        private static readonly DependencyProperty dpBackgroundImageHoverDepressed = DependencyProperty.Register("BackgroundImageHoverDepressed", typeof(SourcedRef<StretchableImage9>?), typeof(Button),
            new DependencyPropertyMetadata(HandleBackgroundImageHoverDepressedChanged, null, DependencyPropertyOptions.None));

        // Property values.
        private Boolean depressed;
    }
}
