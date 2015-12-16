using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the data which is used to draw an individual glyph.
    /// </summary>
    public struct GlyphData
    {
        /// <summary>
        /// Gets or sets the glyph which is being drawn.
        /// </summary>
        public Char Glyph
        {
            get { return glyph; }
            set
            {
                if (glyph != value)
                {
                    glyph = value;
                    dirtyGlyph = true;
                }
            }
        }

        /// <summary>
        /// Gets the number of times that this glyph has been passed through the shader.
        /// </summary>
        public Int32 Pass
        {
            get { return pass; }
            internal set { pass = value; }
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
                    dirtyPosition = true;
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
                    dirtyPosition = true;
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
                    dirtyScale = true;
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
                    dirtyScale = true;
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
                    dirtyColor = true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether any of the data's properties have been changed.
        /// </summary>
        public Boolean Dirty
        {
            get { return dirtyGlyph || dirtyPosition || dirtyScale || dirtyColor; }
        }

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="Glyph"/> property has been changed.
        /// </summary>
        public Boolean DirtyGlyph
        {
            get { return dirtyGlyph; }
        }

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="X"/> or <see cref="Y"/> properties have been changed.
        /// </summary>
        public Boolean DirtyPosition
        {
            get { return dirtyPosition; }
        }

        /// <summary>
        /// Gets a value indicating whether the data's <see cref="ScaleX"/> or <see cref="ScaleY"/> properties have been changed.
        /// </summary>
        public Boolean DirtyScale
        {
            get { return dirtyScale; }
        }
        
        /// <summary>
        /// Gets a value indicating whether the data's <see cref="Color"/> property has been changed.
        /// </summary>
        public Boolean DirtyColor
        {
            get { return dirtyColor; }
        }

        /// <summary>
        /// Clears the flags which specify whether any of the glyph data's properties are dirty.
        /// </summary>
        internal void ClearDirtyFlags()
        {
            dirtyGlyph = false;
            dirtyPosition = false;
            dirtyScale = false;
            dirtyColor = false;
        }

        // Property values.
        private Char glyph;
        private Int32 pass;
        private Single x;
        private Single y;
        private Single scaleX;
        private Single scaleY;
        private Color color;

        // State values.
        private Boolean dirtyGlyph;
        private Boolean dirtyPosition;
        private Boolean dirtyScale;
        private Boolean dirtyColor;
    }
}
