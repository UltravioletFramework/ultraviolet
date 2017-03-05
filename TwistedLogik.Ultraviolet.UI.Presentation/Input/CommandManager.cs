using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a command is executed.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="command">The command that is being executed.</param>
    /// <param name="parameter">The parameter object that is being passed to the executing command.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void ExecutedRoutedEventHandler(DependencyObject element, ICommand command, Object parameter, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a command is being checked to determine whether it can execute.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="command">The command that is being evaluated.</param>
    /// <param name="parameter">The parameter object that is being passed to the evaluated command.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void CanExecuteRoutedEventHandler(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data);

    /// <summary>
    /// Contains methods for registering command and input bindings and managing command handlers.
    /// </summary>
    public partial class CommandManager
    {
        /// <summary>
        /// Forces the <see cref="CommandManager"/> to raise the <see cref="RequerySuggested"/> event.
        /// </summary>
        public static void InvalidateRequerySuggested()
        {
            var manager = requeryManagerSingleton.Value;
            if (manager != null)
                manager.InvalidateRequerySuggested();
        }

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
            Contract.Require(type, nameof(type));
            Contract.Require(inputBinding, nameof(inputBinding));

            lock (((IDictionary)classInputBindings).SyncRoot)
            {
                InputBindingCollection bindings;
                if (!classInputBindings.TryGetValue(type, out bindings))
                {
                    bindings = new InputBindingCollection();
                    classInputBindings[type] = bindings;
                }
                bindings.Add(inputBinding);
            }
        }

        /// <summary>
        /// Registers a command binding for the specified type.
        /// </summary>
        /// <param name="type">The type for which to register a binding.</param>
        /// <param name="commandBinding">The command binding to register for the specified type.</param>
        public static void RegisterClassCommandBinding(Type type, CommandBinding commandBinding)
        {
            Contract.Require(type, nameof(type));
            Contract.Require(commandBinding, nameof(commandBinding));

            lock (((IDictionary)classCommandBindings).SyncRoot)
            {
                CommandBindingCollection bindings;
                if (!classCommandBindings.TryGetValue(type, out bindings))
                {
                    bindings = new CommandBindingCollection();
                    classCommandBindings[type] = bindings;
                }
                bindings.Add(commandBinding);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="CommandManager"/> detects conditions that might change the ability of a command to execute.
        /// </summary>
        public static event EventHandler RequerySuggested
        {
            add
            {
                var manager = requeryManagerSingleton.Value;
                if (manager == null)
                    throw new InvalidOperationException(UltravioletStrings.ContextMissing);

                manager.Add(value);
            }
            remove
            {
                var manager = requeryManagerSingleton.Value;
                if (manager == null)
                    throw new InvalidOperationException(UltravioletStrings.ContextMissing);

                manager.Remove(value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.CommandManager.PreviewExecuted"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the <see cref="RoutedCommand.Execute(PresentationFoundationView, object)"/> method on a <see cref="RoutedCommand"/> is called.
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
        /// Occurs when the <see cref="RoutedCommand.Execute(PresentationFoundationView, object)"/> method on a <see cref="RoutedCommand"/> is called.
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
        /// Occurs when the <see cref="RoutedCommand.CanExecute(PresentationFoundationView, object)"/> method on a <see cref="RoutedCommand"/> is called.
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
        /// Occurs when the <see cref="RoutedCommand.CanExecute(PresentationFoundationView, object)"/> method on a <see cref="RoutedCommand"/> is called.
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

        /// <summary>
        /// Determines whether the specified command source can execute its command.
        /// </summary>
        internal static Boolean CheckCanExecuteSource(PresentationFoundationView view, ICommandSource source)
        {
            Contract.Require(view, nameof(view));
            Contract.Require(source, nameof(source));

            var command = source.Command;
            if (command == null)
                return false;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null)
            {
                var target = source.CommandTarget ?? source as IInputElement;
                return routedCommand.CanExecute(view, source.CommandParameter, target);
            }

            return command.CanExecute(view, source.CommandParameter);
        }

        /// <summary>
        /// Executes the specified command source's command.
        /// </summary>
        internal static void ExecuteSource(PresentationFoundationView view, ICommandSource source)
        {
            Contract.Require(view, nameof(view));
            Contract.Require(source, nameof(source));

            var command = source.Command;
            if (command == null)
                return;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null)
            {
                var target = source.CommandTarget ?? source as IInputElement;
                routedCommand.Execute(view, source.CommandParameter, target);
                return;
            }

            command.Execute(view, source.CommandParameter);
        }

        /// <summary>
        /// Registers the value resolvers used to parse commands.
        /// </summary>
        internal static void RegisterValueResolvers()
        {
            ObjectResolver.RegisterValueResolver<ICommand>(ICommandResolver);
        }

        /// <summary>
        /// Attempts to translate a mouse click event into a command invocation for the specified element.
        /// </summary>
        internal static void HandleMouseClickTranslation(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return;

            // Check element input commands
            var invocation = FindMatchingBinding_MouseClick(uiElement.InputBindings, device, button, data);

            // Check class input bindings
            if (invocation == null)
            {
                lock (((IDictionary)classInputBindings).SyncRoot)
                {
                    var bindings = default(InputBindingCollection);
                    if (classInputBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseClick(bindings, device, button, data);
                }
            }

            // Check element command bindings
            invocation = invocation ?? FindMatchingBinding_MouseClick(uiElement.CommandBindings, device, button, data);

            // Check class command bindings
            if (invocation == null)
            {
                lock (((IDictionary)classCommandBindings).SyncRoot)
                {
                    var bindings = default(CommandBindingCollection);
                    if (classCommandBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseClick(bindings, device, button, data);
                }
            }

            // Execute command, if found
            ExecuteTranslatedCommand(invocation, uiElement, data);
        }

        /// <summary>
        /// Attempts to translate a mouse double click event into a command invocation for the specified element.
        /// </summary>
        internal static void HandleMouseDoubleClickTranslation(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return;

            // Check element input commands
            var invocation = FindMatchingBinding_MouseDoubleClick(uiElement.InputBindings, device, button, data);

            // Check class input bindings
            if (invocation == null)
            {
                lock (((IDictionary)classInputBindings).SyncRoot)
                {
                    var bindings = default(InputBindingCollection);
                    if (classInputBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseDoubleClick(bindings, device, button, data);
                }
            }

            // Check element command bindings
            invocation = invocation ?? FindMatchingBinding_MouseDoubleClick(uiElement.CommandBindings, device, button, data);

            // Check class command bindings
            if (invocation == null)
            {
                lock (((IDictionary)classCommandBindings).SyncRoot)
                {
                    var bindings = default(CommandBindingCollection);
                    if (classCommandBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseDoubleClick(bindings, device, button, data);
                }
            }

            // Execute command, if found
            ExecuteTranslatedCommand(invocation, uiElement, data);
        }

        /// <summary>
        /// Attempts to translate a mouse wheel event into a command invocation for the specified element.
        /// </summary>
        internal static void HandleMouseWheelTranslation(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return;

            // Check element input commands
            var invocation = FindMatchingBinding_MouseWheel(uiElement.InputBindings, device, x, y, data);

            // Check class input bindings
            if (invocation == null)
            {
                lock (((IDictionary)classInputBindings).SyncRoot)
                {
                    var bindings = default(InputBindingCollection);
                    if (classInputBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseWheel(bindings, device, x, y, data);
                }
            }

            // Check element command bindings
            invocation = invocation ?? FindMatchingBinding_MouseWheel(uiElement.CommandBindings, device, x, y, data);

            // Check class command bindings
            if (invocation == null)
            {
                lock (((IDictionary)classCommandBindings).SyncRoot)
                {
                    var bindings = default(CommandBindingCollection);
                    if (classCommandBindings.TryGetValue(uiElement.GetType(), out bindings))
                        invocation = FindMatchingBinding_MouseWheel(bindings, device, x, y, data);
                }
            }

            // Execute command, if found
            ExecuteTranslatedCommand(invocation, uiElement, data);
        }

        /// <summary>
        /// Attempts to translate a key down event into a command invocation for the specified element.
        /// </summary>
        internal static void HandleKeyDownTranslation(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return;

            // Check element input commands
            var invocation = FindMatchingBinding_KeyDown(uiElement.InputBindings, device, key, modifiers, data);

            // Check class input bindings
            if (invocation == null)
            {
                lock (((IDictionary)classInputBindings).SyncRoot)
                {
                    var bindings = default(InputBindingCollection);
                    var type = element.GetType();
                    while (type != null)
                    {
                        if (classInputBindings.TryGetValue(type, out bindings))
                        {
                            invocation = FindMatchingBinding_KeyDown(bindings, device, key, modifiers, data);
                            if (invocation != null)
                            {
                                break;
                            }
                        }
                        type = type.BaseType;
                    }
                }
            }

            // Check element command bindings
            invocation = invocation ?? FindMatchingBinding_KeyDown(uiElement.CommandBindings, device, key, modifiers, data);

            // Check class command bindings
            if (invocation == null)
            {
                lock (((IDictionary)classCommandBindings).SyncRoot)
                {
                    var bindings = default(CommandBindingCollection);
                    var type = element.GetType();
                    while (type != null)
                    {
                        if (classCommandBindings.TryGetValue(type, out bindings))
                        {
                            invocation = FindMatchingBinding_KeyDown(bindings, device, key, modifiers, data);
                            if (invocation != null)
                            {
                                break;
                            }
                        }
                        type = type.BaseType;
                    }
                }
            }

            // Execute command, if found
            ExecuteTranslatedCommand(invocation, uiElement, data);
        }

        /// <summary>
        /// Handles the <see cref="PreviewExecutedEvent"/> routed event for the specified element.
        /// </summary>
        internal static void HandlePreviewExecuted(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            HandleCommandEvent(element, command, parameter, data, true);
        }

        /// <summary>
        /// Handles the <see cref="ExecutedEvent"/> routed event for the specified element.
        /// </summary>
        internal static void HandleExecuted(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            HandleCommandEvent(element, command, parameter, data, false);
        }

        /// <summary>
        /// Handles the <see cref="PreviewCanExecuteEvent"/> routed event for the specified element.
        /// </summary>
        internal static void HandlePreviewCanExecute(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            HandleCommandEvent(element, command, parameter, data, true);
        }

        /// <summary>
        /// Handles the <see cref="CanExecuteEvent"/> routed event for the specified element.
        /// </summary>
        internal static void HandleCanExecute(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            HandleCommandEvent(element, command, parameter, data, false);
        }

        /// <summary>
        /// Handles the routed events which implement the commanding infrastructure for the specified element.
        /// </summary>
        private static void HandleCommandEvent(DependencyObject element, ICommand command, Object parameter, RoutedEventData data, Boolean preview)
        {
            if (element == null || command == null)
                return;

            InvokeCommandBinding(element, command, parameter, data, preview);

            if (data.Handled || preview)
                return;

            if (FocusManager.GetIsFocusScope(element))
            {
                var scope = VisualTreeHelper.GetParent(element);
                if (scope == null)
                    scope = LogicalTreeHelper.GetParent(element);

                if (scope != null)
                {
                    var scopeFocusedElement = FocusManager.GetFocusedElement(scope) as DependencyObject;
                    if (scopeFocusedElement != null && !FocusManager.IsDescendantOfScope(element, scopeFocusedElement))
                    {
                        RedirectCommandEvent(scopeFocusedElement, command, parameter, data, preview);
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the appropriate command binding for the specified event.
        /// </summary>
        private static void InvokeCommandBinding(DependencyObject element, ICommand command, Object parameter, RoutedEventData data, Boolean preview)
        {
            var searchList = commandBindingsSearchList.Value;
            try
            {
                // Invoke local bindings, if there are any.
                var elementBindings = (element as UIElement)?.CommandBindings;
                if (elementBindings != null)
                {
                    foreach (var binding in elementBindings)
                    {
                        if (binding.Command == command)
                            binding.HandleExecutedOrCanExecute(element, command, parameter, data, preview);
                    }
                }

                // Search for class bindings.
                lock (((IDictionary)classCommandBindings).SyncRoot)
                {
                    var type = element.GetType();
                    while (type != null)
                    {
                        var classBindingsForType = default(CommandBindingCollection);
                        if (classCommandBindings.TryGetValue(type, out classBindingsForType))
                        {
                            for (int i = 0; i < classCommandBindings.Count; i++)
                            {
                                var binding = classBindingsForType[i];
                                if (binding.Command == command)
                                {
                                    searchList.Add(new CommandBindingKey(type, binding));
                                }
                            }
                        }
                        type = type.BaseType;
                    }
                }

                // Invoke class bindings, if there are any.
                if (searchList.Count > 0)
                {
                    for (int i = 0; i < searchList.Count; i++)
                    {
                        var binding = searchList[i].Binding;
                        binding.HandleExecutedOrCanExecute(element, command, parameter, data, preview);

                        // Skip past other handlers for this type
                        if (data.Handled)
                        {
                            var j = i;
                            while (j < searchList.Count && searchList[j].Type == searchList[j].Type)
                                j++;

                            i = j - 1;
                        }
                    }
                }
            }
            finally
            {
                searchList.Clear();
            }
        }

        /// <summary>
        /// Redirects a routed event to the specified element.
        /// </summary>
        private static void RedirectCommandEvent(DependencyObject element, ICommand command, Object parameter, RoutedEventData data, Boolean preview)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return;

            try
            {
                var view = uiElement.View;
                var execute = !(data is CanExecuteRoutedEventData);
                if (execute)
                {
                    ((RoutedCommand)command).Execute(view, parameter, uiElement);
                }
                else
                {
                    ((CanExecuteRoutedEventData)data).CanExecute = ((RoutedCommand)command).CanExecute(view, parameter, uiElement);
                }
            }
            finally
            {
                data.Handled = true;
            }
        }

        /// <summary>
        /// Searches the specified collection of input bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseClick(InputBindingCollection bindings, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                if (binding.Command != null && (binding?.Gesture?.MatchesMouseClick(device, button, data) ?? false))
                    return new CommandInvocationData(binding.CommandTarget, binding.Command, binding.CommandParameter);
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of input bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseDoubleClick(InputBindingCollection bindings, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                if (binding.Command != null && (binding?.Gesture?.MatchesMouseDoubleClick(device, button, data) ?? false))
                    return new CommandInvocationData(binding.CommandTarget, binding.Command, binding.CommandParameter);
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of input bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseWheel(InputBindingCollection bindings, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                if (binding.Command != null && (binding?.Gesture?.MatchesMouseWheel(device, x, y, data) ?? false))
                    return new CommandInvocationData(binding.CommandTarget, binding.Command, binding.CommandParameter);
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of input bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_KeyDown(InputBindingCollection bindings, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                if (binding.Command != null && (binding?.Gesture?.MatchesKeyDown(device, key, modifiers, data) ?? false))
                    return new CommandInvocationData(binding.CommandTarget, binding.Command, binding.CommandParameter);
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of command bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseClick(CommandBindingCollection bindings, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                var command = binding.Command as RoutedCommand;
                if (command == null)
                    continue;

                var gestures = command.InputGestures;
                for (int j = gestures.Count - 1; j >= 0; j--)
                {
                    if (gestures[j].MatchesMouseClick(device, button, data))
                        return new CommandInvocationData(null, binding.Command, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of command bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseDoubleClick(CommandBindingCollection bindings, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                var command = binding.Command as RoutedCommand;
                if (command == null)
                    continue;

                var gestures = command.InputGestures;
                for (int j = gestures.Count - 1; j >= 0; j--)
                {
                    if (gestures[j].MatchesMouseDoubleClick(device, button, data))
                        return new CommandInvocationData(null, binding.Command, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of command bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_MouseWheel(CommandBindingCollection bindings, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                var command = binding.Command as RoutedCommand;
                if (command == null)
                    continue;

                var gestures = command.InputGestures;
                for (int j = gestures.Count - 1; j >= 0; j--)
                {
                    if (gestures[j].MatchesMouseWheel(device, x, y, data))
                        return new CommandInvocationData(null, binding.Command, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches the specified collection of command bindings for a binding which matches the specified input event.
        /// </summary>
        private static CommandInvocationData? FindMatchingBinding_KeyDown(CommandBindingCollection bindings, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                var binding = bindings[i];
                var command = binding.Command as RoutedCommand;
                if (command == null)
                    continue;

                var gestures = command.InputGestures;
                for (int j = gestures.Count - 1; j >= 0; j--)
                {
                    if (gestures[j].MatchesKeyDown(device, key, modifiers, data))
                        return new CommandInvocationData(null, binding.Command, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Executes a command as a result of an input event.
        /// </summary>
        private static void ExecuteTranslatedCommand(CommandInvocationData? invocation, UIElement element, RoutedEventData data)
        {
            if (invocation == null || element == null)
                return;

            var @continue = false;
            var commandData = invocation.GetValueOrDefault();
            var commandTarget = commandData.CommandTarget ?? element;
            var commandParameter = commandData.CommandParameter;

            var command = commandData.Command;
            if (command == ApplicationCommands.NotACommand)
                return;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null)
            {
                if (routedCommand.CanExecute(element.View, commandParameter, commandTarget, out @continue))
                {
                    routedCommand.Execute(element.View, commandParameter, commandTarget);
                    @continue = false;
                }
            }
            else
            {
                if (command.CanExecute(element.View, commandParameter))
                    command.Execute(element.View, commandParameter);
            }

            data.Handled = !@continue;
        }

        /// <summary>
        /// Resolves <see cref="ICommand"/> instances from string values.
        /// </summary>
        private static Object ICommandResolver(String value, IFormatProvider provider)
        {
            Contract.Require(value, nameof(value));

            var parts = value.Split('.');
            if (parts.Length != 2)
                throw new FormatException();

            switch (parts[0])
            {
                case nameof(ApplicationCommands):
                    return ICommandResolver_ApplicationCommands(parts[1], provider);

                case nameof(ComponentCommands):
                    break;

                case nameof(MediaCommands):
                    break;

                case nameof(NavigationCommands):
                    break;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Resolves <see cref="ApplicationCommands"/> from string values.
        /// </summary>
        private static Object ICommandResolver_ApplicationCommands(String value, IFormatProvider provider)
        {
            switch (value)
            {
                case nameof(ApplicationCommands.CancelPrint):
                    return ApplicationCommands.CancelPrint;

                case nameof(ApplicationCommands.Close):
                    return ApplicationCommands.Close;

                case nameof(ApplicationCommands.ContextMenu):
                    return ApplicationCommands.ContextMenu;

                case nameof(ApplicationCommands.Copy):
                    return ApplicationCommands.Copy;

                case nameof(ApplicationCommands.CorrectionList):
                    return ApplicationCommands.CorrectionList;

                case nameof(ApplicationCommands.Cut):
                    return ApplicationCommands.Cut;

                case nameof(ApplicationCommands.Delete):
                    return ApplicationCommands.Delete;

                case nameof(ApplicationCommands.Find):
                    return ApplicationCommands.Find;

                case nameof(ApplicationCommands.Help):
                    return ApplicationCommands.Help;

                case nameof(ApplicationCommands.New):
                    return ApplicationCommands.New;

                case nameof(ApplicationCommands.NotACommand):
                    return ApplicationCommands.NotACommand;

                case nameof(ApplicationCommands.Open):
                    return ApplicationCommands.Open;

                case nameof(ApplicationCommands.Paste):
                    return ApplicationCommands.Paste;

                case nameof(ApplicationCommands.Print):
                    return ApplicationCommands.Print;

                case nameof(ApplicationCommands.PrintPreview):
                    return ApplicationCommands.PrintPreview;

                case nameof(ApplicationCommands.Properties):
                    return ApplicationCommands.Properties;

                case nameof(ApplicationCommands.Redo):
                    return ApplicationCommands.Redo;

                case nameof(ApplicationCommands.Replace):
                    return ApplicationCommands.Replace;

                case nameof(ApplicationCommands.Save):
                    return ApplicationCommands.Save;

                case nameof(ApplicationCommands.SaveAs):
                    return ApplicationCommands.SaveAs;

                case nameof(ApplicationCommands.SelectAll):
                    return ApplicationCommands.SelectAll;

                case nameof(ApplicationCommands.Stop):
                    return ApplicationCommands.Stop;

                case nameof(ApplicationCommands.Undo):
                    return ApplicationCommands.Undo;
            }
            throw new FormatException();
        }

        /// <summary>
        /// Resolves <see cref="ComponentCommands"/> from string values.
        /// </summary>
        private static Object ICommandResolver_ComponentCommands(String value, IFormatProvider provider)
        {
            switch (value)
            {
                case nameof(ComponentCommands.ExtendSelectionDown):
                    return ComponentCommands.ExtendSelectionDown;

                case nameof(ComponentCommands.ExtendSelectionLeft):
                    return ComponentCommands.ExtendSelectionLeft;

                case nameof(ComponentCommands.ExtendSelectionRight):
                    return ComponentCommands.ExtendSelectionRight;

                case nameof(ComponentCommands.ExtendSelectionUp):
                    return ComponentCommands.ExtendSelectionUp;

                case nameof(ComponentCommands.MoveDown):
                    return ComponentCommands.MoveDown;

                case nameof(ComponentCommands.MoveFocusBack):
                    return ComponentCommands.MoveFocusBack;

                case nameof(ComponentCommands.MoveFocusDown):
                    return ComponentCommands.MoveFocusDown;

                case nameof(ComponentCommands.MoveFocusForward):
                    return ComponentCommands.MoveFocusForward;

                case nameof(ComponentCommands.MoveFocusUp):
                    return ComponentCommands.MoveFocusUp;

                case nameof(ComponentCommands.MoveLeft):
                    return ComponentCommands.MoveLeft;

                case nameof(ComponentCommands.MoveRight):
                    return ComponentCommands.MoveRight;

                case nameof(ComponentCommands.MoveToEnd):
                    return ComponentCommands.MoveToEnd;

                case nameof(ComponentCommands.MoveToHome):
                    return ComponentCommands.MoveToHome;

                case nameof(ComponentCommands.MoveToPageDown):
                    return ComponentCommands.MoveToPageDown;

                case nameof(ComponentCommands.MoveToPageUp):
                    return ComponentCommands.MoveToPageUp;

                case nameof(ComponentCommands.MoveUp):
                    return ComponentCommands.MoveUp;

                case nameof(ComponentCommands.ScrollDown):
                    return ComponentCommands.ScrollDown;

                case nameof(ComponentCommands.ScrollLeft):
                    return ComponentCommands.ScrollLeft;

                case nameof(ComponentCommands.ScrollRight):
                    return ComponentCommands.ScrollRight;

                case nameof(ComponentCommands.ScrollUp):
                    return ComponentCommands.ScrollUp;

                case nameof(ComponentCommands.SelectToEnd):
                    return ComponentCommands.SelectToEnd;

                case nameof(ComponentCommands.SelectToHome):
                    return ComponentCommands.SelectToHome;

                case nameof(ComponentCommands.SelectToPageDown):
                    return ComponentCommands.SelectToPageDown;

                case nameof(ComponentCommands.SelectToPageUp):
                    return ComponentCommands.SelectToPageUp;
            }
            throw new FormatException();
        }

        /// <summary>
        /// Resolves <see cref="MediaCommands"/> from string values.
        /// </summary>
        private static Object ICommandResolver_MediaCommands(String value, IFormatProvider provider)
        {
            switch (value)
            {
                case nameof(MediaCommands.BoostBass):
                    return MediaCommands.BoostBass;

                case nameof(MediaCommands.ChannelDown):
                    return MediaCommands.ChannelDown;

                case nameof(MediaCommands.ChannelUp):
                    return MediaCommands.ChannelUp;

                case nameof(MediaCommands.DecreaseBass):
                    return MediaCommands.DecreaseBass;

                case nameof(MediaCommands.DecreaseMicrophoneVolume):
                    return MediaCommands.DecreaseMicrophoneVolume;

                case nameof(MediaCommands.DecreaseTreble):
                    return MediaCommands.DecreaseTreble;

                case nameof(MediaCommands.DecreaseVolume):
                    return MediaCommands.DecreaseVolume;

                case nameof(MediaCommands.FastForward):
                    return MediaCommands.FastForward;

                case nameof(MediaCommands.IncreaseBass):
                    return MediaCommands.IncreaseBass;

                case nameof(MediaCommands.IncreaseMicrophoneVolume):
                    return MediaCommands.IncreaseMicrophoneVolume;

                case nameof(MediaCommands.IncreaseTreble):
                    return MediaCommands.IncreaseTreble;

                case nameof(MediaCommands.IncreaseVolume):
                    return MediaCommands.IncreaseVolume;

                case nameof(MediaCommands.MuteMicrophoneVolume):
                    return MediaCommands.MuteMicrophoneVolume;

                case nameof(MediaCommands.MuteVolume):
                    return MediaCommands.MuteVolume;

                case nameof(MediaCommands.NextTrack):
                    return MediaCommands.NextTrack;

                case nameof(MediaCommands.Pause):
                    return MediaCommands.Pause;

                case nameof(MediaCommands.Play):
                    return MediaCommands.Play;

                case nameof(MediaCommands.PreviousTrack):
                    return MediaCommands.PreviousTrack;

                case nameof(MediaCommands.Record):
                    return MediaCommands.Record;

                case nameof(MediaCommands.Rewind):
                    return MediaCommands.Rewind;

                case nameof(MediaCommands.Select):
                    return MediaCommands.Select;

                case nameof(MediaCommands.Stop):
                    return MediaCommands.Stop;

                case nameof(MediaCommands.ToggleMicrophoneOnOff):
                    return MediaCommands.ToggleMicrophoneOnOff;

                case nameof(MediaCommands.TogglePlayPause):
                    return MediaCommands.TogglePlayPause;
            }
            throw new FormatException();
        }

        /// <summary>
        /// Resolves <see cref="NavigationCommands"/> from string values.
        /// </summary>
        private static Object ICommandResolver_NavigationCommands(String value, IFormatProvider provider)
        {
            switch (value)
            {
                case nameof(NavigationCommands.BrowseBack):
                    return NavigationCommands.BrowseBack;

                case nameof(NavigationCommands.BrowseForward):
                    return NavigationCommands.BrowseForward;

                case nameof(NavigationCommands.BrowseHome):
                    return NavigationCommands.BrowseHome;

                case nameof(NavigationCommands.BrowseStop):
                    return NavigationCommands.BrowseStop;

                case nameof(NavigationCommands.DecreaseZoom):
                    return NavigationCommands.DecreaseZoom;

                case nameof(NavigationCommands.Favorites):
                    return NavigationCommands.Favorites;

                case nameof(NavigationCommands.FirstPage):
                    return NavigationCommands.FirstPage;

                case nameof(NavigationCommands.GoToPage):
                    return NavigationCommands.GoToPage;

                case nameof(NavigationCommands.IncreaseZoom):
                    return NavigationCommands.IncreaseZoom;

                case nameof(NavigationCommands.LastPage):
                    return NavigationCommands.LastPage;

                case nameof(NavigationCommands.NavigateJournal):
                    return NavigationCommands.NavigateJournal;

                case nameof(NavigationCommands.NextPage):
                    return NavigationCommands.NextPage;

                case nameof(NavigationCommands.PreviousPage):
                    return NavigationCommands.PreviousPage;

                case nameof(NavigationCommands.Refresh):
                    return NavigationCommands.Refresh;

                case nameof(NavigationCommands.Search):
                    return NavigationCommands.Search;

                case nameof(NavigationCommands.Zoom):
                    return NavigationCommands.Zoom;
            }
            throw new FormatException();
        }

        // Manager instance used to recognize events raised by us
        private static readonly CommandManager instance = new CommandManager();

        // Manager for status requery requests
        private static readonly UltravioletSingleton<CommandRequeryManager> requeryManagerSingleton =
            new UltravioletSingleton<CommandRequeryManager>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
                new CommandRequeryManager(uv, instance));

        // Class binding collections
        private static readonly Dictionary<Type, CommandBindingCollection> classCommandBindings = new Dictionary<Type, CommandBindingCollection>();
        private static readonly Dictionary<Type, InputBindingCollection> classInputBindings = new Dictionary<Type, InputBindingCollection>();

        // State values.
        private static readonly ThreadLocal<List<CommandBindingKey>> commandBindingsSearchList = 
            new ThreadLocal<List<CommandBindingKey>>(() => new List<CommandBindingKey>());
    }
}
