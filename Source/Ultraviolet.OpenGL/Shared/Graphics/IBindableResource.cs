
namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a resource which can be bound to the graphics device.
    /// </summary>
    public interface IBindableResource
    {
        /// <summary>
        /// Binds the resource for reading.
        /// </summary>
        void BindRead();

        /// <summary>
        /// Binds the resource for writing.
        /// </summary>
        void BindWrite();

        /// <summary>
        /// Unbinds the resource for reading.
        /// </summary>
        void UnbindRead();

        /// <summary>
        /// Unbinds the resource for writing.
        /// </summary>
        void UnbindWrite();
    }
}
