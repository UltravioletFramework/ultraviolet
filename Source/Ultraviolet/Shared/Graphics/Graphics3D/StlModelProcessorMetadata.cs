using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="StlModelProcessor"/> class.
    /// </summary>
    public sealed class StlModelProcessorMetadata
    {
        /// <summary>
        /// Gets or sets the default material which is applied to the model.
        /// </summary>
        public Material DefaultMaterial { get; set; } = new BasicMaterial() { DiffuseColor = Color.White };

        /// <summary>
        /// Gets or sets the scale which is applied to the model.
        /// </summary>
        public Single Scale { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the rotation along the x-axis, in radians, which is applied to the model.
        /// </summary>
        public Radians RotationX { get; set; }

        /// <summary>
        /// Gets or sets the rotation along the y-axis, in radians, which is applied to the model.
        /// </summary>
        public Radians RotationY { get; set; }

        /// <summary>
        /// Gets or sets the rotation along the z-axis, in radians, which is applied to the model.
        /// </summary>
        public Radians RotationZ { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to swap the winding order of the model's vertices.
        /// </summary>
        public Boolean SwapWindingOrder { get; set; }
    }
}
