using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the list of frames that constitutes a <see cref="SpriteAnimation"/>.
    /// </summary>
    public sealed class SpriteAnimationFrameCollection : ObservableList<SpriteFrame>
    {
        /// <summary>
        /// Gets the total duration of the animation's frames in milliseconds.
        /// </summary>
        public Int32 Duration
        {
            get;
            private set;
        }

        /// <inheritdoc />
        protected override void OnCollectionReset()
        {
            var recalculateDuration = 0;
            foreach (var item in this)
            {
                recalculateDuration += item.Duration;
            }
            Duration = recalculateDuration;

            base.OnCollectionReset();
        }

        /// <inheritdoc />
        protected override void OnCollectionItemAdded(SpriteFrame item)
        {
            Contract.Require(item, "item");

            Duration += item.Duration;

            base.OnCollectionItemAdded(item);
        }

        /// <inheritdoc />
        protected override void OnCollectionItemRemoved(SpriteFrame item)
        {
            Duration -= item.Duration;

            base.OnCollectionItemRemoved(item);
        }
    }
}
