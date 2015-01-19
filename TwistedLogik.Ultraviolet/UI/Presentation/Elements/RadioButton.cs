using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UIElement("RadioButton")]
    public class RadioButton : ButtonBase
    {
        static RadioButton()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(RadioButton).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.RadioButton.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public RadioButton(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot(ComponentTemplate);
        }

        public static XDocument ComponentTemplate
        {
            get;
            set;
        }
    }
}
