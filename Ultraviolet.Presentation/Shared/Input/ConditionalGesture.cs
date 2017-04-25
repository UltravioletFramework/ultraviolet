using System;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an input gesture which is only valid if the specified condition is <see langword="true"/>
    /// </summary>
    internal sealed class ConditionalGesture : InputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalGesture"/> class.
        /// </summary>
        /// <param name="condition">The condition which must be satisfied in order for the gesture to be valid.</param>
        /// <param name="wrappedGesture">The gesture which is wrapped within this conditional gesture.</param>
        public ConditionalGesture(Func<Object, Boolean> condition, InputGesture wrappedGesture)
        {
            Contract.Require(condition, nameof(condition));
            Contract.Require(wrappedGesture, nameof(wrappedGesture));

            this.condition = condition;
            this.WrappedGesture = wrappedGesture;
        }

        /// <summary>
        /// Gets a value indicating whether this gesture is currently valid.
        /// </summary>
        /// <param name="source">The source of the command event.</param>
        /// <returns><see langword="true"/> if the gesture is valid; otherwise, <see langword="false"/>.</returns>
        public Boolean IsValid(Object source) => condition(source);

        /// <summary>
        /// Gets the gesture which is wrapped within the conditional gesture.
        /// </summary>
        public InputGesture WrappedGesture { get; }

        /// <inheritdoc/>
        public override Boolean MatchesKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data) =>
            IsValid(data.Source) ? WrappedGesture.MatchesKeyDown(device, key, modifiers, data) : false;

        /// <inheritdoc/>
        public override Boolean MatchesMouseClick(MouseDevice device, MouseButton button, RoutedEventData data) =>
            IsValid(data.Source) ? WrappedGesture.MatchesMouseClick(device, button, data) : false;

        /// <inheritdoc/>
        public override Boolean MatchesMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data) =>
            IsValid(data.Source) ? WrappedGesture.MatchesMouseDoubleClick(device, button, data) : false;

        /// <inheritdoc/>
        public override Boolean MatchesMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data) =>
            IsValid(data.Source) ? WrappedGesture.MatchesMouseWheel(device, x, y, data) : false;

        /// <inheritdoc/>
        public override Boolean MatchesGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data) =>
            IsValid(data.Source) ? WrappedGesture.MatchesGamePadButtonDown(device, button, repeat, data) : false;

        // State values.
        private readonly Func<Object, Boolean> condition;
    }
}
