using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a button on a user interface which can be toggled between its states.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Primitives.Templates.ToggleButton.xml")]
    public class ToggleButton : ButtonBase
    {
        /// <summary>
        /// Initializes the <see cref="ToggleButton"/> type.
        /// </summary>
        static ToggleButton()
        {
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(ToggleButton), new PropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(ToggleButton), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ToggleButton(UltravioletContext uv, String name)
            : base(uv, name)
        {
            VisualStateGroups.Create("checkstate", new[] { "unchecked", "checked", "indeterminate" });
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is checked.
        /// </summary>
        /// <value><see langword="true"/> if the button is checked, <see langword="false"/> if the button is not
        /// checked, or <see langword="null"/> if the button is in an indeterminate state. The default value is
        /// <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsCheckedProperty"/></dpropField>
        ///		<dpropStylingName>checked</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean? IsChecked
        {
            get { return GetValue<Boolean?>(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control supports three states.
        /// </summary>
        /// <value><see langword="true"/> if the button has three states; otherwise, <see langword="false"/>. 
        /// The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsThreeStateProperty"/></dpropField>
        ///		<dpropStylingName>three-state</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsThreeState
        {
            get { return GetValue<Boolean>(IsThreeStateProperty); }
            set { SetValue(IsThreeStateProperty, value); }
        }
        
        /// <summary>
        /// Occurs when the toggle button is checked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="CheckedEvent"/></revtField>
        ///		<revtStylingName>checked</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button is checked as a result of user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="CheckedByUserEvent"/></revtField>
        ///		<revtStylingName>checked-by-user</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler CheckedByUser
        {
            add { AddHandler(CheckedByUserEvent, value); }
            remove { RemoveHandler(CheckedByUserEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button is unchecked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="UncheckedEvent"/></revtField>
        ///		<revtStylingName>unchecked</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button is unchecked as a result of user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="UncheckedByUserEvent"/></revtField>
        ///		<revtStylingName>unchecked-by-user</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler UncheckedByUser
        {
            add { AddHandler(UncheckedByUserEvent, value); }
            remove { RemoveHandler(UncheckedByUserEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button enters an indeterminate state.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="IndeterminateEvent"/></revtField>
        ///		<revtStylingName>indeterminate</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Indeterminate
        {
            add { AddHandler(IndeterminateEvent, value); }
            remove { RemoveHandler(IndeterminateEvent, value); }
        }

        /// <summary>
        /// Occurs when the toggle button enters an indeterminate state as a result of user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="IndeterminateByUserEvent"/></revtField>
        ///		<revtStylingName>indeterminate-by-user</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler IndeterminateByUser
        {
            add { AddHandler(IndeterminateByUserEvent, value); }
            remove { RemoveHandler(IndeterminateByUserEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsChecked"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsChecked"/> dependency property.</value>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(Boolean?), typeof(ToggleButton),
            new PropertyMetadata<Boolean?>(CommonBoxedValues.Boolean.False, HandleIsCheckedChanged));

        /// <summary>
        /// Identifies the <see cref="IsThreeState"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsThreeState"/> dependency property.</value>
        public static readonly DependencyProperty IsThreeStateProperty = DependencyProperty.Register("IsThreeState", typeof(Boolean), typeof(ToggleButton),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="Checked"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Checked"/> routed event.</value>
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="CheckedByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="CheckedByUser"/> routed event.</value>
        public static readonly RoutedEvent CheckedByUserEvent = EventManager.RegisterRoutedEvent("CheckedByUser", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="Unchecked"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Unchecked"/> routed event.</value>
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="UncheckedByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="UncheckedByUser"/> routed event.</value>
        public static readonly RoutedEvent UncheckedByUserEvent = EventManager.RegisterRoutedEvent("UncheckedByUser", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="Indeterminate"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Indeterminate"/> routed event.</value>
        public static readonly RoutedEvent IndeterminateEvent = EventManager.RegisterRoutedEvent("Indeterminate", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <summary>
        /// Identifies the <see cref="IndeterminateByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="IndeterminateByUser"/> routed event.</value>
        public static readonly RoutedEvent IndeterminateByUserEvent = EventManager.RegisterRoutedEvent("IndeterminateByUser", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ToggleButton));

        /// <inheritdoc/>
        protected override void OnClick()
        {
            OnToggle(true);
            base.OnClick();
        }

        /// <summary>
        /// Toggles the value of the <see cref="IsChecked"/> property.
        /// </summary>
        /// <param name="toggledByUser">A value indicating whether this state change was caused by a user interaction.</param>
        protected virtual void OnToggle(Boolean toggledByUser = false)
        {
            var isChecked = IsChecked;
            if (IsThreeState)
            {
                if (isChecked.HasValue)
                {
                    if (isChecked.Value)
                    {
                        IsChecked = null;
                        if (toggledByUser)
                        {
                            OnIndeterminateByUser();
                        }
                    }
                    else
                    {
                        IsChecked = true;
                        if (toggledByUser)
                        {
                            OnCheckedByUser();
                        }
                    }
                }
                else
                {
                    IsChecked = false;
                    if (toggledByUser)
                    {
                        OnUncheckedByUser();
                    }
                }
            }
            else
            {
                IsChecked = !isChecked.GetValueOrDefault();
                if (toggledByUser)
                {
                    if (IsChecked.GetValueOrDefault())
                    {
                        OnCheckedByUser();
                    }
                    else
                    {
                        OnUncheckedByUser();
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Checked"/> event.
        /// </summary>
        protected virtual void OnChecked()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(CheckedEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="CheckedByUser"/> event.
        /// </summary>
        protected virtual void OnCheckedByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(CheckedByUserEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="Unchecked"/> event.
        /// </summary>
        protected virtual void OnUnchecked()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(UncheckedEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="UncheckedByUser"/> event.
        /// </summary>
        protected virtual void OnUncheckedByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(UncheckedByUserEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="Indeterminate"/> event.
        /// </summary>
        protected virtual void OnIndeterminate()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(IndeterminateEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="IndeterminateByUser"/> event.
        /// </summary>
        protected virtual void OnIndeterminateByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(IndeterminateByUserEvent);
            evtDelegate(this, evtData);
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
