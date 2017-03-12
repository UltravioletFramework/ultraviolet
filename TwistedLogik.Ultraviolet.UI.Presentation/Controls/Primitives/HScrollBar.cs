using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.HScrollBar.xml")]
    public class HScrollBar : OrientedScrollBar
    {
        /// <summary>
        /// Initializes the <see cref="HScrollBar"/> type.
        /// </summary>
        static HScrollBar()
        {
            // Commands - horizontal scroll
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.LineRightCommand, ExecutedLineRightCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Right, ModifierKeys.None, "Right"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.LineLeftCommand, ExecutedLineLeftCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Left, ModifierKeys.None, "Left"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.PageRightCommand, ExecutedPageRightCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.PageLeftCommand, ExecutedPageLeftCommand, CanExecuteScrollCommand,
                null);
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollToRightEndCommand, ExecutedScrollToRightEndCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollToLeftEndCommand, ExecutedScrollToLeftEndCommand, CanExecuteScrollCommand,
                new KeyGesture(Key.Home, ModifierKeys.None, "Home"));

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(ScrollBar), ScrollBar.ScrollHereCommand, ExecutedScrollHereCommand, CanExecuteScrollHereCommand,
                null);
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
        private static void ExecutedLineRightCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((HScrollBar)element).IncreaseSmall();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineLeftCommand"/> command.
        /// </summary>
        private static void ExecutedLineLeftCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((HScrollBar)element).DecreaseSmall();
        }
        
        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageRightCommand"/> command.
        /// </summary>
        private static void ExecutedPageRightCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((HScrollBar)element).IncreaseLarge();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageLeftCommand"/> command.
        /// </summary>
        private static void ExecutedPageLeftCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            ((HScrollBar)element).DecreaseLarge();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToRightEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToRightEndCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var scrollBar = (HScrollBar)element;
            scrollBar.Value = scrollBar.Maximum;
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToLeftEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToLeftEndCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var scrollBar = (HScrollBar)element;
            scrollBar.Value = scrollBar.Minimum;
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollHereCommand"/> command.
        /// </summary>
        private static void ExecutedScrollHereCommand(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var scrollBar = (HScrollBar)element;
            var scrollBarMin = scrollBar.Minimum;
            scrollBar.Value = scrollBar.lastRightClickedPoint.HasValue ?
                (scrollBar.Track?.ValueFromPoint(scrollBar.lastRightClickedPoint.Value) ?? scrollBarMin) : scrollBarMin;
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = !((HScrollBar)element).IsPartOfScrollViewer;
        }

        /// <summary>
        /// Determines whether the <see cref="ScrollBar.ScrollHereCommand"/> command can execute.
        /// </summary>
        private static void CanExecuteScrollHereCommand(DependencyObject element, ICommand command, Object paramter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineLeftButton.
        /// </summary>
        private void HandleClickLineLeft(DependencyObject element, RoutedEventData data)
        {
            DecreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineRightButton.
        /// </summary>
        private void HandleClickLineRight(DependencyObject element, RoutedEventData data)
        {
            IncreaseSmall();
            RaiseScrollEvent(ScrollEventType.LargeDecrement);
        }

        // State values.
        private Point2D? lastRightClickedPoint;
    }
}
