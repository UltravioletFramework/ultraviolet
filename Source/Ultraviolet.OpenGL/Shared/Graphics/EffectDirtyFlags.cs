using System;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the set of values on an effect which are dirty and require updates.
    /// </summary>
    [Flags]
    internal enum EffectDirtyFlags
    {
        /// <summary>
        /// No dirty values.
        /// </summary>
        None = 0,

        /// <summary>
        /// A value changed which will change the effect's shader index.
        /// </summary>
        ShaderIndex = 1,

        /// <summary>
        /// A value relating to the handling of sRGB color has changed.
        /// </summary>
        SrgbColor = 2,

        /// <summary>
        /// A value relating to the material color has changed.
        /// </summary>
        MaterialColor = 4,

        /// <summary>
        /// A value relating to the material texture has changed.
        /// </summary>
        MaterialTexture = 8,

        /// <summary>
        /// A value relating to the material's world matrix has changed.
        /// </summary>
        World = 16,

        /// <summary>
        /// A value relating to the material's combined world/view/projection matrix has changed.
        /// </summary>
        WorldViewProjection = 32,

        /// <summary>
        /// A value relating to the current eye position has changed.
        /// </summary>
        EyePosition = 64, 

        /// <summary>
        /// A value indicating whether fog is enabled has changed.
        /// </summary>
        FogEnabled = 128,

        /// <summary>
        /// A value relating to the calculation of fog has changed.
        /// </summary>
        Fog = 256,

        /// <summary>
        /// All values are dirty.
        /// </summary>
        All = -1,
    }
}
