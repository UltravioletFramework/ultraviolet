#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

 in vec4 uv_Position0;
 in vec3 uv_Normal0;

out vec4 vDiffuse;
out vec4 vSpecular;
out vec4 vPositionPS;

void main()
{
	CommonVSOutput cout = ComputeCommonVSOutputWithLighting(uv_Position0, uv_Normal0, 1);
	SetCommonVSOutputParams;
}