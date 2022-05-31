using Ultraviolet.Core;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents an Ultraviolet plugin which registers OpenGL as the graphics subsystem implementation.
    /// </summary>
    public class OpenGLGraphicsPlugin : UltravioletPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLGraphicsPlugin"/> class.
        /// </summary>
        /// <param name="configuration">The graphics configuration settings.</param>
        public OpenGLGraphicsPlugin(OpenGLGraphicsConfiguration configuration = null)
        {
            this.configuration = configuration ?? OpenGLGraphicsConfiguration.Default;
        }

        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            var asm = typeof(OpenGLGraphicsPlugin).Assembly;
            configuration.GraphicsSubsystemAssembly = $"{asm.GetName().Name}, Version={asm.GetName().Version}, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL";
            configuration.GraphicsConfiguration = this.configuration;

            base.Register(configuration);
        }

        // Graphics configuration settings.
        private readonly OpenGLGraphicsConfiguration configuration;
    }
}
