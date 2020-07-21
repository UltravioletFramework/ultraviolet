using System;
using Ultraviolet.Platform;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="GraphicsCapabilities"/>.
    /// </summary>
    public sealed class DummySwapChainManager : SwapChainManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummySwapChainManager"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public DummySwapChainManager(UltravioletContext uv) : base(uv) { }

        /// <inheritdoc/>
        public override void DrawAndSwap(UltravioletTime time,
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawing,
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawn)
        { }
    }
}
