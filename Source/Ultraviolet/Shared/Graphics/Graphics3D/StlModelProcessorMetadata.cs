using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="StlModelProcessor"/> class.
    /// </summary>
    public sealed class StlModelProcessorMetadata
    {
        /// <summary>
        /// Gets or sets the diffuse color which is applied to the model.
        /// </summary>
        public Color DiffuseColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the scale which is applied to the model.
        /// </summary>
        public Single Scale { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the rotation along the x-axis which is applied to the model.
        /// </summary>
        public Single RotationX { get; set; }

        /// <summary>
        /// Gets or sets the rotation along the y-axis which is applied to the model.
        /// </summary>
        public Single RotationY { get; set; }

        /// <summary>
        /// Gets or sets the rotation along the z-axis which is applied to the model.
        /// </summary>
        public Single RotationZ { get; set; }
    }
}
