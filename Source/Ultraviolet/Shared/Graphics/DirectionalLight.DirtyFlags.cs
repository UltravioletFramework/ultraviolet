using System;

namespace Ultraviolet.Graphics
{
    partial class DirectionalLight
    {
        /// <summary>
        /// Represents a set of flags which are used to track changes to the parameters of a <see cref="DirectionalLight"/> instance.
        /// </summary>
        [Flags]
        private enum DirtyFlags
        {
            /// <summary>
            /// No dirty parameters.
            /// </summary>
            None = 0,

            /// <summary>
            /// The <see cref="DirectionalLight.Direction"/> parameter is dirty.
            /// </summary>
            Direction,

            /// <summary>
            /// The <see cref="DirectionalLight.DiffuseColor"/> parameter is dirty.
            /// </summary>
            DiffuseColor,

            /// <summary>
            /// The <see cref="DirectionalLight.SpecularColor"/> parameter is dirty.
            /// </summary>
            SpecularColor,
        }
    }
}
