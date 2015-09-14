using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the root layout element for a <see cref="PresentationFoundationView"/>.
    /// </summary>
    public sealed class PresentationFoundationViewRoot : Decorator
    {
        /// <summary>
        /// Initializes the <see cref="PresentationFoundationViewRoot"/> type.
        /// </summary>
        static PresentationFoundationViewRoot()
        {
            FocusManager.IsFocusScopeProperty.OverrideMetadata(
                typeof(PresentationFoundationView), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationViewRoot"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public PresentationFoundationViewRoot(UltravioletContext uv, String name) 
            : base(uv, name)
        {

        }
    }
}
