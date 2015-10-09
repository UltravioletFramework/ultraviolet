using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Button.xml")]
    public class Button : ButtonBase
    {
        /// <summary>
        /// Initializes the button type.
        /// </summary>
        static Button()
        {
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(Button), new PropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(Button), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Button(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether this is the default button for the current view.
        /// </summary>
        public Boolean IsDefault
        {
            get { return GetValue<Boolean>(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is the cancel button for the current view.
        /// </summary>
        public Boolean IsCancel
        {
            get { return GetValue<Boolean>(IsCancelProperty); }
            set { SetValue(IsCancelProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the button is activated when the user presses the Enter key.
        /// </summary>
        public Boolean IsDefaulted
        {
            get { return GetValue<Boolean>(IsDefaultedProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="IsDefault"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsDefaultChanged));

        /// <summary>
        /// Identifies the <see cref="IsCancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsCancelChanged));

        /// <summary>
        /// The private access key for the <see cref="IsDefault"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsDefaultedPropertyKey = DependencyProperty.RegisterReadOnly("IsDefaulted", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsDefaultedChanged));

        /// <summary>
        /// Identifies the <see cref="IsDefaulted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDefaultedProperty = IsDefaultedPropertyKey.DependencyProperty;

        /// <summary>
        /// Occurs when the value of the <see cref="IsDefault"/> dependency property changes.
        /// </summary>
        private static void HandleIsDefaultChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            // TODO
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsCancel"/> dependency property changes.
        /// </summary>
        private static void HandleIsCancelChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            // TODO
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsDefaulted"/> dependency property changes.
        /// </summary>
        private static void HandleIsDefaultedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            // TODO
        }
    }
}
