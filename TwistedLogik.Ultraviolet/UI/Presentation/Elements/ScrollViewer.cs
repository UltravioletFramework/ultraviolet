using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UIElement("ScrollViewer")]
    public class ScrollViewer : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="Canvas"/> type.
        /// </summary>
        static ScrollViewer()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(ScrollViewer).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ScrollViewer.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollViewer(UltravioletContext uv, String id)
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
    }
}
