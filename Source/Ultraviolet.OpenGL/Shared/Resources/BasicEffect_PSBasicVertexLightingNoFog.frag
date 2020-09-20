#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

 in vec4 vDiffuse;
 in vec4 vSpecular;

DECLARE_OUTPUT_COLOR

void main()
{
	vec4 color = vDiffuse;

	AddSpecular(color, vSpecular.rgb);

	OUTPUT_COLOR = color;
}