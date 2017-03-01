using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a command is executed.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="command">The command that is being executed.</param>
    /// <param name="parameter">The parameter object that is being passed to the executing command.</param>
    public delegate void ExecutedRoutedEventHandler(Object sender, ICommand command, Object parameter);

    /// <summary>
    /// Represents the method that is called when a command is being checked to determine whether it can execute.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="command">The command that is being evaluated.</param>
    /// <param name="parameter">The parameter object that is being passed to the evaluated command.</param>
    /// <param name="args">The event's arguments.</param>
    public delegate void CanExecuteRoutedEventHandler(Object sender, ICommand command, Object parameter, CanExecuteEventArgs args);

    /// <summary>
    /// Contains methods for registering command and input bindings and managing command handlers.
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewExecutedEventHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.AddHandler(PreviewExecutedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.Executed"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddExecutedEventHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.AddHandler(ExecutedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewCanExecute"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewCanExecuteEventHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.AddHandler(PreviewCanExecuteEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.CanExecute"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddCanExecuteEventHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.AddHandler(CanExecuteEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewExecutedEventHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.RemoveHandler(PreviewExecutedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.Executed"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveExecutedEventHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.RemoveHandler(ExecutedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewCanExecute"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewCanExecuteEventHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.RemoveHandler(PreviewCanExecuteEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.CanExecute"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveCanExecuteEventHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            element.RemoveHandler(CanExecuteEvent, handler);
        }

        /// <summary>
        /// Registers an input binding for the specified type.
        /// </summary>
        /// <param name="type">The type for which to register a binding.</param>
        /// <param name="inputBinding">The input binding to register for the specified type.</param>
        public static void RegisterClassInputBinding(Type type, InputBinding inputBinding)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a command binding for the specified type.
        /// </summary>
        /// <param name="type">The type for which to register a binding.</param>
        /// <param name="commandBinding">The command binding to register for the specified type.</param>
        public static void RegisterClassCommandBinding(Type type, CommandBinding commandBinding)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the <see cref="RoutedCommand.Execute(object)"/> method on a <see cref="RoutedCommand"/> is called.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewExecutedEvent"/></revtField>
        ///     <revtStylingName>preview-executed</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="ExecutedRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.Executed"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewExecutedEvent = EventManager.RegisterRoutedEvent("PreviewExecuted", 
            RoutingStrategy.Tunnel, typeof(ExecutedRoutedEventHandler), typeof(CommandManager));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.Executed"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.Executed"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the <see cref="RoutedCommand.Execute(object)"/> method on a <see cref="RoutedCommand"/> is called.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ExecutedEvent"/></revtField>
        ///     <revtStylingName>executed</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="ExecutedRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent ExecutedEvent = EventManager.RegisterRoutedEvent("Executed",
            RoutingStrategy.Bubble, typeof(ExecutedRoutedEventHandler), typeof(CommandManager));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewCanExecute"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewCanExecute"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the <see cref="RoutedCommand.CanExecute(object)"/> method on a <see cref="RoutedCommand"/> is called.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewCanExecuteEvent"/></revtField>
        ///     <revtStylingName>preview-can-execute</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="CanExecuteRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.CanExecute"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewCanExecuteEvent = EventManager.RegisterRoutedEvent("PreviewCanExecute",
            RoutingStrategy.Tunnel, typeof(CanExecuteRoutedEventHandler), typeof(CommandManager));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.CanExecute"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.CanExecute"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the <see cref="RoutedCommand.CanExecute(object)"/> method on a <see cref="RoutedCommand"/> is called.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="CanExecuteEvent"/></revtField>
        ///     <revtStylingName>can-execute</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="CanExecuteRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewCanExecute"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent CanExecuteEvent = EventManager.RegisterRoutedEvent("CanExecute",
            RoutingStrategy.Bubble, typeof(CanExecuteRoutedEventHandler), typeof(CommandManager));
    }
}
