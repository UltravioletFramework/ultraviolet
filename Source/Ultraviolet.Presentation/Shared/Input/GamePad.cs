using System;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives an axis changed event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was changed.</param>
    /// <param name="value">The game pad axis value after the change.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisChangedEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a axis press event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was pressed.</param>
    /// <param name="value">The game pad axis value.</param>
    /// <param name="repeat">A value indicating whether this is a repeated axis press.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisDownEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a axis release event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="axis">The game pad axis that was released.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadAxisUpEventHandler(DependencyObject element, GamePadDevice device, GamePadAxis axis, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a button press event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="button">The game pad button that was pressed.</param>
    /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadButtonDownEventHandler(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives a button release event from a game pad device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The game pad device.</param>
    /// <param name="button">The game pad button that was released.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfGamePadButtonUpEventHandler(DependencyObject element, GamePadDevice device, GamePadButton button, RoutedEventData data);

    /// <summary>
    /// Represents the primary game pad device.
    /// </summary>
    [UvmlKnownType]
    public static partial class GamePad
    {
        /// <summary>
        /// Initializes the <see cref="GamePad"/> type.
        /// </summary>
        static GamePad()
        {
            ConfirmButton = GamePadButton.A;
            CancelButton = GamePadButton.B;

            TabButton = GamePadButton.RightShoulder;
            ShiftTabButton = GamePadButton.LeftShoulder;
            ControlTabButton = GamePadButton.None;
        }

        /// <summary>
        /// Gets the game pad device that is associated with the specified player.
        /// </summary>
        /// <param name="playerIndex">The index of the player for which to retrieve a game pad device.</param>
        /// <returns>The game pad device that is associated with the specified player, or <see langword="null"/>
        /// if no such game pad is connected.</returns>
        public static GamePadDevice GetDeviceForPlayer(Int32 playerIndex)
        {
            return gamePadState.Value.GetGamePadForPlayer(playerIndex);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisChangedHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewAxisChangedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewAxisDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewAxisUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewButtonDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewButtonDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewButtonUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewButtonUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisChangedHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, AxisChangedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, AxisDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, AxisUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddButtonDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, ButtonDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddButtonUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, ButtonUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewAxisChangedHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewAxisChangedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewAxisDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewAxisUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonDown"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewButtonDownHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewButtonDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewButtonUpHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewButtonUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveAxisChangedHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, AxisChangedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveAxisDownHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, AxisDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveAxisUpHandler(DependencyObject element, UpfGamePadAxisChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, AxisDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveButtonDownHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, ButtonDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveButtonUpHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, ButtonUpEvent, handler);
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
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the value of a game pad axis changes while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewAxisChangedEvent"/></revtField>
        ///     <revtStylingName>preview-axis-changed</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewAxisChangedEvent = EventManager.RegisterRoutedEvent("PreviewAxisChanged", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisChangedEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad axis moves crosses over the <see cref="GamePadDevice.AxisDownThreshold"/> value into the "down" state
        /// while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewAxisDownEvent"/></revtField>
        ///     <revtStylingName>preview-axis-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewAxisDownEvent = EventManager.RegisterRoutedEvent("PreviewAxisDown", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad axis crosses over the <see cref="GamePadDevice.AxisDownThreshold"/> value into the "up" state
        /// while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewAxisUpEvent"/></revtField>
        ///     <revtStylingName>preview-axis-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewAxisUpEvent = EventManager.RegisterRoutedEvent("PreviewAxisUp", RoutingStrategy.Tunnel,
            typeof(UpfGamePadAxisUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonDown"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonDown"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad button enters the "down" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewButtonDownEvent"/></revtField>
        ///     <revtStylingName>preview-button-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadButtonDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewButtonDown", RoutingStrategy.Tunnel,
            typeof(UpfGamePadButtonDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad button enters the "up" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewButtonUpEvent"/></revtField>
        ///     <revtStylingName>preview-button-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadButtonUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewButtonUp", RoutingStrategy.Tunnel,
            typeof(UpfGamePadButtonUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the value of a game pad axis changes while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="AxisChangedEvent"/></revtField>
        ///     <revtStylingName>axis-changed</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent AxisChangedEvent = EventManager.RegisterRoutedEvent("AxisChanged", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisChangedEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad axis moves crosses over the <see cref="GamePadDevice.AxisDownThreshold"/> value into the "down" state
        /// while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="AxisDownEvent"/></revtField>
        ///     <revtStylingName>axis-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent AxisDownEvent = EventManager.RegisterRoutedEvent("AxisDown", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad axis moves crosses over the <see cref="GamePadDevice.AxisDownThreshold"/> value into the "down" state
        /// while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="AxisUpEvent"/></revtField>
        ///     <revtStylingName>axis-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadAxisUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent AxisUpEvent = EventManager.RegisterRoutedEvent("AxisUp", RoutingStrategy.Bubble,
            typeof(UpfGamePadAxisUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad button enters the "down" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ButtonDownEvent"/></revtField>
        ///     <revtStylingName>button-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadButtonDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent ButtonDownEvent = EventManager.RegisterRoutedEvent("ButtonDown", RoutingStrategy.Bubble,
            typeof(UpfGamePadButtonDownEventHandler), typeof(GamePad));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/> attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/> attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a game pad button enters the "up" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ButtonUpEvent"/></revtField>
        ///     <revtStylingName>button-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGamePadButtonUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent ButtonUpEvent = EventManager.RegisterRoutedEvent("ButtonUp", RoutingStrategy.Bubble,
            typeof(UpfGamePadButtonUpEventHandler), typeof(GamePad));

        /// <summary>
        /// Gets the primary game pad input device.
        /// </summary>
        public static GamePadDevice PrimaryDevice => gamePadState.Value.PrimaryDevice;

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisChanged"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisChanged(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisChangedEventHandler>(PreviewAxisChangedEvent);
            evt?.Invoke(element, device, axis, value, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisDown(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisDownEventHandler>(PreviewAxisDownEvent);
            evt?.Invoke(element, device, axis, value, repeat, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisUp"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewAxisUp(DependencyObject element, GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisUpEventHandler>(PreviewAxisUpEvent);
            evt?.Invoke(element, device, axis, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewAxisDown"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewButtonDown(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadButtonDownEventHandler>(PreviewButtonDownEvent);
            evt?.Invoke(element, device, button, repeat, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.PreviewButtonUp"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewButtonUp(DependencyObject element, GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadButtonUpEventHandler>(PreviewButtonUpEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisChanged"/> attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisChanged(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisChangedEventHandler>(AxisChangedEvent);
            evt?.Invoke(element, device, axis, value, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisDown"/> attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisDown(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisDownEventHandler>(AxisDownEvent);
            evt?.Invoke(element, device, axis, value, repeat, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.AxisUp"/> attached event for the specified element.
        /// </summary>
        internal static void RaiseAxisUp(DependencyObject element, GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadAxisUpEventHandler>(AxisUpEvent);
            evt?.Invoke(element, device, axis, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonDown"/> attached event for the specified element.
        /// </summary>
        internal static void RaiseButtonDown(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadButtonDownEventHandler>(ButtonDownEvent);
            evt?.Invoke(element, device, button, repeat, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.GamePad.ButtonUp"/> attached event for the specified element.
        /// </summary>
        internal static void RaiseButtonUp(DependencyObject element, GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGamePadButtonUpEventHandler>(ButtonUpEvent);
            evt?.Invoke(element, device, button, data);
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<GamePadState> gamePadState =
            new UltravioletSingleton<GamePadState>(uv => new GamePadState(uv));
    }
}
