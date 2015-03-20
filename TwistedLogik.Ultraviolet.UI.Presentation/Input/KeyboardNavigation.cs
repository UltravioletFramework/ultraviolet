using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains methods for performing navigation with the keyboard.
    /// </summary>
    public static class KeyboardNavigation
    {
        /// <summary>
        /// Moves focus in the specified direction.
        /// </summary>
        /// <param name="current">The element at which to begin navigation.</param>
        /// <param name="direction">The direction in which to move keyboard focus.</param>
        /// <returns><c>true</c> if focus was moved; otherwise, <c>false</c>.</returns>
        internal static Boolean MoveFocus(DependencyObject current, FocusNavigationDirection direction)
        {
            Contract.Require(current, "current");

            var element = current as UIElement;
            if (element == null)
                return false;

            var view = element.View;
            if (view == null)
                return false;

            switch (direction)
            {
                case FocusNavigationDirection.Next:
                    return MoveNext(element);

                case FocusNavigationDirection.Previous:
                    return MovePrevious(element);

                case FocusNavigationDirection.Left:
                    return MoveLeft(element);

                case FocusNavigationDirection.Right:
                    return MoveRight(element);

                case FocusNavigationDirection.Up:
                    return MoveUp(element);

                case FocusNavigationDirection.Down:
                    return MoveDown(element);

                default:
                    throw new ArgumentException("direction");
            }
        }

        /// <summary>
        /// Identifies the TabIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty TabIndexProperty = DependencyProperty.Register("TabIndex", typeof(Int32), typeof(KeyboardNavigation),
            new DependencyPropertyMetadata(null, () => CommonBoxedValues.Int32.MaxValue, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the IsTabStop dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.Register("IsTabStop", typeof(Boolean), typeof(KeyboardNavigation),
            new DependencyPropertyMetadata(null, () => CommonBoxedValues.Boolean.True, DependencyPropertyOptions.None));

        /// <summary>
        /// Moves focus to the next tab stop after the specified element.
        /// </summary>
        private static Boolean MoveNext(UIElement current)
        {
            return false;
        }

        /// <summary>
        /// Moves focus to the previous tab stop before the specified element.
        /// </summary>
        private static Boolean MovePrevious(UIElement current)
        {
            return false;
        }

        /// <summary>
        /// Moves focus left from the specified element.
        /// </summary>
        private static Boolean MoveLeft(UIElement current)
        {
            return false;
        }

        /// <summary>
        /// Moves focus right from the specified element.
        /// </summary>
        private static Boolean MoveRight(UIElement current)
        {
            return false;
        }

        /// <summary>
        /// Moves focus up from the specified element.
        /// </summary>
        private static Boolean MoveUp(UIElement current)
        {
            return false;
        }

        /// <summary>
        /// Moves focus down from the specified element.
        /// </summary>
        private static Boolean MoveDown(UIElement current)
        {
            return false;
        }
    }
}
