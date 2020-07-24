using System;

namespace Ultraviolet.Graphics
{
    partial class BasicEffect
    {
        /// <summary>
        /// Represents a set of flags which are used to track changes to the parameters of a <see cref="BasicEffect"/> instance.
        /// </summary>
        [Flags]
        private enum DirtyFlags
        {
            /// <summary>
            /// No dirty parameters.
            /// </summary>
            None = 0,

            /// <summary>
            /// The <see cref="DiffuseColor"/> parameter is dirty.
            /// </summary>
            DiffuseColor = 1 << 1,

            /// <summary>
            /// The <see cref="FogColor"/> parameter is dirty.
            /// </summary>
            FogColor = 1 << 2,

            /// <summary>
            /// The <see cref="FogStart"/> parameter is dirty.
            /// </summary>
            FogStart = 1 << 3,

            /// <summary>
            /// The <see cref="FogEnd"/> parameter is dirty.
            /// </summary>
            FogEnd = 1 << 4,

            /// <summary>
            /// The <see cref="World"/> parameter is dirty.
            /// </summary>
            World = 1 << 5,

            /// <summary>
            /// The <see cref="View"/> parameter is dirty.
            /// </summary>
            View = 1 << 6,

            /// <summary>
            /// The <see cref="Projection"/> parameter.
            /// </summary>
            Projection = 1 << 7,

            /// <summary>
            /// The <see cref="World"/>, <see cref="View"/>, or <see cref="Projection"/> parameter is dirty.
            /// </summary>
            WorldViewProj = 1 << 8,

            /// <summary>
            /// The <see cref="Texture"/> parameter is dirty.
            /// </summary>
            Texture = 1 << 9,

            /// <summary>
            /// The <see cref="SrgbColor"/> parameter is dirty.
            /// </summary>
            SrgbColor = 1 << 10,
        }
    }
}
