using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command which is routed through the element tree.
    /// </summary>
    public class RoutedCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="ownerType">The type that owns the command.</param>
        public RoutedCommand(String name, Type ownerType)
            : this(name, ownerType, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="ownerType">The type that owns the command.</param>
        /// <param name="inputGestures">A collection containing the default input gestures associated with the command.</param>
        public RoutedCommand(String name, Type ownerType, InputGestureCollection inputGestures)
        {
            Contract.Require(name, nameof(name));
            Contract.Require(ownerType, nameof(ownerType));

            this.name = name;
            this.ownerType = ownerType;
            this.inputGestures = inputGestures;
        }

        /// <inheritdoc/>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter)
        {
            var @continue = false;
            return CanExecute(view, parameter, null, Keyboard.GetFocusedElement(view), out @continue);
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="valparameter">The command value parameter, or <see langword="null"/> if the command
        /// does not require a value parameter. This parameter is intended for use in internal micro-optimizations and
        /// will appear as one of the fields in the routed event data.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter, PrimitiveUnion? valparameter)
        {
            var @continue = false;
            return CanExecute(view, parameter, valparameter, Keyboard.GetFocusedElement(view), out @continue);
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter, IInputElement target)
        {
            var @continue = false;
            return CanExecute(view, parameter, null, target, out @continue);
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="valparameter">The command value parameter, or <see langword="null"/> if the command
        /// does not require a value parameter. This parameter is intended for use in internal micro-optimizations and
        /// will appear as one of the fields in the routed event data.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter, PrimitiveUnion? valparameter, IInputElement target)
        {
            var @continue = false;
            return CanExecute(view, parameter, valparameter, target, out @continue);
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        /// <param name="continue">A value indicating whether command routing should continue.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter, IInputElement target, out Boolean @continue)
        {
            return CanExecute(view, parameter, null, target, out @continue);
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="valparameter">The command value parameter, or <see langword="null"/> if the command
        /// does not require a value parameter. This parameter is intended for use in internal micro-optimizations and
        /// will appear as one of the fields in the routed event data.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        /// <param name="continue">A value indicating whether command routing should continue.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter, PrimitiveUnion? valparameter, IInputElement target, out Boolean @continue)
        {
            Contract.Require(view, nameof(view));

            var uiElement = target as UIElement;
            if (uiElement != null && uiElement.View != view)
                throw new ArgumentException(PresentationStrings.ElementDoesNotBelongToView);

            if (uiElement != null)
            {
                var data = CanExecuteRoutedEventData.Retrieve(this, valparameter, autorelease: false);
                try
                {
                    var evtPreview = EventManager.GetInvocationDelegate<UpfCanExecuteRoutedEventHandler>(CommandManager.PreviewCanExecuteEvent);
                    evtPreview(uiElement, this, parameter, data);

                    if (!data.Handled)
                    {
                        var evt = EventManager.GetInvocationDelegate<UpfCanExecuteRoutedEventHandler>(CommandManager.CanExecuteEvent);
                        evt(uiElement, this, parameter, data);
                    }

                    @continue = data.ContinueRouting;
                    return data.CanExecute;
                }
                finally
                {
                    data.Release();
                }
            }
            else
            {
                @continue = false;
                return false;
            }
        }

        /// <inheritdoc/>
        public void Execute(PresentationFoundationView view, Object parameter)
        {
            Execute(view, parameter, null, Keyboard.GetFocusedElement(null));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="valparameter">The command value parameter, or <see langword="null"/> if the command
        /// does not require a value parameter. This parameter is intended for use in internal micro-optimizations and
        /// will appear as one of the fields in the routed event data.</param>
        public void Execute(PresentationFoundationView view, Object parameter, PrimitiveUnion? valparameter)
        {
            Execute(view, parameter, valparameter, Keyboard.GetFocusedElement(null));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        public void Execute(PresentationFoundationView view, Object parameter, IInputElement target)
        {
            Execute(view, parameter, null, target);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <param name="valparameter">The command value parameter, or <see langword="null"/> if the command
        /// does not require a value parameter. This parameter is intended for use in internal micro-optimizations and
        /// will appear as one of the fields in the routed event data.</param>
        /// <param name="target">The element within <paramref name="view"/> at which to begin
        /// searching for command handlers.</param>
        public void Execute(PresentationFoundationView view, Object parameter, PrimitiveUnion? valparameter, IInputElement target)
        {
            Contract.Require(view, nameof(view));

            var uiElement = target as UIElement;
            if (uiElement != null && uiElement.View != view)
                throw new ArgumentException(PresentationStrings.ElementDoesNotBelongToView);

            if (uiElement != null)
            {
                var data = ExecutedRoutedEventData.Retrieve(this, valparameter, autorelease: false);
                try
                {
                    var evtPreview = EventManager.GetInvocationDelegate<UpfExecutedRoutedEventHandler>(CommandManager.PreviewExecutedEvent);
                    evtPreview(uiElement, this, parameter, data);

                    if (!data.Handled)
                    {
                        var evt = EventManager.GetInvocationDelegate<UpfExecutedRoutedEventHandler>(CommandManager.ExecutedEvent);
                        evt(uiElement, this, parameter, data);
                    }
                }
                finally
                {
                    data.Release();
                }
            }
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the type that owns the command.
        /// </summary>
        public Type OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets the collection containing the default input gestures associated with this command.
        /// </summary>
        public InputGestureCollection InputGestures
        {
            get
            {
                if (inputGestures == null)
                    inputGestures = new InputGestureCollection();

                return inputGestures;
            }
        }
        
        // Property values.
        private readonly String name;
        private readonly Type ownerType;
        private InputGestureCollection inputGestures;
    }
}
