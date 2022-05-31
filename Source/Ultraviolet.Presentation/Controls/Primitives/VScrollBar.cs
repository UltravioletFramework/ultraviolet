using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Primitives.Templates.VScrollBar.xml")]
    public class VScrollBar : OrientedScrollBar
    {
        /// <summary>
        /// Initializes the <see cref="VScrollBar"/> type.
        /// </summary>
        static VScrollBar()
        {
            // Commands - vertical scroll
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.LineDownCommand, ExecutedLineDownCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Down, ModifierKeys.None, "Down"),
                new GamePadGesture(GamePadButton.LeftStickDown, 0, "LeftStickDown"));
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.LineUpCommand, ExecutedLineUpCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Up, ModifierKeys.None, "Up"),
                new GamePadGesture(GamePadButton.LeftStickUp, 0, "LeftStickUp"));
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.PageDownCommand, ExecutedPageDownCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.PageUpCommand, ExecutedPageUpCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.ScrollToBottomCommand, ExecutedScrollToBottomCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.Control, "Ctrl+End"));
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.ScrollToTopCommand, ExecutedScrollToTopCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(VScrollBar), ScrollBar.ScrollHereCommand, ExecutedScrollHereCommand, CanExecuteScrollHereCommand,
                null);

            // Commands - track
            CommandManager.RegisterClassBindings(typeof(VScrollBar), Track.IncreaseCommand, ExecutedTrackIncreaseCommand, CanExecuteTrackCommand);
            CommandManager.RegisterClassBindings(typeof(VScrollBar), Track.DecreaseCommand, ExecutedTrackDecreaseCommand, CanExecuteTrackCommand);
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
            var newValue = Math.Min(Maximum, Value + SmallChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.SmallIncrement);
            }
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a small amount in the vertical direction.
        /// </summary>
        public void LineUp()
        {
            var newValue = Math.Min(Maximum, Value - SmallChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.SmallDecrement);
            }
        }

        /// <summary>
        /// Increases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public void PageDown()
        {
            var newValue = Math.Min(Maximum, Value + LargeChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.LargeIncrement);
            }
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a large amount in the vertical direction.
        /// </summary>
        public void PageUp()
        {
            var newValue = Math.Min(Maximum, Value - LargeChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.LargeDecrement);
            }
        }

        /// <summary>
        /// Scrolls the scroll bar to its maximum value.
        /// </summary>
        public void ScrollToBottom()
        {
            if (Value != Maximum)
            {
                Value = Maximum;
                RaiseScrollEvent(ScrollEventType.Last);
            }
        }

        /// <summary>
        /// Scrolls the scroll bar to its minimum value.
        /// </summary>
        public void ScrollToTop()
        {
            if (Value != Minimum)
            {
                Value = Minimum;
                RaiseScrollEvent(ScrollEventType.First);
            }
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
        private static void ExecutedLineDownCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).LineDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineUpCommand"/> command.
        /// </summary>
        private static void ExecutedLineUpCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).LineUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageDownCommand"/> command.
        /// </summary>
        private static void ExecutedPageDownCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).PageDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageUpCommand"/> command.
        /// </summary>
        private static void ExecutedPageUpCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).PageUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToBottomCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToBottomCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).ScrollToBottom();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToTopCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToTopCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((VScrollBar)element).ScrollToTop();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollHereCommand"/> command.
        /// </summary>
        private static void ExecutedScrollHereCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var scrollBar = (VScrollBar)element;
            var scrollBarMin = scrollBar.Minimum;

            var newValue = scrollBar.lastRightClickedPoint.HasValue ?
                (scrollBar.Track?.ValueFromPoint(scrollBar.lastRightClickedPoint.Value) ?? scrollBarMin) : scrollBarMin;
            if (newValue != scrollBar.Value)
            {
                scrollBar.Value = newValue;
                scrollBar.RaiseScrollEvent(ScrollEventType.ThumbPosition);
            }
        }

        /// <summary>
        /// Executes the <see cref="Track.IncreaseCommand"/> command.
        /// </summary>
        private static void ExecutedTrackIncreaseCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (element is FrameworkElement fe)
                ScrollBar.PageDownCommand.Execute(fe.View, null, fe);
        }

        /// <summary>
        /// Executes the <see cref="Track.DecreaseCommand"/> command.
        /// </summary>
        private static void ExecutedTrackDecreaseCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (element is FrameworkElement fe)
                ScrollBar.PageUpCommand.Execute(fe.View, null, fe);
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = !((VScrollBar)element).IsPartOfScrollViewer;
        }

        /// <summary>
        /// Determines whether a track command can execute.
        /// </summary>
        private static void CanExecuteTrackCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ScrollBar.ScrollHereCommand"/> command can execute.
        /// </summary>
        private static void CanExecuteScrollHereCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }
                
        // State values.
        private Point2D? lastRightClickedPoint;
    }
}
