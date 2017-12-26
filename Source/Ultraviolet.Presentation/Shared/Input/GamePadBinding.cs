using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an association between a game pad gesture and a command.
    /// </summary>
    [UvmlKnownType]
    public class GamePadBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadBinding"/> class.
        /// </summary>
        public GamePadBinding() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadBinding"/> class.
        /// </summary>
        /// <param name="command">The command associated with the specified gesture.</param>
        /// <param name="gesture">The gesture associated with the specified command.</param>
        public GamePadBinding(ICommand command, GamePadGesture gesture)
            : base(command, gesture)
        {

        }
        
        /// <inheritdoc/>
        public override InputGesture Gesture
        {
            get { return base.Gesture as GamePadGesture; }
            set
            {
                var gamePadGesture = value as GamePadGesture;
                if (gamePadGesture == null)
                    throw new ArgumentException(nameof(value));

                base.Gesture = gamePadGesture;
                UpdateBindingFromGesture();
            }
        }

        /// <summary>
        /// Gets or sets the game pad button associated with the binding's command.
        /// </summary>
        public GamePadButton Button
        {
            get { return GetValue<GamePadButton>(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets the game pad player index associated with the binding's command.
        /// </summary>
        public Int32 PlayerIndex
        {
            get { return GetValue<Int32>(PlayerIndexProperty); }
            set { SetValue(PlayerIndexProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Button"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register(nameof(Button), typeof(GamePadButton), typeof(GamePadBinding),
            new PropertyMetadata<GamePadButton>(GamePadButton.None, HandleButtonChanged));

        /// <summary>
        /// Identifies the <see cref="PlayerIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlayerIndexProperty = DependencyProperty.Register(nameof(PlayerIndex), typeof(Int32), typeof(GamePadBinding),
            new PropertyMetadata<Int32>(GamePadGesture.AnyPlayerIndex, HandlePlayerIndexChanged));

        /// <summary>
        /// Occurs when the value of the <see cref="Button"/> dependency property changes.
        /// </summary>
        private static void HandleButtonChanged(DependencyObject dobj, GamePadButton oldValue, GamePadButton newValue)
        {
            var binding = (GamePadBinding)dobj;
            binding.UpdateGestureFromBinding();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Button"/> dependency property changes.
        /// </summary>
        private static void HandlePlayerIndexChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var binding = (GamePadBinding)dobj;
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
            
            Gesture = new GamePadGesture(Button, PlayerIndex);

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

            var gamePadGesture = Gesture as GamePadGesture;
            Button = gamePadGesture.Button;
            PlayerIndex = gamePadGesture.PlayerIndex;

            updating = false;
        }

        // State values
        private Boolean updating;
    }
}
