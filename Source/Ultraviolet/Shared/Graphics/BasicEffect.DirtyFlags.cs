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
            /// The <see cref="SrgbColor"/> parameter is dirty.
            /// </summary>
            SrgbColor = 1 << 1,

            /// <summary>
            /// The <see cref="PreferPerPixelLighting"/> parameter is dirty.
            /// </summary>
            PreferPerPixelLighting = 1 << 2,

            /// <summary>
            /// The <see cref="VertexColorEnabled"/> parameter is dirty.
            /// </summary>
            VertexColorEnabled = 1 << 3,

            /// <summary>
            /// The <see cref="Alpha"/> parameter is dirty.
            /// </summary>
            Alpha = 1 << 4,

            /// <summary>
            /// The <see cref="DiffuseColor"/> parameter is dirty.
            /// </summary>
            DiffuseColor = 1 << 5,

            /// <summary>
            /// The <see cref="EmissiveColor"/> parameter is dirty.
            /// </summary>
            EmissiveColor = 1 << 6,

            /// <summary>
            /// The <see cref="SpecularColor"/> parameter is dirty.
            /// </summary>
            SpecularColor = 1 << 7,

            /// <summary>
            /// The <see cref="SpecularPower"/> parameter is dirty.
            /// </summary>
            SpecularPower = 1 << 8,

            /// <summary>
            /// The <see cref="FogEnabled"/> parameter is dirty.
            /// </summary>
            FogEnabled = 1 << 9,

            /// <summary>
            /// The <see cref="FogColor"/> parameter is dirty.
            /// </summary>
            FogColor = 1 << 10,

            /// <summary>
            /// The <see cref="FogStart"/> parameter is dirty.
            /// </summary>
            FogStart = 1 << 11,

            /// <summary>
            /// The <see cref="FogEnd"/> parameter is dirty.
            /// </summary>
            FogEnd = 1 << 12,

            /// <summary>
            /// The <see cref="World"/> parameter is dirty.
            /// </summary>
            World = 1 << 13,

            /// <summary>
            /// The <see cref="View"/> parameter is dirty.
            /// </summary>
            View = 1 << 14,

            /// <summary>
            /// The <see cref="Projection"/> parameter.
            /// </summary>
            Projection = 1 << 15,

            /// <summary>
            /// The <see cref="World"/>, <see cref="View"/>, or <see cref="Projection"/> parameter is dirty.
            /// </summary>
            Matrices = 1 << 16,

            /// <summary>
            /// The <see cref="TextureEnabled"/> parameter is dirty.
            /// </summary>
            TextureEnabled = 1 << 17,

            /// <summary>
            /// The <see cref="Texture"/> parameter is dirty.
            /// </summary>
            Texture = 1 << 18,

            /// <summary>
            /// The <see cref="LightingEnabled"/> parameter is dirty.
            /// </summary>
            LightingEnabled = 1 << 19,

            /// <summary>
            /// The <see cref="AmbientLightColor"/> parameter is dirty.
            /// </summary>
            AmbientLightColor = 1 << 20,
        }
    }
}
