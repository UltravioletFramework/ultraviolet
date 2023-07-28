using System;
using Ultraviolet.Core;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UIScreen"/> class that implement the <see cref="MessageBox"/> modal.
    /// </summary>
    /// <param name="mb">The message box that owns the screen.</param>
    /// <param name="owner">The screen that owns the message box.</param>
    /// <returns>The instance of <see cref="UIScreen"/> that was created.</returns>
    public delegate UIScreen MessageBoxScreenFactory(MessageBoxModal mb, UIScreen owner);

    /// <summary>
    /// Represents a modal message box.
    /// </summary>
    public class MessageBoxModal : Modal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxModal"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the message box.</param>
        public MessageBoxModal(UIScreen owner)
        {
            Contract.Require(owner, nameof(owner));

            var uv = owner.Ultraviolet;
            var screenFactory = uv.TryGetFactoryMethod<MessageBoxScreenFactory>();
            if (screenFactory == null)
                throw new InvalidOperationException(PresentationStrings.FactoryMethodMissing.Format(typeof(MessageBoxScreenFactory).Name));

            screen = screenFactory(this, owner);

            if (screen == null)
                throw new InvalidOperationException(PresentationStrings.FactoryMethodInvalidResult.Format(typeof(MessageBoxScreenFactory).Name));
        }

        /// <inheritdoc/>
        public override UIScreen Screen => screen;

        /// <summary>
        /// Prepares the message box to be displayed.
        /// </summary>
        /// <param name="text">The message box's text.</param>
        /// <param name="caption">The message box's caption.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies the set of buttons to display.</param>
        /// <param name="image">A <see cref="MessageBoxImage"/> value that specifies the image to display.</param>
        /// <param name="defaultResult">A <see cref="MessageBoxResult"/> value that specifies the message box's default option.</param>
        internal void Prepare(String text, String caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            var vm = screen.View.GetViewModel<MessageBoxViewModel>();
            if (vm != null)
            {
                vm.Prepare(text, caption, button, image, defaultResult);
            }
        }

        /// <summary>
        /// Gets the message box's text.
        /// </summary>
        internal String Text
        {
            get
            {
                var vm = Screen.View.GetViewModel<MessageBoxViewModel>();
                if (vm != null)
                {
                    return vm.Text;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the message box's caption.
        /// </summary>
        internal String Caption
        {
            get
            {
                var vm = Screen.View.GetViewModel<MessageBoxViewModel>();
                if (vm != null)
                {
                    return vm.Caption;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxButton"/> value that specifies which buttons are being displayed.
        /// </summary>
        internal MessageBoxButton Button
        {
            get
            {
                var vm = Screen.View.GetViewModel<MessageBoxViewModel>();
                if (vm != null)
                {
                    return vm.Button;
                }
                return MessageBoxButton.OK;
            }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxImage"/> value that specifies which image is being displayed.
        /// </summary>
        internal MessageBoxImage Image
        {
            get
            {
                var vm = Screen.View.GetViewModel<MessageBoxViewModel>();
                if (vm != null)
                {
                    return vm.Image;
                }
                return MessageBoxImage.None;
            }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxResult"/> value that specifies the message box's default option.
        /// </summary>
        internal MessageBoxResult DefaultResult
        {
            get
            {
                var vm = Screen.View.GetViewModel<MessageBoxViewModel>();
                if (vm != null)
                {
                    return vm.DefaultResult;
                }
                return MessageBoxResult.None;
            }
        }

        /// <summary>
        /// Gets or sets the message box's result value.
        /// </summary>
        internal MessageBoxResult MessageBoxResult
        {
            get => messageBoxResult;
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (messageBoxResult != value)
                {
                    messageBoxResult = value;
                    switch (value)
                    {
                        case MessageBoxResult.OK:
                        case MessageBoxResult.Yes:
                            Close(true);
                            break;

                        case MessageBoxResult.No:
                            Close(false);
                            break;

                        default:
                            Close();
                            break;
                    }
                }
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

        /// <inheritdoc/>
        protected override void OnOpening()
        {
            messageBoxResult = MessageBoxResult.None;
            base.OnOpening();
        }

        // Property values.
        private readonly UIScreen screen;
        private MessageBoxResult messageBoxResult;
    }
}
