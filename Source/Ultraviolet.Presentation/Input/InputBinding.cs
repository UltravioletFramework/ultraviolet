using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a binding between an input gesture and a command.
    /// </summary>
    [UvmlKnownType]
    public class InputBinding : DependencyObject, ICommandSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputBinding"/> class.
        /// </summary>
        protected InputBinding() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBinding"/> class.
        /// </summary>
        /// <param name="command">The command associated with the specified gesture.</param>
        /// <param name="gesture">The gesture associated with the specified command.</param>
        public InputBinding(ICommand command, InputGesture gesture)
        {
            Contract.Require(command, nameof(command));
            Contract.Require(gesture, nameof(gesture));

            this.Command = command;
            this.gesture = gesture;
        }

        /// <summary>
        /// Gets or sets the command's target element.
        /// </summary>
        public IInputElement CommandTarget
        {
            get { return GetValue<IInputElement>(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command associated with the binding's gesture.
        /// </summary>
        public ICommand Command
        {
            get { return GetValue<ICommand>(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter object which is passed to the binding's command when it is invoked.
        /// </summary>
        public Object CommandParameter
        {
            get { return GetValue<Object>(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gesture which is associated with the binding's command.
        /// </summary>
        public virtual InputGesture Gesture
        {
            get { return gesture; }
            set
            {
                Contract.Require(value, nameof(value));
                gesture = value;
            }
        }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(InputBinding),
            new PropertyMetadata<IInputElement>(null));

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(InputBinding),
            new PropertyMetadata<ICommand>(null));

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(Object), typeof(InputBinding),
            new PropertyMetadata<Object>(null));

        // Property values
        private InputGesture gesture;
    }
}
