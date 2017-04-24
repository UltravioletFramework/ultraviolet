using System.IO;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SurfaceSaver"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="SurfaceSaver"/> that was created.</returns>
    public delegate SurfaceSaver SurfaceSaverFactory();

    /// <summary>
    /// Contains methods for saving the contents of a surface to a file.
    /// </summary>
    public abstract class SurfaceSaver
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SurfaceSaver"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SurfaceSaver"/> that was created.</returns>
        public static SurfaceSaver Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SurfaceSaverFactory>()();
        }

        /// <summary>
        /// Saves the specified surface as a PNG image.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        public abstract void SaveAsPng(Surface2D surface, Stream stream);

        /// <summary>
        /// Saves the specified surface as a JPEG image.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        public abstract void SaveAsJpeg(Surface2D surface, Stream stream);

        /// <summary>
        /// Saves the specified render target as a PNG image.
        /// </summary>
        /// <param name="renderTarget">The render target to save.</param>
        /// <param name="stream">The stream to which to save the render target data.</param>
        public abstract void SaveAsPng(RenderTarget2D renderTarget, Stream stream);

        /// <summary>
        /// Saves the specified render target as a JPEG image.
        /// </summary>
        /// <param name="renderTarget">The render target to save.</param>
        /// <param name="stream">The stream to which to save the render target data.</param>
        public abstract void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream);
    }
}
