using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a sprite animation's list of frames.
    /// </summary>
    public sealed class SpriteAnimationFrameCollection : ObservableList<SpriteFrame>
    {
        /// <summary>
        /// Gets the total duration of the animation's frames.
        /// </summary>
        public Int32 Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// Raises the Cleared event.
        /// </summary>
        protected override void OnCleared()
        {
            Duration = 0;

            base.OnCleared();
        }

        /// <summary>
        /// Raises the ItemAdded event.
        /// </summary>
        /// <param name="item">The item that was added to the list.</param>
        protected override void OnItemAdded(SpriteFrame item)
        {
            Contract.Require(item, "item");

            Duration += item.Duration;

            base.OnItemAdded(item);
        }

        /// <summary>
        /// Raises the ItemRemoved event.
        /// </summary>
        /// <param name="item">The item that was added to the list.</param>
        protected override void OnItemRemoved(SpriteFrame item)
        {
            Duration -= item.Duration;

            base.OnItemRemoved(item);
        }
    }
}
