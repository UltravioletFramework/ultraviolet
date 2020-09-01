namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a <see cref="Camera"/> which exposes a value for the eye position.
    /// </summary>
    public interface ICameraEyePosition
    {
        /// <summary>
        /// Gets the position of the camera's "eye" in 3D space.
        /// </summary>
        Vector3 EyePosition { get; }
    }
}
