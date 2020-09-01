using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// An intermediate representation of the data in an STL model file.
    /// </summary>
    public class StlModelDescription
    {
        /// <summary>
        /// Gets or sets the model's name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the model's list of triangles.
        /// </summary>
        public IList<StlModelTriangleDescription> Triangles { get; set; }
    }
}
