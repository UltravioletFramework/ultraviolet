using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command which is routed through the element tree and contains a text property.
    /// </summary>
    public class RoutedUICommand : RoutedCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedUICommand"/> class.
        /// </summary>
        /// <param name="text">The descriptive text for the command.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="ownerType">The type that owns the command.</param>
        public RoutedUICommand(String text, String name, Type ownerType)
            : base(name, ownerType, null)
        {
            Contract.Require(text, nameof(text));

            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedUICommand"/> class.
        /// </summary>
        /// <param name="text">The descriptive text for the command.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="ownerType">The type that owns the command.</param>
        /// <param name="inputGestures">A collection containing the default input gestures associated with the command.</param>
        public RoutedUICommand(String text, String name, Type ownerType, InputGestureCollection inputGestures)
            : base(name, ownerType, inputGestures)
        {
            Contract.Require(text, nameof(text));

            this.text = text;
        }

        /// <summary>
        /// Gets or sets the descriptive text for the command.
        /// </summary>
        public String Text
        {
            get { return text; }
            set
            {
                Contract.Require(text, nameof(text));

                this.text = value;
            }
        }
        
        // Property values.
        private String text;
    }
}
