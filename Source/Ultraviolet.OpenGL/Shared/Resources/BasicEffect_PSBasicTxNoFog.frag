#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

uniform sampler2D Texture;

 in vec4 vDiffuse;
 in vec2 vTexCoord;

DECLARE_OUTPUT_COLOR

void main()
{
	OUTPUT_COLOR = SAMPLE_TEXTURE2D(Texture, vTexCoord) * vDiffuse;
}