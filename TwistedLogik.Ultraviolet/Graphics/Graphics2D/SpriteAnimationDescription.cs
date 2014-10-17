using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a SpriteAnimation object.
    /// </summary>
    public class SpriteAnimationDescription
    {
        /// <summary>
        /// Initializes a new instance of the SpriteAnimationDescription class.
        /// </summary>
        public SpriteAnimationDescription()
        {
            Repeat = "loop";
        }

        /// <summary>
        /// Retrieves the animation's name.
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves the animation's repeat value.
        /// </summary>
        public String Repeat
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves an array containing the animation's frames.
        /// </summary>
        public SpriteFrameDescription[] Frames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an array of frame groups describing the animation's frames.
        /// </summary>
        public SpriteFrameGroupDescription[] FrameGroups
        {
            get;
            set;
        }
    }
}
