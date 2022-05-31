using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an association between a key gesture and a command.
    /// </summary>
    [UvmlKnownType]
    public class KeyBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        public KeyBinding() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        /// <param name="command">The command associated with the specified gesture.</param>
        /// <param name="gesture">The gesture associated with the specified command.</param>
        public KeyBinding(ICommand command, KeyGesture gesture)
            : base(command, gesture)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        /// <param name="command">The command associated with the specified gesture.</param>
        /// <param name="key">The key associated with the specified command.</param>
        /// <param name="modifiers">The key modifiers associated with the specified command.</param>
        public KeyBinding(ICommand command, Key key, ModifierKeys modifiers) 
            : base(command, new KeyGesture(key, modifiers))
        {

        }

        /// <inheritdoc/>
        public override InputGesture Gesture
        {
            get { return base.Gesture as KeyGesture; }
            set
            {
                var keyGesture = value as KeyGesture;
                if (keyGesture == null)
                    throw new ArgumentException(nameof(value));

                base.Gesture = keyGesture;
                UpdateBindingFromGesture();
            }
        }

        /// <summary>
        /// Gets or sets the key associated with the binding's command.
        /// </summary>
        public Key Key
        {
            get { return GetValue<Key>(KeyProperty); }
            set { SetValue(KeyProperty, value); }
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
        /// Identifies the <see cref="Key"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(Key), typeof(KeyBinding),
            new PropertyMetadata<Key>(Key.None, HandleKeyChanged));

        /// <summary>
        /// Identifies the <see cref="Modifiers"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register(nameof(Modifiers), typeof(ModifierKeys), typeof(KeyBinding),
            new PropertyMetadata<ModifierKeys>(ModifierKeys.None, HandleModifiersChanged));
        
        /// <summary>
        /// Occurs when the value of the <see cref="Key"/> dependency property changes.
        /// </summary>
        private static void HandleKeyChanged(DependencyObject dobj, Key oldValue, Key newValue)
        {
            var binding = (KeyBinding)dobj;
            binding.UpdateGestureFromBinding();
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="Modifiers"/> dependency property changes.
        /// </summary>
        private static void HandleModifiersChanged(DependencyObject dobj, ModifierKeys oldValue, ModifierKeys newValue)
        {
            var binding = (KeyBinding)dobj;
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
            
            Gesture = new KeyGesture(Key, Modifiers);

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

            var keyGesture = Gesture as KeyGesture;
            Key = keyGesture.Key;
            Modifiers = keyGesture.Modifiers;

            updating = false;
        }

        // State values
        private Boolean updating;
    }
}
