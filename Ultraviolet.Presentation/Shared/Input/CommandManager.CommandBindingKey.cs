using System;

namespace Ultraviolet.Presentation.Input
{
    partial class CommandManager
    {
        /// <summary>
        /// Represents an association between a type and a command binding.
        /// </summary>
        private struct CommandBindingKey
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CommandBindingKey"/> structure.
            /// </summary>
            /// <param name="type">The type that owns the binding.</param>
            /// <param name="binding">The command binding owned by the key's associated type.</param>
            public CommandBindingKey(Type type, CommandBinding binding)
            {
                this.Type = type;
                this.Binding = binding;
            }
            
            /// <summary>
            /// Gets the type that owns the command binding.
            /// </summary>
            public Type Type { get; }

            /// <summary>
            /// Gets the command binding owned by the key's associated type.
            /// </summary>
            public CommandBinding Binding { get; }
        }
    }
}
