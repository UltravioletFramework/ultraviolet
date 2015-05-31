using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a button on a user interface which can be toggled between its states.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.ToggleButton.xml")]
    public class ToggleButton : ButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ToggleButton(UltravioletContext uv, String name)
            : base(uv, name)
        {
            VisualStateGroups.Create("checkstate", new[] { "unchecked", "checked", "indeterminate" });

            SetDefaultValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            SetDefaultValue<VerticalAlignment>(VerticalContentAlignmentProperty, VerticalAlignment.Center);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is checked.
        /// </summary>
        public Boolean? IsChecked
        {
            get { return GetValue<Boolean?>(IsCheckedProperty); }
            set { SetValue<Boolean?>(IsCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control supports three states.
        /// </summary>
        public Boolean IsThreeState
        {
            get { return GetValue<Boolean>(IsThreeStateProperty); }
            set { SetValue<Boolean>(IsThreeStateProperty, value); }
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
        /// Occures when the toggle button is neither checked nor unchecked.
        /// </summary>
        public event UpfRoutedEventHandler Indeterminate
        {
            add { AddHandler(IndeterminateEvent, value); }
            remove { RemoveHandler(IndeterminateEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsChecked"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'checked'.</remarks>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(Boolean?), typeof(ToggleButton),
            new PropertyMetadata<Boolean?>(CommonBoxedValues.Boolean.False, HandleIsCheckedChanged));

        /// <summary>
        /// Identifies the <see cref="IsThreeState"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'three-state'.</remarks>
        public static readonly DependencyProperty IsThreeStateProperty = DependencyProperty.Register("IsThreeState", typeof(Boolean), typeof(ToggleButton),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="Checked"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'checked'.</remarks>
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="Unchecked"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'unchecked'.</remarks>
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="Indeterminate"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'indeterminate'.</remarks>
        public static readonly RoutedEvent IndeterminateEvent = EventManager.RegisterRoutedEvent("Indeterminate", RoutingStrategy.Bubble,
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
            var isChecked = IsChecked;
            if (IsThreeState)
            {
                if (isChecked.HasValue)
                {
                    if (isChecked.Value)
                    {
                        IsChecked = null;
                    }
                    else
                    {
                        IsChecked = true;
                    }
                }
                else
                {
                    IsChecked = false;
                }
            }
            else
            {
                IsChecked = !isChecked.GetValueOrDefault();
            }
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
        /// Raises the <see cref="Indeterminate"/> event.
        /// </summary>
        protected virtual void OnIndeterminate()
        {
            var evtData     = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(IndeterminateEvent);
            evtDelegate(this, ref evtData);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> dependency property changes.
        /// </summary>
        private static void HandleIsCheckedChanged(DependencyObject dobj, Boolean? oldValue, Boolean? newValue)
        {
            var element = (ToggleButton)dobj;

            var isChecked = element.IsChecked;
            if (isChecked.HasValue)
            {
                if (isChecked.Value)
                {
                    element.OnChecked();
                }
                else
                {
                    element.OnUnchecked();
                }
            }
            else
            {
                element.OnIndeterminate();
            }
            element.UpdateCheckState();
        }

        /// <summary>
        /// Transitions the button into the appropriate check state.
        /// </summary>
        private void UpdateCheckState()
        {
            var isChecked = IsChecked;
            if (isChecked.HasValue)
            {
                if (isChecked.Value)
                {
                    VisualStateGroups.GoToState("checkstate", "checked");
                }
                else
                {
                    VisualStateGroups.GoToState("checkstate", "unchecked");
                }
            }
            else
            {
                VisualStateGroups.GoToState("checkstate", "indeterminate");
            }
        }
    }
}
