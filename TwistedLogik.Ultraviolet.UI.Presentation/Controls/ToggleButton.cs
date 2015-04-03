using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface which can be toggled between its states.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ToggleButton.xml")]
    public class ToggleButton : ButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ToggleButton(UltravioletContext uv, String id)
            : base(uv, id)
        {
            VisualStateGroups.Create("checkstate", new[] { "unchecked", "checked" });

            SetDefaultValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            SetDefaultValue<VerticalAlignment>(VerticalContentAlignmentProperty, VerticalAlignment.Center);
        }

        /// <summary>
        /// Gets a value indicating whether the button is checked.
        /// </summary>
        public Boolean IsChecked
        {
            get { return GetValue<Boolean>(IsCheckedProperty); }
            set { SetValue<Boolean>(IsCheckedProperty, value); }
        }

        /// <summary>
        /// Occurs when the toggle button is checked.
        /// </summary>
        public event UpfRoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button is unchecked.
        /// </summary>
        public event UpfRoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }

        /// <summary>
        /// Identifies the Checked dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(Boolean), typeof(ToggleButton),
            new PropertyMetadata(CommonBoxedValues.Boolean.False, HandleIsCheckedChanged));

        /// <summary>
        /// Identifies the <see cref="Checked"/> routed event.
        /// </summary>
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="Unchecked"/> routed event.
        /// </summary>
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <inheritdoc/>
        protected override void OnClick()
        {
            OnToggle();
            base.OnClick();
        }

        /// <summary>
        /// Toggles the value of the <see cref="IsChecked"/> property.
        /// </summary>
        protected virtual void OnToggle()
        {
            IsChecked = !IsChecked;
        }

        /// <summary>
        /// Raises the <see cref="Checked"/> event.
        /// </summary>
        protected virtual void OnChecked()
        {
            var evtData     = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(CheckedEvent);
            evtDelegate(this, ref evtData);
        }

        /// <summary>
        /// Raises the <see cref="Unchecked"/> event.
        /// </summary>
        protected virtual void OnUnchecked()
        {
            var evtData     = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(UncheckedEvent);
            evtDelegate(this, ref evtData);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleIsCheckedChanged(DependencyObject dobj)
        {
            var element = (ToggleButton)dobj;

            if (element.IsChecked)
            {
                element.OnChecked();
            }
            else
            {
                element.OnUnchecked();
            }

            element.UpdateCheckState();
        }

        /// <summary>
        /// Transitions the button into the appropriate check state.
        /// </summary>
        private void UpdateCheckState()
        {
            if (IsChecked)
            {
                VisualStateGroups.GoToState("checkstate", "checked");
            }
            else
            {
                VisualStateGroups.GoToState("checkstate", "unchecked");
            }
        }
    }
}
