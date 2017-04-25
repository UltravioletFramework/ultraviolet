using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents a clock which tracks the playback state of a <see cref="Storyboard"/>.
    /// </summary>
    public sealed class StoryboardClock : Clock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardClock"/> class.
        /// </summary>
        internal StoryboardClock() { }

        /// <inheritdoc/>
        public override LoopBehavior LoopBehavior
        {
            get { return (Storyboard == null) ? LoopBehavior.None : Storyboard.LoopBehavior; }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get { return (Storyboard == null) ? TimeSpan.Zero : Storyboard.Duration; }
        }

        /// <summary>
        /// Gets the clock's associated storyboard.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return StoryboardInstance == null ? null : StoryboardInstance.Storyboard; }
        }

        /// <summary>
        /// Gets the clock's associated storyboard instance.
        /// </summary>
        public StoryboardInstance StoryboardInstance
        {
            get;
            internal set;
        }

        /// <inheritdoc/>
        protected override Boolean IsValid
        {
            get { return StoryboardInstance != null; }
        }
    }
}
