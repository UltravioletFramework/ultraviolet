using System;

namespace Ultraviolet.Presentation.Animations
{
    partial class SimpleClockPool
    {
        /// <summary>
        /// Represents a pool of <see cref="SimpleClock"/> objects.
        /// </summary>
        private sealed class PoolImpl : UpfPool<SimpleClock>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PoolImpl"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            /// <param name="capacity">The pool's initial capacity.</param>
            /// <param name="watermark">The pool's high watermark value.</param>
            /// <param name="allocator">The pool's allocator function.</param>
            public PoolImpl(UltravioletContext uv, Int32 capacity, Int32 watermark, Func<SimpleClock> allocator)
                : base(uv, capacity, watermark, allocator) { }

            /// <inheritdoc/>
            protected override void OnCleaningUpObject(SimpleClock @object)
            {
                @object.Stop();
                base.OnCleaningUpObject(@object);
            }
        }
    }
}
