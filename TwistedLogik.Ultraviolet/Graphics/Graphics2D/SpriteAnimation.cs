using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a sequence of sprite frames.
    /// </summary>
    public sealed class SpriteAnimation
    {
        /// <summary>
        /// Initializes a new instance of the SpriteAnimation class.
        /// </summary>
        /// <param name="name">The animation's name.</param>
        /// <param name="repeat">The animation's repeat mode.</param>
        public SpriteAnimation(String name, SpriteAnimationRepeat repeat)
        {
            this.name = name;
            this.repeat = repeat;
        }

        /// <summary>
        /// Gets or sets the animation's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets a value indicating how the animation repeats after completing a sequence.
        /// </summary>
        public SpriteAnimationRepeat Repeat
        {
            get { return repeat; }
        }

        /// <summary>
        /// Gets the animation's sequence of frames.
        /// </summary>
        public SpriteAnimationFrameCollection Frames
        {
            get { return frames; }
        }

        /// <summary>
        /// Gets the default animation controller for this animation.
        /// </summary>
        public SpriteAnimationController Controller
        {
            get { return controller; }
        }

        // Property values.
        private readonly String name;
        private readonly SpriteAnimationRepeat repeat;
        private readonly SpriteAnimationFrameCollection frames = new SpriteAnimationFrameCollection();
        private readonly SpriteAnimationController controller = new SpriteAnimationController();
    }
}
