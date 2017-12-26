using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an association between a mouse gesture and a command.
    /// </summary>
    [UvmlKnownType]
    public class MouseBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseBinding"/> class.
        /// </summary>
        public MouseBinding() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseBinding"/> class.
        /// </summary>
        /// <param name="command">The command associated with the specified gesture.</param>
        /// <param name="gesture">The gesture associated with the specified command.</param>
        public MouseBinding(ICommand command, MouseGesture gesture)
            : base(command, gesture)
        {

        }
        
        /// <inheritdoc/>
        public override InputGesture Gesture
        {
            get { return base.Gesture as MouseGesture; }
            set
            {
                var mouseGesture = value as MouseGesture;
                if (mouseGesture == null)
                    throw new ArgumentException(nameof(value));

                base.Gesture = mouseGesture;
                UpdateBindingFromGesture();
            }
        }

        /// <summary>
        /// Gets or sets the mouse action associated with the binding's command.
        /// </summary>
        public MouseAction MouseAction
        {
            get { return GetValue<MouseAction>(MouseActionProperty); }
            set { SetValue(MouseActionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key modifiers associated with the binding's command.
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return GetValue<ModifierKeys>(ModifiersProperty); }
            set { SetValue(ModifiersProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MouseAction"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseActionProperty = DependencyProperty.Register(nameof(MouseAction), typeof(MouseAction), typeof(MouseBinding),
            new PropertyMetadata<MouseAction>(MouseAction.None, HandleMouseActionChanged));

        /// <summary>
        /// Identifies the <see cref="Modifiers"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register(nameof(Modifiers), typeof(ModifierKeys), typeof(MouseBinding),
            new PropertyMetadata<ModifierKeys>(ModifierKeys.None, HandleModifiersChanged));

        /// <summary>
        /// Occurs when the value of the <see cref="MouseAction"/> dependency property changes.
        /// </summary>
        private static void HandleMouseActionChanged(DependencyObject dobj, MouseAction oldValue, MouseAction newValue)
        {
            var binding = (MouseBinding)dobj;
            binding.UpdateGestureFromBinding();
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="Modifiers"/> dependency property changes.
        /// </summary>
        private static void HandleModifiersChanged(DependencyObject dobj, ModifierKeys oldValue, ModifierKeys newValue)
        {
            var binding = (MouseBinding)dobj;
            binding.UpdateGestureFromBinding();
        }

        /// <summary>
        /// Updates the value of the <see cref="Gesture"/> property based on the current property values of this binding.
        /// </summary>
        private void UpdateGestureFromBinding()
        {
            if (updating)
                return;

            updating = true;
            
            Gesture = new MouseGesture(MouseAction, Modifiers);

            updating = false;
        }

        /// <summary>
        /// Updates the property values of this binding based on the current value of the <see cref="Gesture"/> property.
        /// </summary>
        private void UpdateBindingFromGesture()
        {
            if (updating)
                return;

            updating = true;

            var mouseGesture = Gesture as MouseGesture;
            MouseAction = mouseGesture.MouseAction;
            Modifiers = mouseGesture.Modifiers;

            updating = false;
        }

        // State values
        private Boolean updating;
    }
}
