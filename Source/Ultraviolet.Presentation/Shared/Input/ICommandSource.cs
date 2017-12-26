using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>A
    /// Represents an object that can invoke a command.
    /// </summary>
    public interface ICommandSource
    {
        /// <summary>
        /// Gets the command that is executed when the command source is invoked.
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        /// Gets the object that is targeted by the command source's executed command.
        /// </summary>
        IInputElement CommandTarget { get; }

        /// <summary>
        /// Gets the parameter object to pass to the command source's executed command.
        /// </summary>
        Object CommandParameter { get; }
    }
}
