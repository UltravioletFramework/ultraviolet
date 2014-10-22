using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a window's stack of active screens.
    /// </summary>
    public sealed partial class UIScreenStack : IEnumerable<UIScreen>
    {
        /// <summary>
        /// Represents a screen that is waiting for another screen to close before being opened.
        /// </summary>
        private struct PendingOpening
        {
            public PendingOpening(UIScreen opening, UIScreen closing, TimeSpan duration)
            {
                this.Opening  = opening;
                this.Closing  = closing;
                this.Duration = duration;
            }
            public readonly UIScreen Opening;
            public readonly UIScreen Closing;
            public readonly TimeSpan Duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIScreenStack"/> class.
        /// </summary>
        /// <param name="window">The window that owns the screen stack.</param>
        internal UIScreenStack(IUltravioletWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Clears the stack.
        /// </summary>
        public void Clear()
        {
            foreach (var screen in screens)
            {
                screen.Window = null;
                screen.Close(TimeSpan.Zero);
            }
            screens.Clear();
            pendingOpenings.Clear();
        }

        /// <summary>
        /// Draws the screens in the stack.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the screen stack's screens.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");

            if (screens.Count > 0)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                for (var screen = FindFirstScreenToRender(); screen != null; screen = screen.Next)
                {
                    screen.Value.Draw(time, spriteBatch);
                }
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Updates the screen stack's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            LinkedListNode<UIScreen> current, next;
            for (current = screens.First; current != null; current = next)
            {
                next = current.Next;
                current.Value.Update(time);
                RemoveIfClosed(current);
            }
            UpdatePendingOpenings();
        }

        /// <summary>
        /// Brings the specified screen to the front of the stack.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to bring to the front of the stack.</param>
        public void BringToFront(UIScreen screen)
        {
            Contract.Require(screen, "screen");

            if (!screens.Remove(screen))
                throw new ArgumentException(UltravioletStrings.ScreenNotInStack);

            screens.AddLast(screen);
        }

        /// <summary>
        /// Sends the specified screen to the back of the stack.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to send to the back of the stack.</param>
        public void SendToBack(UIScreen screen)
        {
            Contract.Require(screen, "screen");

            if (!screens.Remove(screen))
                throw new ArgumentException(UltravioletStrings.ScreenNotInStack);

            screens.AddFirst(screen);
        }

        /// <summary>
        /// Opens the specified screen on the top of the stack.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to open.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        public void Open(UIScreen screen, TimeSpan? duration = null)
        {
            Contract.Require(screen, "screen");

            if (screens.Contains(screen))
                throw new ArgumentException(UltravioletStrings.ScreenAlreadyInStack);

            screens.AddLast(screen);
            screen.Window = window;
            screen.Close(TimeSpan.Zero);
            screen.Open(duration);
        }

        /// <summary>
        /// Opens the specified screen on the top of the stack.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to open.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation.</returns>
        public Task OpenAsync(UIScreen screen, TimeSpan? duration = null)
        {
            Contract.Require(screen, "screen");

            if (screens.Contains(screen))
                throw new ArgumentException(UltravioletStrings.ScreenAlreadyInStack);

            screens.AddLast(screen);
            screen.Window = window;
            screen.Close(TimeSpan.Zero);
            return screen.OpenAsync(duration);
        }

        /// <summary>
        /// Opens the specified screen and positions it immediately beneath a second
        /// screen which is already on the screen stack.
        /// </summary>
        /// <param name="screenToOpen">The <see cref="UIScreen"/> to open.</param>
        /// <param name="screenAbove">The <see cref="UIScreen"/> below which to open <paramref name="screenToOpen"/>.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        public void OpenBelow(UIScreen screenToOpen, UIScreen screenAbove, TimeSpan? duration = null)
        {
            Contract.Require(screenToOpen, "screenToOpen");
            Contract.Require(screenAbove, "screenAbove");

            if (screens.Contains(screenToOpen))
                throw new ArgumentException(UltravioletStrings.ScreenAlreadyInStack);

            var node = screens.Find(screenAbove);
            if (node == null)
                throw new ArgumentException(UltravioletStrings.ScreenNotInStack);

            screens.AddBefore(node, screenToOpen);
            screenToOpen.Window = window;
            screenToOpen.Close(TimeSpan.Zero);
            screenToOpen.Open(duration);
        }

        /// <summary>
        /// Opens the specified screen and positions it immediately beneath a second
        /// screen which is already on the screen stack.
        /// </summary>
        /// <param name="screenToOpen">The <see cref="UIScreen"/> to open.</param>
        /// <param name="screenAbove">The <see cref="UIScreen"/> below which to open <paramref name="screenToOpen"/>.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation.</returns>
        public Task OpenBelowAsync(UIScreen screenToOpen, UIScreen screenAbove, TimeSpan? duration = null)
        {
            Contract.Require(screenToOpen, "screenToOpen");
            Contract.Require(screenAbove, "screenAbove");

            if (screens.Contains(screenToOpen))
                throw new ArgumentException(UltravioletStrings.ScreenAlreadyInStack);

            var node = screens.Find(screenAbove);
            if (node == null)
                throw new ArgumentException(UltravioletStrings.ScreenNotInStack);

            screens.AddBefore(node, screenToOpen);
            screenToOpen.Window = window;
            screenToOpen.Close(TimeSpan.Zero);
            return screenToOpen.OpenAsync(duration);
        }

        /// <summary>
        /// Closes the specified screen.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to close.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns><c>true</c> if the screen was closed; otherwise, <c>false</c>.</returns>
        public Boolean Close(UIScreen screen, TimeSpan? duration = null)
        {
            Contract.Require(screen, "screen");

            if (screens.Contains(screen))
            {
                screen.Close(duration);
                RemoveIfClosed(screen);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Asynchronously closes the specified screen.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to close.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A task which represents the asynchronous operation.</returns>
        public Task CloseAsync(UIScreen screen, TimeSpan? duration = null)
        {
            Contract.Require(screen, "screen");

            if (screens.Contains(screen))
            {
                var task = screen.CloseAsync(duration);
                RemoveIfClosed(screen);
                return task;
            }
            return TaskUtil.FromResult<Boolean>(false);
        }

        /// <summary>
        /// Closes any screens which match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate with which to determine which screens to close.</param>
        /// <param name="duration">The amount of time over which to transition the screens' states, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns><c>true</c> if any screens were closed; otherwise, <c>false</c>.</returns>
        public Boolean Close(Func<UIScreen, Boolean> predicate, TimeSpan? duration = null)
        {
            Contract.Require(predicate, "predicate");

            var any = false;

            LinkedListNode<UIScreen> current, next;
            for (current = screens.First; current != null; current = next)
            {
                next = current.Next;
                var screen = current.Value;

                if (predicate(screen))
                {
                    any = true;
                    screen.Close(duration);
                    RemoveIfClosed(current);
                }
            }

            return any;
        }

        /// <summary>
        /// Asynchronously closes any screens which match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate with which to determine which screens to close.</param>
        /// <param name="duration">The amount of time over which to transition the screens' states, or
        /// <c>null</c> to use the default transition time.</param>
        /// <returns>A collection of <see cref="Task"/> objects representing the asynchronous operations.</returns>
        public IEnumerable<Task> CloseAsync(Func<UIScreen, Boolean> predicate, TimeSpan? duration = null)
        {
            Contract.Require(predicate, "predicate");

            var tasks = default(List<Task>);

            LinkedListNode<UIScreen> current, next;
            for (current = screens.First; current != null; current = next)
            {
                next = current.Next;
                var screen = current.Value;

                if (predicate(screen))
                {
                    tasks = tasks ?? new List<Task>();
                    tasks.Add(screen.CloseAsync(duration));
                    RemoveIfClosed(current);
                }
            }

            return tasks ?? Enumerable.Empty<Task>();
        }

        /// <summary>
        /// Closes the specified screen, then opens another screen once the first screen has finished closing.
        /// </summary>
        /// <param name="closing">The <see cref="UIScreen"/> to close.</param>
        /// <param name="opening">The <see cref="UIScreen"/> to open.</param>
        /// <returns><c>true</c> if the first screen needed to be closed; otherwise, <c>false</c>.</returns>
        public Boolean CloseThenOpen(UIScreen closing, UIScreen opening)
        {
            Contract.Require(closing, "closing");
            Contract.Require(opening, "opening");

            if (!Contains(closing))
                throw new InvalidOperationException(UltravioletStrings.ScreenNotInStack);

            return CloseThenOpenInternal(closing, closing.DefaultCloseTransitionDuration, opening, opening.DefaultOpenTransitionDuration);
        }

        /// <summary>
        /// Closes the specified screen, then opens another screen once the first screen has finished closing.
        /// </summary>
        /// <param name="closing">The <see cref="UIScreen"/> to close.</param>
        /// <param name="closingDuration">The amount of time over which to close the screen.</param>
        /// <param name="opening">The <see cref="UIScreen"/> to open.</param>
        /// <param name="openingDuration">The amount of time over which to open the screen.</param>
        /// <returns><c>true</c> if the first screen needed to be closed; otherwise, <c>false</c>.</returns>
        public Boolean CloseThenOpen(UIScreen closing, TimeSpan closingDuration, UIScreen opening, TimeSpan openingDuration)
        {
            Contract.Require(closing, "closing");
            Contract.Require(opening, "opening");

            if (!Contains(closing))
                throw new InvalidOperationException(UltravioletStrings.ScreenNotInStack);

            return CloseThenOpenInternal(closing, closingDuration, opening, openingDuration);
        }

        /// <summary>
        /// Gets a value indicating whether the screen stack contains the specified screen.
        /// </summary>
        /// <param name="screen">The <see cref="UIScreen"/> to evaluate.</param>
        /// <returns><c>true</c> if the screen stack contains the specified screen; otherwise, <c>false</c>.</returns>
        public Boolean Contains(UIScreen screen)
        {
            Contract.Require(screens, "screens");

            return screens.Contains(screen);
        }

        /// <summary>
        /// Retrieves the topmost screen on the stack.
        /// </summary>
        /// <returns>The topmost <see cref="UIScreen"/> on the stack.</returns>
        public UIScreen Peek()
        {
            if (screens.Count == 0)
                throw new InvalidOperationException(UltravioletStrings.StackIsEmpty);

            return screens.Last.Value;
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<UIScreen> IEnumerable<UIScreen>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public LinkedList<UIScreen>.Enumerator GetEnumerator()
        {
            return screens.GetEnumerator();
        }

        /// <summary>
        /// Gets the window that owns the screen stack.
        /// </summary>
        public IUltravioletWindow Window
        {
            get { return window; }
        }

        /// <summary>
        /// Gets the number of screens on the stack.
        /// </summary>
        public Int32 Count
        {
            get { return screens.Count; }
        }

        /// <summary>
        /// Searches the screen stack for the first screen that should be rendered.
        /// This takes into consideration which screens are marked opaque; screen beneath opaque screens
        /// are not rendered as a performance optimization.
        /// </summary>
        /// <returns>The linked list node that contains the first screen in the stack to render.</returns>
        private LinkedListNode<UIScreen> FindFirstScreenToRender()
        {
            for (var screen = screens.Last; screen != null; screen = screen.Previous)
            {
                if (!screen.Value.IsTransitioning && screen.Value.IsOpaque)
                {
                    return screen;
                }
            }
            return screens.First;
        }

        /// <summary>
        /// Removes the specified screen from the screen list, if the screen is closed.
        /// </summary>
        private void RemoveIfClosed(UIScreen screen)
        {
            if (screen.State == UIPanelState.Closed)
            {
                screen.Window = null;
                screens.Remove(screen);
            }
        }

        /// <summary>
        /// Removes the specified screen from the screen list, if the screen is closed.
        /// </summary>
        private void RemoveIfClosed(LinkedListNode<UIScreen> screen)
        {
            if (screen.Value.State == UIPanelState.Closed)
            {
                screen.Value.Window = null;
                screens.Remove(screen);
            }
        }

        /// <summary>
        /// Updates the stack's pending openings.
        /// </summary>
        private void UpdatePendingOpenings()
        {
            if (pendingOpenings.Count == 0)
                return;

            var current = pendingOpenings.First;
            var next    = current.Next;

            while (current != null)
            {
                next = current.Next;

                if (current.Value.Closing.Window != window)
                {
                    Open(current.Value.Opening, current.Value.Duration);
                    pendingOpenings.Remove(current);
                }
                else
                {
                    switch (current.Value.Closing.State)
                    {
                        case UIPanelState.Open:
                        case UIPanelState.Opening:
                            pendingOpenings.Remove(current);
                            break;
                    }
                }

                current = next;
            }
        }

        /// <summary>
        /// Closes the specified screen, then opens another screen once the first screen has finished closing.
        /// </summary>
        /// <param name="closing">The screen to close.</param>
        /// <param name="closingDuration">The amount of time over which to close the screen.</param>
        /// <param name="opening">The screen to open.</param>
        /// <param name="openingDuration">The amount of time over which to open the screen.</param>
        /// <returns><c>true</c> if the first screen needed to be closed; otherwise, <c>false</c>.</returns>
        private Boolean CloseThenOpenInternal(UIScreen closing, TimeSpan closingDuration, UIScreen opening, TimeSpan openingDuration)
        {
            if (closing.State == UIPanelState.Closed)
            {
                Open(opening);
                return false;
            }

            var pending = new PendingOpening(opening, closing, openingDuration);
            pendingOpenings.AddLast(pending);

            Close(closing, closingDuration);

            return true;
        }

        // Property values.
        private readonly IUltravioletWindow window;

        // The current list of active screens.
        private readonly PooledLinkedList<UIScreen> screens = 
            new PooledLinkedList<UIScreen>();

        // The list of screens that are waiting to be opened.
        private readonly PooledLinkedList<PendingOpening> pendingOpenings =
            new PooledLinkedList<PendingOpening>(1);
    }
}
