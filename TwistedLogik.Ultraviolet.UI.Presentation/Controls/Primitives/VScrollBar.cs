using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.VScrollBar.xml")]
    public class VScrollBar : OrientedScrollBar
    {
        /// <summary>
        /// Initializes the <see cref="VScrollBar"/> type.
        /// </summary>
        static VScrollBar()
        {
            // Commands - vertical scroll
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.LineDownCommand, ExecutedLineDownCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Down, ModifierKeys.None, "Down"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.LineUpCommand, ExecutedLineUpCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Up, ModifierKeys.None, "Up"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.PageDownCommand, ExecutedPageDownCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.PageUpCommand, ExecutedPageUpCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollToBottomCommand, ExecutedScrollToBottomCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.Control, "Ctrl+End"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollToTopCommand, ExecutedScrollToTopCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollHereCommand, ExecutedScrollHereCommand, CanExecuteScrollHereCommand,
                null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public VScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Increases the value of the scroll bar by a small amount in the vertical direction.
        /// </summary>
        public void LineDown()
        {
            IncreaseSmall();
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a small amount in the vertical direction.
        /// </summary>
        public void LineUp()
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Increases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public void PageDown()
        {
            IncreaseLarge();
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public void PageUp()
        {
            DecreaseLarge();
        }

        /// <summary>
        /// Scrolls the scroll bar to its maximum value.
        /// </summary>
        public void ScrollToBottom()
        {
            Value = Maximum;
        }

        /// <summary>
        /// Scrolls the scroll bar to its minimum value.
        /// </summary>
        public void ScrollToTop()
        {
            Value = Minimum;
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Right)
            {
                lastRightClickedPoint = (Track == null) ? (Point2D?)null : Mouse.GetPosition(Track);
            }
            base.OnMouseUp(device, button, data);
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineDownCommand"/> command.
        /// </summary>
        private static void ExecutedLineDownCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).LineDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineUpCommand"/> command.
        /// </summary>
        private static void ExecutedLineUpCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).LineUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageDownCommand"/> command.
        /// </summary>
        private static void ExecutedPageDownCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).PageDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageUpCommand"/> command.
        /// </summary>
        private static void ExecutedPageUpCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).PageUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToBottomCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToBottomCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).ScrollToBottom();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToTopCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToTopCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((VScrollBar)element).ScrollToTop();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollHereCommand"/> command.
        /// </summary>
        private static void ExecutedScrollHereCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var scrollBar = (VScrollBar)element;
            var scrollBarMin = scrollBar.Minimum;
            scrollBar.Value = scrollBar.lastRightClickedPoint.HasValue ?
                (scrollBar.Track?.ValueFromPoint(scrollBar.lastRightClickedPoint.Value) ?? scrollBarMin) : scrollBarMin;
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = !((VScrollBar)element).IsPartOfScrollViewer;
        }

        /// <summary>
        /// Determines whether the <see cref="ScrollBar.ScrollHereCommand"/> command can execute.
        /// </summary>
        private static void CanExecuteScrollHereCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineUpButton.
        /// </summary>
        private void HandleClickLineUp(DependencyObject element, RoutedEventData data)
        {
            DecreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineDownButton.
        /// </summary>
        private void HandleClickLineDown(DependencyObject element, RoutedEventData data)
        {
            IncreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallIncrement);
        }
        
        // State values.
        private Point2D? lastRightClickedPoint;
    }
}
