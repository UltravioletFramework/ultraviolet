using Ultraviolet.Graphics;

namespace Ultraviolet.TestFramework.Graphics
{
    /// <summary>
    /// Represents a compositor which works within the context of the Ultraviolet testing framework.
    /// </summary>
    public interface ITestFrameworkCompositor
    {
        /// <summary>
        /// Gets or sets the target to which the current test is rendering its scene.
        /// </summary>
        RenderTarget2D TestFrameworkRenderTarget { get; set; }
    }
}
