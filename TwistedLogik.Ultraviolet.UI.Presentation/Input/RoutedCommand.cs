using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents a command which is routed through the element tree.
    /// </summary>
    public class RoutedCommand
    {
        /// <inheritdoc/>
        public Boolean CanExecute(Object parameter)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Execute(Object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
