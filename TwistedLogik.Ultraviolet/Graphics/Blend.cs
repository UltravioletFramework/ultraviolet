
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the available color blending factors.
    /// </summary>
    public enum Blend
    {
        /// <summary>
        /// The components of the blended color are multiplied by zero.
        /// </summary>
        Zero,

        /// <summary>
        /// The components of the blended color are multiplied by one.
        /// </summary>
        One,

        /// <summary>
        /// The components of the blended color are multiplied by the source color.
        /// </summary>
        SourceColor,

        /// <summary>
        /// The components of the blended color are multiplied by the inverse of the source color.
        /// </summary>
        InverseSourceColor,

        /// <summary>
        /// The components of the blended color are multiplied by the source alpha.
        /// </summary>
        SourceAlpha,

        /// <summary>
        /// The components of the blended color are multiplied by the inverse of the source alpha.
        /// </summary>
        InverseSourceAlpha,

        /// <summary>
        /// The components of the blended color are multiplied by the destination alpha.
        /// </summary>
        DestinationAlpha,

        /// <summary>
        /// The components of the blended color are multiplied by the inverse of the destination alpha.
        /// </summary>
        InverseDestinationAlpha,

        /// <summary>
        /// The components of the blended color are multiplied by the destination color.
        /// </summary>
        DestinationColor,

        /// <summary>
        /// The components of the blended color are multiplied by the inverse of the destination color.
        /// </summary>
        InverseDestinationColor,

        /// <summary>
        /// The components of the blended color are multiplied by the larger of 
        /// the alpha of the source color or the inverse of the alpha of the source color.
        /// </summary>
        SourceAlphaSaturation,

        /// <summary>
        /// The components of the blended color are multiplied by a constant value.
        /// </summary>
        BlendFactor,

        /// <summary>
        /// The components of the blended color are multiplied by the inverse of a constant value.
        /// </summary>
        InverseBlendFactor,
    }
}
