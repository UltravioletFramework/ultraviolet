using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives an axis changed event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was changed.</param>
    /// <param name="value">The game pad axis value after the change.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisChangedEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a axis press event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was pressed.</param>
    /// <param name="value">The game pad axis value.</param>
    /// <param name="repeat">A value indicating whether this is a repeated axis press.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisDownEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a axis release event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was released.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisUpEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a button press event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="button">The game pad button that was pressed.</param>
    /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadButtonDownEventHandler(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a button release event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="button">The game pad button that was released.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadButtonUpEventHandler(DependencyObject element, GamePadDevice device, GamePadButton button, ref RoutedEventData data);

    /// <summary>
    /// Represents the primary game pad device.
    /// </summary>
    public static class GamePad
    {
        /// <summary>
        /// Initializes the <see cref="GamePad"/> type.
        /// </summary>
        static GamePad()
        {
            UseAxisForDirectionalNavigation = true;

            DirectionalNavigationAxisX = GamePadAxis.LeftJoystickX;
            DirectionalNavigationAxisY = GamePadAxis.LeftJoystickY;

            ConfirmButton = GamePadButton.A;
            CancelButton = GamePadButton.B;

            TabButton = GamePadButton.RightShoulder;
            ShiftTabButton = GamePadButton.LeftShoulder;
            ControlTabButton = GamePadButton.None;

            AxisThreshold = 0.75f;
        }

        /// <summary>
        /// Adds a handler for the PreviewAxisChanged attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisChangedHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewAxisChangedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewAxisDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewAxisDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewAxisUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewAxisUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewButtonDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewButtonDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewButtonDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewButtonUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewButtonUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewButtonUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the AxisChanged attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisChangedHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, AxisChangedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the AxisDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, AxisDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the AxisUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, AxisUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the ButtonDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddButtonDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, ButtonDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the ButtonUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddButtonUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, ButtonUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewAxisChanged attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewAxisChangedHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewAxisChangedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewButtonDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewButtonDownHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewButtonDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewButtonUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewButtonUpHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewButtonUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the AxisChanged attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveAxisChangedHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, AxisChangedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the ButtonDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveButtonDownHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, ButtonDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the ButtonUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveButtonUpHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, ButtonUpEvent, handler);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use controller axes for directional navigation. If this property is
        /// set to <c>false</c>, then the directional pad will be used instead.
        /// </summary>
        public static Boolean UseAxisForDirectionalNavigation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the axis which is used to perform left/right directional navigation if <see cref="UseAxisForDirectionalNavigation"/> is <c>true</c>.
        /// </summary>
        public static GamePadAxis DirectionalNavigationAxisX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the axis which is used to perform up/down directional navigation if <see cref="UseAxisForDirectionalNavigation"/> is <c>true</c>.
        /// </summary>
        public static GamePadAxis DirectionalNavigationAxisY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the game pad button which confirms interface actions.
        /// </summary>
        public static GamePadButton ConfirmButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the game pad button which cancels interface actions.
        /// </summary>
        public static GamePadButton CancelButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the game pad button which performs the equivalent of pressing Tab.
        /// </summary>
        public static GamePadButton TabButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the game pad button which performs the equivalent of pressing Shift + Tab.
        /// </summary>
        public static GamePadButton ShiftTabButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the game pad button which performs the equivalent of pressing Control + Tab.
        /// </summary>
        public static GamePadButton ControlTabButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the magnitude that an axis much reach in order for it to be considered "pressed" for the purposes of directional navigation.
        /// </summary>
        public static Single AxisThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Identifies the PreviewAxisChanged routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-axis-changed.</remarks>
        public static readonly RoutedEvent PreviewAxisChangedEvent = EventManager.RegisterRoutedEvent("PreviewAxisChanged", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisChangedEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the PreviewAxisDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-axis-down.</remarks>
        public static readonly RoutedEvent PreviewAxisDownEvent = EventManager.RegisterRoutedEvent("PreviewAxisDown", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the PreviewAxisUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-axis-up.</remarks>
        public static readonly RoutedEvent PreviewAxisUpEvent = EventManager.RegisterRoutedEvent("PreviewAxisUp", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the PreviewButtonDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-button-down.</remarks>
        public static readonly RoutedEvent PreviewButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewButtonDown", RoutingStrategy.Tunnel,
            typeof(UpfGamePadButtonDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the PreviewButtonUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-button-up.</remarks>
        public static readonly RoutedEvent PreviewButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewButtonUp", RoutingStrategy.Tunnel,
            typeof(UpfGamePadButtonUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the AxisChanged routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is axis-changed.</remarks>
        public static readonly RoutedEvent AxisChangedEvent = EventManager.RegisterRoutedEvent("AxisChanged", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisChangedEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the AxisDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is -axis-down.</remarks>
        public static readonly RoutedEvent AxisDownEvent = EventManager.RegisterRoutedEvent("AxisDown", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the AxisUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is -axis-up.</remarks>
        public static readonly RoutedEvent AxisUpEvent = EventManager.RegisterRoutedEvent("AxisUp", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the ButtonDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is button-down.</remarks>
        public static readonly RoutedEvent ButtonDownEvent = EventManager.RegisterRoutedEvent("ButtonDown", RoutingStrategy.Bubble,
            typeof(UpfGamePadButtonDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the ButtonUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is button-up.</remarks>
        public static readonly RoutedEvent ButtonUpEvent = EventManager.RegisterRoutedEvent("ButtonUp", RoutingStrategy.Bubble,
            typeof(UpfGamePadButtonUpEventHandler), typeof(GamePad));
        
        /// <summary>
        /// Raises the PreviewAxisChanged attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisChanged(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisChangedEventHandler>(PreviewAxisChangedEvent);
            if (temp != null)
            {
                temp(element, device, axis, value, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewAxisDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisDown(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisDownEventHandler>(PreviewAxisDownEvent);
            if (temp != null)
            {
                temp(element, device, axis, value, repeat, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewAxisUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisUp(DependencyObject element, GamePadDevice device, GamePadAxis axis, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisUpEventHandler>(PreviewAxisUpEvent);
            if (temp != null)
            {
                temp(element, device, axis, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewButtonDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewButtonDown(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadButtonDownEventHandler>(PreviewButtonDownEvent);
            if (temp != null)
            {
                temp(element, device, button, repeat, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewButtonUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewButtonUp(DependencyObject element, GamePadDevice device, GamePadButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadButtonUpEventHandler>(PreviewButtonUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the AxisChanged attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisChanged(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisChangedEventHandler>(AxisChangedEvent);
            if (temp != null)
            {
                temp(element, device, axis, value, ref data);
            }
        }

        /// <summary>
        /// Raises the AxisDown attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisDown(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisDownEventHandler>(AxisDownEvent);
            if (temp != null)
            {
                temp(element, device, axis, value, repeat, ref data);
            }
        }

        /// <summary>
        /// Raises the AxisUp attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisUp(DependencyObject element, GamePadDevice device, GamePadAxis axis, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadAxisUpEventHandler>(AxisUpEvent);
            if (temp != null)
            {
                temp(element, device, axis, ref data);
            }
        }

        /// <summary>
        /// Raises the ButtonDown attached event for the specified element.
        /// </summary>
        internal static void RaiseButtonDown(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadButtonDownEventHandler>(ButtonDownEvent);
            if (temp != null)
            {
                temp(element, device, button, repeat, ref data);
            }
        }

        /// <summary>
        /// Raises the ButtonUp attached event for the specified element.
        /// </summary>
        internal static void RaiseButtonUp(DependencyObject element, GamePadDevice device, GamePadButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGamePadButtonUpEventHandler>(ButtonUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }
    }
}
