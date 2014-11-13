using TwistedLogik.Ultraviolet.Android.Platform;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.Android.Graphics;

namespace TwistedLogik.Ultraviolet.Android
{
    /// <summary>
    /// Initializes factory methods for the Android platform compatibility shim.
    /// </summary>
    internal sealed class AndroidFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new AndroidSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new AndroidSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new AndroidIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new AndroidFileSystemService());
        }
    }
}
