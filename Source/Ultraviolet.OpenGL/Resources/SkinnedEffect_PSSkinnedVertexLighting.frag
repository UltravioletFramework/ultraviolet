#includeres "Ultraviolet.OpenGL.Resources.SkinnedEffectPreamble.glsl" executing

uniform sampler2D Texture;

 in vec2 vTexCoord;
 in vec4 vDiffuse;
 in vec4 vSpecular;

DECLARE_OUTPUT_COLOR

void main()
{
	vec4 color = SAMPLE_TEXTURE2D(Texture, vTexCoord) * vDiffuse;

	AddSpecular(color, vSpecular.rgb);
	ApplyFog(color, vSpecular.w);

	OUTPUT_COLOR = color;
}