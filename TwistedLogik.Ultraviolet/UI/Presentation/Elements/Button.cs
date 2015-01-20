using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("Button")]
    public class Button : ButtonBase
    {
        /// <summary>
        /// Initializes the <see cref="Button"/> type.
        /// </summary>
        static Button()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(Button).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.Button.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Button(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            SetDefaultValue<VerticalAlignment>(VerticalContentAlignmentProperty, VerticalAlignment.Center);

            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void OnButtonPressed()
        {
            View.CaptureMouse(this);
            base.OnButtonPressed();
        }

        /// <inheritdoc/>
        protected override void OnButtonReleased()
        {
            View.ReleaseMouse(this);
            base.OnButtonReleased();
        }
    }
}
