using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for loading UI elements from UVML.
    /// </summary>
    internal static partial class UvmlLoader
    {
        /// <summary>
        /// Loads an instance of the <see cref="PresentationFoundationView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the panel.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view.</param>
        /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <c>null</c> to skip view model creation.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML element.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the component template of the specified control.
        /// </summary>
        /// <param name="control">The instance of <see cref="Control"/> for which to load a component root.</param>
        /// <param name="template">The component template that specifies the control's component layout.</param>
        public static void LoadComponentTemplate(Control control, XDocument template)
        {
            throw new NotImplementedException();
        }        
    }
}
