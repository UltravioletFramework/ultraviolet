using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for buttons.
    /// </summary>
    [UvmlKnownType]
    public abstract class ButtonBase : ContentControl, ICommandSource
    {
        /// <summary>
        /// Initializes the <see cref="ButtonBase"/> type.
        /// </summary>
        static ButtonBase()
        {
            IsEnabledProperty.OverrideMetadata(typeof(ButtonBase), new PropertyMetadata<Boolean>(HandleIsEnabledChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ButtonBase(UltravioletContext uv, String name)
            : base(uv, name)
        {
            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <inheritdoc/>
        public IInputElement CommandTarget
        {
            get { return GetValue<IInputElement>(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <inheritdoc/>
        public ICommand Command
        {
            get { return GetValue<ICommand>(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <inheritdoc/>
        public Object CommandParameter
        {
            get { return GetValue<Object>(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the button is being pressed.
        /// </summary>
        /// <value><see langword="true"/> if the button is being pressed; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsPressedProperty"/></dpropField>
        ///     <dpropStylingName>pressed</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsPressed
        {
            get { return GetValue<Boolean>(IsPressedProperty); }
            protected set { SetValue<Boolean>(IsPressedPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the button's click mode.
        /// </summary>
        /// <value>A <see cref="ClickMode"/> value specifying when the button's 
        /// <see cref="Click"/> event occurs.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ClickModeProperty"/></dpropField>
        ///     <dpropStylingName>click-mode</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ClickMode ClickMode
        {
            get { return GetValue<ClickMode>(ClickModeProperty); }
            set { SetValue<ClickMode>(ClickModeProperty, value); }
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ClickEvent"/></revtField>
        ///     <revtStylingName>click</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// Occurs when the button is clicked as a result of user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ClickByUserEvent"/></revtField>
        ///     <revtStylingName>click-by-user</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler ClickByUser
        {
            add { AddHandler(ClickByUserEvent, value); }
            remove { RemoveHandler(ClickByUserEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CommandTarget"/> dependency property.</value>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ButtonBase),
            new PropertyMetadata<IInputElement>(null));

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Command"/> dependency property.</value>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonBase),
            new PropertyMetadata<ICommand>(null, HandleCommandChanged));

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CommandParameter"/> dependency property.</value>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(Object), typeof(ButtonBase),
            new PropertyMetadata<Object>(null));

        /// <summary>
        /// The private access key for the <see cref="IsPressed"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(Boolean), typeof(ButtonBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleIsPressedChanged));

        /// <summary>
        /// Identifies the <see cref="IsPressed"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsPressed"/> dependency property.</value>
        public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="ClickMode"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ClickMode"/> dependency property.</value>
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase),
            new PropertyMetadata<ClickMode>(PresentationBoxedValues.ClickMode.Release));

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Click"/> event.</value>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ButtonBase));

        /// <summary>
        /// Identifies the <see cref="ClickByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="ClickByUser"/> event.</value>
        public static readonly RoutedEvent ClickByUserEvent = EventManager.RegisterRoutedEvent("ClickByUser", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ButtonBase));
        
        /// <inheritdoc/>
        protected override Boolean IsEnabledCore => base.IsEnabledCore && commandCanExecute;

        /// <inheritdoc/>
        protected internal override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            if (IsMouseCaptured && Mouse.PrimaryDevice.IsButtonDown(MouseButton.Left))
            {
                var position = Mouse.GetPosition(this);
                IsPressed = Bounds.Contains(position);
            }
            base.OnRenderSizeChanged(info);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (View != null && !View.IsInputEnabledAndAllowed)
            {
                IsPressed = false;
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            UpdateCanExecute();
            base.OnViewChanged(oldView, newView);
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateCommonState();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(RoutedEventData data)
        {
            IsPressed = false;
            base.OnLostMouseCapture(data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (IsPressed)
            {
                IsPressed = false;
            }
            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (ClickMode != ClickMode.Hover)
            {
                if (IsMouseCaptured && device.IsButtonDown(MouseButton.Left))
                {                    
                    var position = Mouse.GetPosition(this);
                    IsPressed = Bounds.Contains(position);
                }
            }
            base.OnMouseMove(device, x, y, dx, dy, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                HandleReleased();
                data.Handled = true;
            }
            base.OnMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, RoutedEventData data)
        {
            if (ClickMode == ClickMode.Hover)
            {
                OnClick();
                OnClickByUser();
            }
            base.OnMouseEnter(device, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchDown(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id))
                {
                    HandlePressed();
                    data.Handled = true;
                }
            }
            base.OnTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchUp(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id))
                {
                    IsPressed = false;
                    data.Handled = true;
                }
            }
            base.OnTouchUp(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchTap(TouchDevice device, Int64 id, Double x, Double y, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id))
                {
                    HandleReleased(checkMousePosition: false);
                    data.Handled = true;
                }
            }
            base.OnTouchTap(device, id, x, y, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchEnter(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id) && ClickMode == ClickMode.Hover)
                {
                    OnClick();
                    OnClickByUser();
                }
            }
            base.OnTouchEnter(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchLeave(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id))
                {
                    IsPressed = false;
                }
            }
            base.OnTouchLeave(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            if (key == Key.Return || key == Key.Space)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyboardDevice device, Key key, RoutedEventData data)
        {
            if (key == Key.Return || key == Key.Space)
            {
                HandleReleased(false);
                data.Handled = true;
            }
            base.OnKeyUp(device, key, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button && !repeat)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnGamePadButtonDown(device, button, repeat, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonUp(GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                HandleReleased(false);
                data.Handled = true;
            }
            base.OnGamePadButtonUp(device, button, data);
        }
        
        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ClickEvent);
            evtDelegate(this, evtData);

            CommandManager.ExecuteSource(View, this);
        }

        /// <summary>
        /// Raises the <see cref="ClickByUser"/> event.
        /// </summary>
        protected virtual void OnClickByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ClickByUserEvent);
            evtDelegate(this, evtData);
        }
        
        /// <summary>
        /// Called when the value of the of <see cref="IsPressed"/> property changes.
        /// </summary>
        protected virtual void OnIsPressedChanged()
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Command"/> dependency property changes.
        /// </summary>
        private static void HandleCommandChanged(DependencyObject dobj, ICommand oldValue, ICommand newValue)
        {
            var button = (ButtonBase)dobj;

            if (oldValue != null)
                button.DetachCanExecuteChanged(oldValue);

            if (newValue != null)
                button.AttachCanExecuteChanged(newValue);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UIElement.IsEnabledChanged"/> dependency property changes.
        /// </summary>
        private static void HandleIsEnabledChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var buttonBase = (ButtonBase)dobj;
            buttonBase.UpdateCommonState();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsPressed"/> dependency property changes.
        /// </summary>
        private static void HandleIsPressedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var buttonBase = (ButtonBase)dobj;
            buttonBase.UpdateCommonState();
            buttonBase.OnIsPressedChanged();
        }

        /// <summary>
        /// Handles the <see cref="ICommand.CanExecuteChanged"/> event for the button's command.
        /// </summary>
        private void OnCanExecuteChanged(Object sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        /// <summary>
        /// Modifies state and raises events relating to the button being pressed.
        /// </summary>
        private void HandlePressed()
        {
            Focus();
            CaptureMouse();

            IsPressed = true;

            if (ClickMode == ClickMode.Press)
            {
                OnClick();
                OnClickByUser();
            }
        }

        /// <summary>
        /// Modifies state and raises events relating to the button being released.
        /// </summary>
        /// <param name="checkMousePosition">A value indicating whether to confirm that the mouse is inside of the button before calling <see cref="OnClick()"/>.</param>
        private void HandleReleased(Boolean checkMousePosition = true)
        {
            var clicked = IsPressed;

            ReleaseMouseCapture();

            IsPressed = false;

            if (clicked && ClickMode == ClickMode.Release)
            {
                if (checkMousePosition)
                {
                    var position = Mouse.GetPosition(this);
                    if (Bounds.Contains(position))
                    {
                        OnClick();
                        OnClickByUser();
                    }
                }
                else
                {
                    OnClick();
                    OnClickByUser();
                }
            }
        }

        /// <summary>
        /// Transitions the button into the appropriate common state.
        /// </summary>
        private void UpdateCommonState()
        {
            if (IsEnabled)
            {
                if (IsPressed)
                {
                    VisualStateGroups.GoToState("common", "pressed");
                }
                else
                {
                    if (IsMouseOver)
                    {
                        VisualStateGroups.GoToState("common", "hover");
                    }
                    else
                    {
                        VisualStateGroups.GoToState("common", "normal");
                    }
                }
            }
            else
            {
                VisualStateGroups.GoToState("common", "disabled");
            }
        }

        /// <summary>
        /// Updates the button when its command's execution state changes.
        /// </summary>
        private void UpdateCanExecute()
        {
            if (View == null)
            {
                commandCanExecute = false;
            }
            else
            {
                commandCanExecute = (Command == null) || CommandManager.CheckCanExecuteSource(View, this);
            }
            CoerceValue(IsEnabledProperty);
            UpdateCommonState();
        }

        /// <summary>
        /// Attaches an event handler to the specified command's <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        private void AttachCanExecuteChanged(ICommand command)
        {
            CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
            UpdateCanExecute();
        }

        /// <summary>
        /// Detaches an event handler from the specified command's <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        private void DetachCanExecuteChanged(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
            UpdateCanExecute();
        }

        // State values.
        private Boolean commandCanExecute;
    }
}
