using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a collection of <see cref="UIView"/> objects.
    /// </summary>
    public sealed class UIViewCollection : IEnumerable<UIView>
    {
        /// <summary>
        /// Draws the views in the collection.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch)
        {
            Contract.Require(spriteBatch, "spriteBatch");

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

        /// <inheritdoc/>
        public LinkedList<UIView>.Enumerator GetEnumerator()
        {
            return views.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UIView> IEnumerable<UIView>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of views in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return views.Count; }
        }

        // State values.
        private readonly PooledLinkedList<UIView> views = 
            new PooledLinkedList<UIView>(8);
    }
}
