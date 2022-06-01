using System;
using Ultraviolet;
using Ultraviolet.Core;

namespace UvDebug.UI.Controls
{
    /// <summary>
    /// Represents one of the vertex colors that is available for selection in a <see cref="TriangleColorSelector"/> control.
    /// </summary>
    public struct AvailableVertexColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableVertexColor"/> structure.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="color">The color value.</param>
        public AvailableVertexColor(String name, Color color)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            this.name = name;
            this.color = color;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return name;
        }

        /// <summary>
        /// Gets the name of the color.
        /// </summary>
        public String Name { get { return name; } }

        /// <summary>
        /// Gets the color value.
        /// </summary>
        public Color Color { get { return color; } }

        // Property values.
        private readonly String name;
        private readonly Color color;
    }
}
