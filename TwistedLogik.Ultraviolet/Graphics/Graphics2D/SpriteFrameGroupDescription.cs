using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a collectively-defined group of <see cref="SpriteFrame"/> objects during deserialization.
    /// </summary>
    public class SpriteFrameGroupDescription
    {
        /// <summary>
        /// Gets or sets the name of the texture on which the group's frames are defined.
        /// </summary>
        public String Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the area in which the group's frames are defined.
        /// </summary>
        public Int32 AreaX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the area in which the group's frames are defined.
        /// </summary>
        public Int32 AreaY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the area in which the group's frames are defined.
        /// </summary>
        public Int32 AreaWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the area in which the group's frames are defined.
        /// </summary>
        public Int32 AreaHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of frames in this group.
        /// </summary>
        public Int32 FrameCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of frames in this group.
        /// </summary>
        public Int32 FrameWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of frames in this group.
        /// </summary>
        public Int32 FrameHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the distance, in pixels, between the left edge of the frames
        /// in this group and their center points.
        /// </summary>
        public Int32 OriginX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the distance, in pixels, between the top edge of the frames
        /// in this group and their center points.
        /// </summary>
        public Int32 OriginY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets duration of frames in this group.
        /// </summary>
        public Int32 Duration
        {
            get;
            set;
        }
    }
}
