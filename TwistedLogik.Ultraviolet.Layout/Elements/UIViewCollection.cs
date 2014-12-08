using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a collection of <see cref="UIView"/> objects.
    /// </summary>
    public sealed partial class UIViewCollection : UltravioletResource, IEnumerable<UIView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIViewCollection"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UIViewCollection(UltravioletContext uv)
            : base(uv)
        {
            HookKeyboardEvents();
            HookMouseEvents();
        }

        /// <summary>
        /// Draws the views in the collection.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");

            if (Ultraviolet.GetPlatform().Windows.GetCurrent() != Window)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            foreach (var view in views)
            {
                view.Draw(time, spriteBatch);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Updates the views in the collection.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            HandleUserInput();

            foreach (var view in views)
            {
                view.Update(time);
            }
        }

        /// <summary>
        /// Adds a view to the collection.
        /// </summary>
        /// <param name="view">The view to add to the collection.</param>
        /// <returns><c>true</c> if the view was added to the collection; otherwise, <c>false</c>.</returns>
        public Boolean Add(UIView view)
        {
            Contract.Require(view, "view");

            if (view.Container == this)
                return false;

            if (view.Container != null)
                view.Container.Remove(view);

            views.AddLast(view);
            return true;
        }

        /// <summary>
        /// Removes a view from the collection.
        /// </summary>
        /// <param name="view">The view to remove from the collection.</param>
        /// <returns><c>true</c> if the view was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(UIView view)
        {
            Contract.Require(view, "view");

            if (views.Remove(view))
            {
                view.Container = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified view; otherwise, <c>false</c>.</returns>
        public Boolean Contains(UIView view)
        {
            Contract.Require(view, "view");

            return view.Container == this;
        }

        /// <summary>
        /// Moves the specified view to the front of the collection's stack.
        /// </summary>
        /// <param name="view">The view to move.</param>
        /// <returns><c>true</c> if the view was moved; otherwise, <c>false</c>.</returns>
        public Boolean BringToFront(UIView view)
        {
            Contract.Require(view, "view");

            if (views.Remove(view))
            {
                views.AddLast(view);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the specified view to the back of the collection's stack.
        /// </summary>
        /// <param name="view">The view to move.</param>
        /// <returns><c>true</c> if the view was moved; otherwise, false.</returns>
        public Boolean SendToBack(UIView view)
        {
            Contract.Require(view, "view");

            if (views.Remove(view))
            {
                views.AddFirst(view);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the view at the specified point on the screen.
        /// </summary>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The view at the specified point on the screen, or <c>null</c> if no view exists at that position.</returns>
        public UIView GetViewAtPoint(Int32 x, Int32 y)
        {
            var current = views.Last;
            while (current != null)
            {
                if (current.Value.Area.Contains(x, y))
                {
                    return current.Value;
                }
                current = current.Previous;
            }
            return null;
        }

        /// <summary>
        /// Gets the view at the specified point on the screen.
        /// </summary>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The view at the specified point on the screen, or <c>null</c> if no view exists at that position.</returns>
        public UIView GetViewAtPoint(Vector2 position)
        {
            return GetViewAtPoint((Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets or sets the window that owns the collection. If <c>null</c>, the primary window is used.
        /// </summary>
        public IUltravioletWindow Window
        {
            get { return this.window ?? Ultraviolet.GetPlatform().Windows.GetPrimary(); }
            set { this.window = value; }
        }

        /// <summary>
        /// Gets the number of views in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return views.Count; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                UnhookKeyboardEvents();
                UnhookMouseEvents();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Hooks into Ultraviolet's keyboard input events.
        /// </summary>
        private void HookKeyboardEvents()
        {
            // TODO
        }

        /// <summary>
        /// Hooks into Ultraviolet's mouse input events.
        /// </summary>
        private void HookMouseEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse             = input.GetMouse();
                mouse.ButtonPressed  += mouse_ButtonPressed;
                mouse.ButtonReleased += mouse_ButtonReleased;
            }
        }

        /// <summary>
        /// Unhooks from Ultraviolet's keyboard input events.
        /// </summary>
        private void UnhookKeyboardEvents()
        {
            // TODO
        }

        /// <summary>
        /// Unhooks from Ultraviolet's mouse input events.
        /// </summary>
        private void UnhookMouseEvents()
        {
            var input = Ultraviolet.GetInput();
            if (input.IsMouseSupported())
            {
                var mouse             = input.GetMouse();
                mouse.ButtonPressed  -= mouse_ButtonPressed;
                mouse.ButtonReleased -= mouse_ButtonReleased;
            }
        }

        /// <summary>
        /// Handles user input by raising input messages on the elements in the view.
        /// </summary>
        private void HandleUserInput()
        {
            if (Ultraviolet.GetInput().IsKeyboardSupported())
            {
                HandleKeyboardInput();
            }
            if (Ultraviolet.GetInput().IsMouseSupported())
            {
                HandleMouseInput();
            }
        }

        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        private void HandleKeyboardInput()
        {
            // TODO
        }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        private void HandleMouseInput()
        {
            // Determine which element is currently under the mouse cursor.
            var mouse     = Ultraviolet.GetInput().GetMouse();
            var mousePos  = mouse.Position;
            var mouseView = mouse.Window == Window ? GetViewAtPoint(mousePos) : null; 
            
            elementUnderMousePrev = elementUnderMouse;
            elementUnderMouse     = (mouseView == null) ? null : mouseView.GetElementAtScreenPoint(mousePos);

            // Handle mouse motion events
            if (elementUnderMouse != elementUnderMousePrev)
            {
                if (elementUnderMousePrev != null)
                    elementUnderMousePrev.OnMouseLeave(mouse);

                if (elementUnderMouse != null)
                    elementUnderMouse.OnMouseEnter(mouse);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonPressed"/> event.
        /// </summary>
        private void mouse_ButtonPressed(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            if (elementUnderMouse != null)
            {
                elementUnderMouse.OnMouseButtonPressed(device, button);
            }
        }

        /// <summary>
        /// Handles the <see cref="MouseDevice.ButtonReleased"/> event.
        /// </summary>
        private void mouse_ButtonReleased(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (window != Window)
                return;

            if (elementUnderMouse != null)
            {
                elementUnderMouse.OnMouseButtonReleased(device, button);
            }
        }

        // Property values.
        private IUltravioletWindow window;

        // State values.
        private readonly PooledLinkedList<UIView> views = 
            new PooledLinkedList<UIView>(8);

        // Input tracking values.
        private UIElement elementUnderMousePrev;
        private UIElement elementUnderMouse;
    }
}
