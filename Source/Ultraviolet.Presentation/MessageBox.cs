using System;
using System.Threading.Tasks;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a modal message box.
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        public static void Show(MessageBoxModal mb, String text)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, null, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

            Modal.ShowDialog(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        public static void Show(MessageBoxModal mb, String text, String caption)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

            Modal.ShowDialog(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        public static void Show(MessageBoxModal mb, String text, String caption, MessageBoxButton button)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, MessageBoxImage.None, MessageBoxResult.None);

            Modal.ShowDialog(mb);
        }
        
        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        public static void Show(MessageBoxModal mb, String text, String caption, MessageBoxButton button, MessageBoxImage image)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, image, MessageBoxResult.None);

            Modal.ShowDialog(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value that specifies the message box's default option.</param>
        public static void Show(MessageBoxModal mb, String text, String caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, image, defaultResult);

            Modal.ShowDialog(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <returns>A <see cref="ModalTask{T}"/> which returns a <see cref="MessageBoxResult"/> value that 
        /// specifies which message box button was clicked by the user.</returns>
        public static ModalTask<MessageBoxResult> ShowAsync(MessageBoxModal mb, String text)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, null, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

            return WrapShowDialogAsync(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <returns>A <see cref="ModalTask{T}"/> which returns a <see cref="MessageBoxResult"/> value that 
        /// specifies which message box button was clicked by the user.</returns>
        public static ModalTask<MessageBoxResult> ShowAsync(MessageBoxModal mb, String text, String caption)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None);

            return WrapShowDialogAsync(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <returns>A <see cref="ModalTask{T}"/> which returns a <see cref="MessageBoxResult"/> value that 
        /// specifies which message box button was clicked by the user.</returns>
        public static ModalTask<MessageBoxResult> ShowAsync(MessageBoxModal mb, String text, String caption, MessageBoxButton button)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, MessageBoxImage.None, MessageBoxResult.None);

            return WrapShowDialogAsync(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <returns>A <see cref="ModalTask{T}"/> which returns a <see cref="MessageBoxResult"/> value that
        /// specifies which message box button was clicked by the user.</returns>
        public static ModalTask<MessageBoxResult> ShowAsync(MessageBoxModal mb, String text, String caption, MessageBoxButton button, MessageBoxImage image)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, image, MessageBoxResult.None);

            return WrapShowDialogAsync(mb);
        }

        /// <summary>
        /// Displays the specified message box as a modal dialog and returns the result.
        /// </summary>
        /// <param name="mb">The message box to display.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="caption">The caption to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value that specifies the message box's default option.</param>
        /// <returns>A <see cref="ModalTask{T}"/> which returns a <see cref="MessageBoxResult"/> value that 
        /// specifies which message box button was clicked by the user.</returns>
        public static ModalTask<MessageBoxResult> ShowAsync(MessageBoxModal mb, String text, String caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            Contract.Require(mb, nameof(mb));
            
            mb.Prepare(text, caption, button, image, defaultResult);

            return WrapShowDialogAsync(mb);
        }
        
        /// <summary>
        /// Opens the specified <see cref="MessageBox"/> as a modal dialog and returns a <see cref="Task{MessageBoxResult}"/>
        /// that represents the result of the message box.
        /// </summary>
        private static ModalTask<MessageBoxResult> WrapShowDialogAsync(MessageBoxModal mb)
        {
            var taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();
            var task = Modal.ShowDialogAsync(mb);
            task.ContinueWith(dialogResult => taskCompletionSource.SetResult(mb.MessageBoxResult));

            return new ModalTask<MessageBoxResult>(taskCompletionSource.Task);
        }        
    }
}
