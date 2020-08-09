#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

 in vec4 uv_Position0;
 in vec2 uv_TextureCoordinate0;

out vec4 vDiffuse;
out vec4 vSpecular;
out vec2 vTexCoord;
out vec4 vPositionPS;

void main()
{
	CommonVSOutput cout = ComputeCommonVSOutput(uv_Position0);
	SetCommonVSOutputParams;

	vTexCoord = FlipTextureCoordinates(uv_TextureCoordinate0);
}