using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics2D
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
        protected override void OnCollectionItemAdded(Int32 index, SpriteFrame item)
        {
            Contract.Require(item, nameof(item));

            Duration += item.Duration;

            base.OnCollectionItemAdded(index, item);
        }

        /// <inheritdoc />
        protected override void OnCollectionItemRemoved(Int32 index, SpriteFrame item)
        {
            Duration -= item.Duration;

            base.OnCollectionItemRemoved(index, item);
        }
    }
}
