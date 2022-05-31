using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.Button.xml")]
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
        /// <value><see langword="true"/> if this is the view's default <see cref="Button"/>; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// <para>When the value of the <see cref="IsDefault"/> property is <see langword="true"/>, the user
        /// can activate the button by pressing the ENTER key.</para>
        /// <dprop>
        ///     <dpropField><see cref="IsDefaultProperty"/></dpropField>
        ///     <dpropStylingName>default</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDefault
        {
            get { return GetValue<Boolean>(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is the cancel button for the current view.
        /// </summary>
        /// <value><see langword="true"/> if this is the view's cancel button; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// <para>When the value of the <see cref="IsCancel"/> property is <see langword="true"/>, the user
        /// can activate the button by pressing the ESC key.</para>
        /// <dprop>
        ///     <dpropField><see cref="IsCancelProperty"/></dpropField>
        ///     <dpropStylingName>cancel</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsCancel
        {
            get { return GetValue<Boolean>(IsCancelProperty); }
            set { SetValue(IsCancelProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the button is activated when the user presses the Enter key.
        /// </summary>
        /// <value><see langword="true"/> if the button is activated when the user presses ENTER;
        /// otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsDefaultedProperty"/></dpropField>
        ///     <dpropStylingName>defaulted</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDefaulted
        {
            get { return GetValue<Boolean>(IsDefaultedProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="IsDefault"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDefault"/> dependency property.</value>
        public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsDefaultChanged));

        /// <summary>
        /// Identifies the <see cref="IsCancel"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsCancel"/> dependency property.</value>
        public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsCancelChanged));

        /// <summary>
        /// The private access key for the <see cref="IsDefault"/> read-only dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDefaulted"/> dependency property.</value>
        private static readonly DependencyPropertyKey IsDefaultedPropertyKey = DependencyProperty.RegisterReadOnly("IsDefaulted", typeof(Boolean), typeof(Button),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsDefaulted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDefaultedProperty = IsDefaultedPropertyKey.DependencyProperty;

        /// <summary>
        /// Called when the button is activated because it is either the default or cancel button for its view.
        /// </summary>
        internal void HandleDefaultOrCancelActivated()
        {
            OnClick();
        }

        /// <summary>
        /// Updates the value of the <see cref="IsDefaulted"/> property for this button.
        /// </summary>
        internal void UpdateIsDefaulted()
        {
            var defaulted = false;
            var focused = (View == null) ? null : Keyboard.GetFocusedElement(View);
            if (IsDefault && IsEnabled && Visibility == Visibility.Visible && focused != null)
            {
                var focusedElement = focused as DependencyObject;
                if (focusedElement == null || !focusedElement.GetValue<Boolean>(KeyboardNavigation.AcceptsReturnProperty))
                {
                    defaulted = true;
                }
            }
            SetValue(IsDefaultedPropertyKey, defaulted);
        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            if (oldView != null)
            {
                if (IsDefault)
                    oldView.UnregisterDefaultButton(this);

                if (IsCancel)
                    oldView.UnregisterCancelButton(this);
            }

            if (newView != null)
            {
                if (IsDefault)
                    newView.RegisterDefaultButton(this);

                if (IsCancel)
                    newView.RegisterCancelButton(this);
            }

            UpdateIsDefaulted();

            base.OnViewChanged(oldView, newView);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsDefault"/> dependency property changes.
        /// </summary>
        private static void HandleIsDefaultChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var button = (Button)dobj;
            if (button.View != null)
            {
                if (newValue)
                {
                    button.View.RegisterDefaultButton(button);
                }
                else
                {
                    button.View.UnregisterDefaultButton(button);
                    button.SetValue(IsDefaultedPropertyKey, false);
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsCancel"/> dependency property changes.
        /// </summary>
        private static void HandleIsCancelChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var button = (Button)dobj;
            if (button.View != null)
            {
                if (newValue)
                {
                    button.View.RegisterCancelButton(button);
                }
                else
                {
                    button.View.UnregisterCancelButton(button);
                }
            }
        }        
    }
}
