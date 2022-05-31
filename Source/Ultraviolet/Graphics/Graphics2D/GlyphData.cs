using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the data which is used to draw an individual glyph.
    /// </summary>
    public struct GlyphData
    {
        /// <summary>
        /// Gets or sets the unicode code point which is being drawn.
        /// Not every glyph corresponds to a Unicode code point.
        /// </summary>
        public Int32 UnicodeCodePoint
        {
            get { return unicodeCodePoint; }
            set
            {
                if (unicodeCodePoint != value)
                {
                    unicodeCodePoint = value;
                    DirtyUnicodeCodePoint = true;
                }
            }
        }

        /// <summary>
        /// Gets the number of times that this glyph has been passed through the shader.
        /// </summary>
        public Int32 Pass
        {
            get; internal set;
        }

        /// <summary>
        /// Gets or sets the x-coordinate at which the glyph is being drawn.
        /// </summary>
        public Single X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    DirtyPosition = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate at which the glyph is being drawn.
        /// </summary>
        public Single Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    DirtyPosition = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the glyph's scaling factor along the x-axis.
        /// </summary>
        public Single ScaleX
        {
            get { return scaleX; }
            set
            {
                if (scaleX != value)
                {
                    scaleX = value;
                    DirtyScale = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the glyph's scaling factor along the y-axis.
        /// </summary>
        public Single ScaleY
        {
            get { return scaleY; }
            set
            {
                if (scaleY != value)
                {
                    scaleY = value;
                    DirtyScale = true;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the color with which the glyph is rendered.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    DirtyColor = true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether any of the data's properties have been changed.
        /// </summary>
        public Boolean Dirty => DirtyUnicodeCodePoint || DirtyPosition || DirtyScale || DirtyColor;

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="UnicodeCodePoint"/> property has been changed.
        /// </summary>
        public Boolean DirtyUnicodeCodePoint
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="X"/> or <see cref="Y"/> properties have been changed.
        /// </summary>
        public Boolean DirtyPosition
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="ScaleX"/> or <see cref="ScaleY"/> properties have been changed.
        /// </summary>
        public Boolean DirtyScale
        {
            get; private set;
        }
        
        /// <summary>
        /// Gets a value indicating whether the data's <see cref="Color"/> property has been changed.
        /// </summary>
        public Boolean DirtyColor
        {
            get; private set;
        }

        /// <summary>
        /// Clears the flags which specify whether any of the glyph data's properties are dirty.
        /// </summary>
        internal void ClearDirtyFlags()
        {
            DirtyUnicodeCodePoint = false;
            DirtyPosition = false;
            DirtyScale = false;
            DirtyColor = false;
        }

        // Property values.
        private Int32 unicodeCodePoint;
        private Single x;
        private Single y;
        private Single scaleX;
        private Single scaleY;
        private Color color;
    }
}
