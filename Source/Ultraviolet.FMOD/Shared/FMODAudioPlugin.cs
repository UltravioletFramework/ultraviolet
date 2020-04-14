using Ultraviolet.Core;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents an Ultraviolet plugin which registers FMOD as the audio subsystem implementation.
    /// </summary>
    public class FMODAudioPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            var asm = typeof(FMODAudioPlugin).Assembly;
            configuration.AudioSubsystemAssembly = $"{asm.GetName().Name}, Version={asm.GetName().Version}, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL";

            base.Register(configuration);
        }
    }
}
