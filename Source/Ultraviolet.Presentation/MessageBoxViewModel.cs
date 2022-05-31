using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// The view model for the <see cref="MessageBoxScreen"/> class.
    /// </summary>
    [ViewModelWrapper(typeof(MessageBoxViewModel_Impl))]
    public class MessageBoxViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxViewModel"/> class.
        /// </summary>
        /// <param name="mb">The message box that owns the screen.</param>
        protected internal MessageBoxViewModel(MessageBoxModal mb)
        {
            Contract.Require(mb, nameof(mb));

            this.mb = mb;
        }

        /// <summary>
        /// Closes the dialog box when the "Yes" button is clicked.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event data for this event invocation.</param>
        public void HandleClickYes(DependencyObject dobj, RoutedEventData data)
        {
            MessageBoxResult = MessageBoxResult.Yes;
        }

        /// <summary>
        /// Closes the dialog box when the "No" button is clicked.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event data for this event invocation.</param>
        public void HandleClickNo(DependencyObject dobj, RoutedEventData data)
        {
            MessageBoxResult = MessageBoxResult.No;
        }

        /// <summary>
        /// Closes the dialog box when the "OK" button is clicked.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event data for this event invocation.</param>
        public void HandleClickOK(DependencyObject dobj, RoutedEventData data)
        {
            MessageBoxResult = MessageBoxResult.OK;
        }

        /// <summary>
        /// Closes the dialog box when the "Cancel" button is clicked.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        /// <param name="data">The routed event data for this event invocation.</param>
        public void HandleClickCancel(DependencyObject dobj, RoutedEventData data)
        {
            MessageBoxResult = MessageBoxResult.Cancel;
        }

        /// <summary>
        /// Gets the message box's text.
        /// </summary>
        public String Text
        {
            get { return text; }
        }

        /// <summary>
        /// Gets the message box's caption.
        /// </summary>
        public String Caption
        {
            get { return caption; }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxButton"/> value that specifies which buttons are being displayed.
        /// </summary>
        public MessageBoxButton Button
        {
            get { return button; }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxImage"/> value that specifies which image is being displayed.
        /// </summary>
        public MessageBoxImage Image
        {
            get { return image; }
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxResult"/> value that specifies the message box's default option.
        /// </summary>
        public MessageBoxResult DefaultResult
        {
            get { return defaultResult; }
        }

        /// <summary>
        /// Gets or sets the message box's result value.
        /// </summary>
        public MessageBoxResult MessageBoxResult
        {
            get { return mb.MessageBoxResult; }
            set { mb.MessageBoxResult = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the message box has valid text.
        /// </summary>
        public Boolean HasText
        {
            get { return !String.IsNullOrEmpty(Text); }
        }

        /// <summary>
        /// Gets a value indicating whether the message box has a valid caption.
        /// </summary>
        public Boolean HasCaption
        {
            get { return !String.IsNullOrEmpty(Caption); }
        }

        /// <summary>
        /// Gets a value indicating whether the message box has a valid image.
        /// </summary>
        public Boolean HasImage
        {
            get { return Image != MessageBoxImage.None; }
        }

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
            this.text = text;
            this.caption = caption;
            this.button = button;
            this.image = image;
            this.defaultResult = defaultResult;

            UpdateImage();
            UpdateButtons();
            UpdateDefaultButton();
        }
        
        /// <summary>
        /// Updates the message box's image.
        /// </summary>
        private void UpdateImage()
        {
            if (PART_Image != null)
            {
                PART_Image.Classes.Clear();

                switch (image)
                {
                    case MessageBoxImage.Hand:
                        PART_Image.Classes.Add("msgbox-img");
                        PART_Image.Classes.Add("msgbox-img-hand");
                        break;

                    case MessageBoxImage.Question:
                        PART_Image.Classes.Add("msgbox-img");
                        PART_Image.Classes.Add("msgbox-img-question");
                        break;

                    case MessageBoxImage.Exclamation:
                        PART_Image.Classes.Add("msgbox-img");
                        PART_Image.Classes.Add("msgbox-img-exclamation");
                        break;

                    case MessageBoxImage.Asterisk:
                        PART_Image.Classes.Add("msgbox-img");
                        PART_Image.Classes.Add("msgbox-img-asterisk");
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the message box's visible buttons.
        /// </summary>
        private void UpdateButtons()
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    SetElementVisibility(PART_ButtonYes, false);
                    SetElementVisibility(PART_ButtonNo, false);
                    SetElementVisibility(PART_ButtonOK, true);
                    SetElementVisibility(PART_ButtonCancel, false);
                    break;

                case MessageBoxButton.OKCancel:
                    SetElementVisibility(PART_ButtonYes, false);
                    SetElementVisibility(PART_ButtonNo, false);
                    SetElementVisibility(PART_ButtonOK, true);
                    SetElementVisibility(PART_ButtonCancel, true);
                    break;

                case MessageBoxButton.YesNoCancel:
                    SetElementVisibility(PART_ButtonYes, true);
                    SetElementVisibility(PART_ButtonNo, true);
                    SetElementVisibility(PART_ButtonOK, false);
                    SetElementVisibility(PART_ButtonCancel, true);
                    break;

                case MessageBoxButton.YesNo:
                    SetElementVisibility(PART_ButtonYes, true);
                    SetElementVisibility(PART_ButtonNo, true);
                    SetElementVisibility(PART_ButtonOK, false);
                    SetElementVisibility(PART_ButtonCancel, false);
                    break;
            }
        }

        /// <summary>
        /// Updates the message box's default button.
        /// </summary>
        private void UpdateDefaultButton()
        {
            if (PART_ButtonOK != null)
                PART_ButtonOK.IsDefault = false;

            if (PART_ButtonCancel != null)
                PART_ButtonCancel.IsDefault = false;

            if (PART_ButtonYes != null)
                PART_ButtonYes.IsDefault = false;

            if (PART_ButtonNo != null)
                PART_ButtonNo.IsDefault = false;

            switch (defaultResult)
            {
                case MessageBoxResult.OK:
                    if (PART_ButtonOK != null)
                        PART_ButtonOK.IsDefault = true;
                    break;

                case MessageBoxResult.Cancel:
                    if (PART_ButtonCancel != null)
                        PART_ButtonCancel.IsDefault = true;
                    break;

                case MessageBoxResult.Yes:
                    if (PART_ButtonYes != null)
                        PART_ButtonYes.IsDefault = true;
                    break;

                case MessageBoxResult.No:
                    if (PART_ButtonNo != null)
                        PART_ButtonNo.IsDefault = true;
                    break;

                default:
                    switch (button)
                    {
                        case MessageBoxButton.OK:
                        case MessageBoxButton.OKCancel:
                            if (PART_ButtonOK != null)
                                PART_ButtonOK.IsDefault = true;
                            break;

                        case MessageBoxButton.YesNoCancel:
                        case MessageBoxButton.YesNo:
                            if (PART_ButtonYes != null)
                                PART_ButtonYes.IsDefault = true;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets the visibility of the specified element.
        /// </summary>
        private void SetElementVisibility(UIElement element, Boolean visible)
        {
            if (element == null)
                return;

            element.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        
        // State values.
        private readonly MessageBoxModal mb;

        // Property values.
        private String text;
        private String caption;
        private MessageBoxButton button;
        private MessageBoxImage image;
        private MessageBoxResult defaultResult;

        // Component references.
        private readonly Button PART_ButtonYes = null;
        private readonly Button PART_ButtonNo = null;
        private readonly Button PART_ButtonOK = null;
        private readonly Button PART_ButtonCancel = null;
        private readonly UIElement PART_Image = null;
    }
}
