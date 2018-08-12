using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Primitives.Templates.HScrollBar.xml")]
    public class HScrollBar : OrientedScrollBar
    {
        /// <summary>
        /// Initializes the <see cref="HScrollBar"/> type.
        /// </summary>
        static HScrollBar()
        {
            // Commands - horizontal scroll
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.LineRightCommand, ExecutedLineRightCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Right, ModifierKeys.None, "Right"),
                new GamePadGesture(GamePadButton.LeftStickRight, 0, "LeftStickRight"));
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.LineLeftCommand, ExecutedLineLeftCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Left, ModifierKeys.None, "Left"),
                new GamePadGesture(GamePadButton.LeftStickLeft, 0, "LeftStickLeft"));
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.PageRightCommand, ExecutedPageRightCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.PageLeftCommand, ExecutedPageLeftCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.ScrollToRightEndCommand, ExecutedScrollToRightEndCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.ScrollToLeftEndCommand, ExecutedScrollToLeftEndCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.None, "Home"));

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(HScrollBar), ScrollBar.ScrollHereCommand, ExecutedScrollHereCommand, CanExecuteScrollHereCommand,
                null);

            // Commands - track
            CommandManager.RegisterClassBindings(typeof(HScrollBar), Track.IncreaseCommand, ExecutedTrackIncreaseCommand, CanExecuteTrackCommand);
            CommandManager.RegisterClassBindings(typeof(HScrollBar), Track.DecreaseCommand, ExecutedTrackDecreaseCommand, CanExecuteTrackCommand);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public HScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Increases the value of the scroll bar by a small amount in the horizontal direction.
        /// </summary>
        public void LineRight()
        {
            var newValue = Math.Min(Maximum, Value + SmallChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.SmallIncrement);
            }
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a small amount in the horizontal direction.
        /// </summary>
        public void LineLeft()
        {
            var newValue = Math.Min(Maximum, Value - SmallChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.SmallDecrement);
            }
        }

        /// <summary>
        /// Increases the value of the scroll bar by a large amount in the horizontal direction.
        /// </summary>
        public void PageRight()
        {
            var newValue = Math.Min(Maximum, Value + LargeChange);
            if (newValue != Value)
            {
                Value = newValue;
                RaiseScrollEvent(ScrollEventType.LargeIncrement);
            }
        }

        /// <summary>
        /// Decreases the value of the scroll bar by a large amount in the horizontal direction.
        /// </summary>
        public void PageLeft()
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
        public void ScrollToRightEnd()
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
        public void ScrollToLeftEnd()
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
        /// Exeuctes the <see cref="ScrollBar.LineRightCommand"/> command.
        /// </summary>
        private static void ExecutedLineRightCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).LineRight();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineLeftCommand"/> command.
        /// </summary>
        private static void ExecutedLineLeftCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).LineLeft();
        }
        
        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageRightCommand"/> command.
        /// </summary>
        private static void ExecutedPageRightCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).PageRight();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageLeftCommand"/> command.
        /// </summary>
        private static void ExecutedPageLeftCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).PageLeft();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToRightEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToRightEndCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).ScrollToRightEnd();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToLeftEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToLeftEndCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            ((HScrollBar)element).ScrollToLeftEnd();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollHereCommand"/> command.
        /// </summary>
        private static void ExecutedScrollHereCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var scrollBar = (HScrollBar)element;
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
                ScrollBar.PageRightCommand.Execute(fe.View, null, fe);
        }

        /// <summary>
        /// Executes the <see cref="Track.DecreaseCommand"/> command.
        /// </summary>
        private static void ExecutedTrackDecreaseCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (element is FrameworkElement fe)
                ScrollBar.PageLeftCommand.Execute(fe.View, null, fe);
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = !((HScrollBar)element).IsPartOfScrollViewer;
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
        private static void CanExecuteScrollHereCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }
        
        // State values.
        private Point2D? lastRightClickedPoint;
    }
}
