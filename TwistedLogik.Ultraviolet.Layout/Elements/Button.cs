using System;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("Button")]
    [DefaultProperty("Text")]
    public class Button : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public Button(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the button's text.
        /// </summary>
        public String Text
        {
            get { return GetValue<String>(dpText); }
            set { SetValue<String>(dpText, value); }
        }

        // Dependency properties.
        private static readonly DependencyProperty dpText = DependencyProperty.Register("Text", typeof(String), typeof(Button));
    }
}
