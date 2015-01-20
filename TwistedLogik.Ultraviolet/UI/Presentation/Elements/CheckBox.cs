using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a check box.
    /// </summary>
    [UIElement("CheckBox")]
    public class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes the <see cref="CheckBox"/> type.
        /// </summary>
        static CheckBox()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(CheckBox).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.CheckBox.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public CheckBox(UltravioletContext uv, String id)
            : base(uv, id)
        {
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

        /// <summary>
        /// Gets a <see cref="Visibility"/> value that describes the visibility state
        /// of the radio button's mark.
        /// </summary>
        private Visibility MarkVisibility
        {
            get { return Checked ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
