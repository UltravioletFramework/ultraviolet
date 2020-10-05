using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the block of effect parameters which are used by <see cref="OpenGLBasicEffect"/>.
    /// </summary>
    internal class OpenGLBasicEffectParameterBlock
    {
        public EffectParameter DiffuseColor { get; set; }
        public EffectParameter EmissiveColor { get; set; }
        public EffectParameter SpecularColor { get; set; }
        public EffectParameter SpecularPower { get; set; }
        public EffectParameter EyePosition { get; set; }
        public EffectParameter FogColor { get; set; }
        public EffectParameter FogVector { get; set; }
        public EffectParameter World { get; set; }
        public EffectParameter WorldInverseTranspose { get; set; }
        public EffectParameter WorldViewProj { get; set; }
        public EffectParameter SrgbColor { get; set; }
        public EffectParameter Texture { get; set; }
    }
}
