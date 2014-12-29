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
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public ScrollViewer(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            if (ComponentTemplate != null)
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
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
