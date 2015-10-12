using System;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UIScreen"/> class that implement the <see cref="MessageBox"/> modal.
    /// </summary>
    /// <param name="owner">The screen that owns the message box.</param>
    /// <returns>The instance of <see cref="UIScreen"/> that was created.</returns>
    public delegate UIScreen MessageBoxScreenFactory(UIScreen owner);

    /// <summary>
    /// Represents a modal message box.
    /// </summary>
    public class MessageBox : Modal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBox"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the message box.</param>
        public MessageBox(UIScreen owner)
        {
            Contract.Require(owner, "owner");

            var uv = owner.Ultraviolet;
            var screenFactory = uv.TryGetFactoryMethod<MessageBoxScreenFactory>();
            if (screenFactory == null)
                throw new InvalidOperationException(PresentationStrings.FactoryMethodMissing.Format(typeof(MessageBoxScreenFactory).Name));

            screen = screenFactory(owner);

            if (screen == null)
                throw new InvalidOperationException(PresentationStrings.FactoryMethodInvalidResult.Format(typeof(MessageBoxScreenFactory).Name));
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        public static void Show(MessageBox mb, String text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        public static void Show(MessageBox mb, String text, String caption)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        public static void Show(MessageBox mb, String text, String caption, MessageBoxButton button)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        public static void Show(MessageBox mb, String text, String caption, MessageBoxButton button, MessageBoxImage image)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button was clicked by the user.</returns>
        public static Task<MessageBoxResult> ShowAsync(MessageBox mb, String text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button was clicked by the user.</returns>
        public static Task<MessageBoxResult> ShowAsync(MessageBox mb, String text, String caption)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button was clicked by the user.</returns>
        public static Task<MessageBoxResult> ShowAsync(MessageBox mb, String text, String caption, MessageBoxButton button)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button was clicked by the user.</returns>
        public static Task<MessageBoxResult> ShowAsync(MessageBox mb, String text, String caption, MessageBoxButton button, MessageBoxImage image)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value that specifies the default result.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button was clicked by the user.</returns>
        public static Task<MessageBoxResult> ShowAsync(MessageBox mb, String text, String caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override UIScreen Screen
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return screen;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(screen);
            }
            base.Dispose(disposing);
        }

        // Property values.
        private readonly UIScreen screen;
    }
}
