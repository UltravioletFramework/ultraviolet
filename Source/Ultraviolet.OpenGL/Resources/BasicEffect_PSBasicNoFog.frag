#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

 in vec4 vDiffuse;

DECLARE_OUTPUT_COLOR

void main()
{
	OUTPUT_COLOR = vDiffuse;
}