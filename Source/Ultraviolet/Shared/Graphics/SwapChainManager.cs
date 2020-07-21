using System;
using Ultraviolet.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a factory method which creates new instances of the <see cref="SwapChainManager"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The <see cref="SwapChainManager"/> instance which was created.</returns>
    public delegate SwapChainManager SwapChainManagerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a service which manages the graphics swap chain.
    /// </summary>
    public abstract class SwapChainManager : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChainManager"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SwapChainManager(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="SwapChainManager"/> class.
        /// </summary>
        /// <returns>The <see cref="SwapChainManager"/> instance that was created.</returns>
        public static SwapChainManager Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SwapChainManagerFactory>()(uv);
        }

        /// <summary>
        /// Renders the current frame and swaps the framebuffers.
        /// </summary>
        /// <param name="time">Time elapsed since the last time the framebuffers were drawn and swapped.</param>
        /// <param name="onWindowDrawing">An action to perform when a window is being drawn.</param>
        /// <param name="onWindowDrawn">An action to perform when a window has finished drawing.</param>
        public abstract void DrawAndSwap(UltravioletTime time, 
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawing, 
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawn);
    }
}
