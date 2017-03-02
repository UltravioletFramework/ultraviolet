using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents a command which is routed through the element tree.
    /// </summary>
    public class RoutedCommand : ICommand
    {
        /// <inheritdoc/>
        public Boolean CanExecute(PresentationFoundationView view, Object parameter)
        {
            return CanExecute(view, parameter, Keyboard.GetFocusedElement(null));
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
            Contract.Require(view, nameof(view));

            var uiElement = target as UIElement;
            if (uiElement != null && uiElement.View != view)
                throw new ArgumentException(PresentationStrings.ElementDoesNotBelongToView);

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Execute(PresentationFoundationView view, Object parameter)
        {
            Execute(view, parameter, Keyboard.GetFocusedElement(null));
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
            var uiElement = target as UIElement;
            if (uiElement != null && uiElement.View != view)
                throw new ArgumentException(PresentationStrings.ElementDoesNotBelongToView);

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
