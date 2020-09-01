namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents Ultraviolet's built-in camera parameters.
    /// </summary>
    public enum CameraParameter
    {
        /// <summary>
        /// The world matrix.
        /// </summary>
        World,

        /// <summary>
        /// The view matrix.
        /// </summary>
        View,

        /// <summary>
        /// The projection matrix.
        /// </summary>
        Projection,

        /// <summary>
        /// The combined view-projection matrix.
        /// </summary>
        ViewProj,

        /// <summary>
        /// The combined world-view-projection matrix.
        /// </summary>
        WorldViewProj,
    }
}
