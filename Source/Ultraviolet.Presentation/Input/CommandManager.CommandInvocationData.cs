using System;

namespace Ultraviolet.Presentation.Input
{
    partial class CommandManager
    {
        /// <summary>
        /// Represents the data necessary to invoke a command.
        /// </summary>
        private struct CommandInvocationData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CommandInvocationData"/> structure.
            /// </summary>
            /// <param name="target">The command target.</param>
            /// <param name="command">The command.</param>
            /// <param name="parameter">The command parameter.</param>
            public CommandInvocationData(IInputElement target, ICommand command, Object parameter)
            {
                this.CommandTarget = target;
                this.Command = command;
                this.CommandParameter = parameter;
            }

            /// <summary>
            /// Gets the command target.
            /// </summary>
            public IInputElement CommandTarget { get; }

            /// <summary>
            /// Gets the command.
            /// </summary>
            public ICommand Command { get; }

            /// <summary>
            /// Gets the command parameter.
            /// </summary>
            public Object CommandParameter { get; }
        }
    }
}
