#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

 in vec4 uv_Position0;
 in vec3 uv_Normal0;
 in vec2 uv_TextureCoordinate0;
 in vec4 uv_Color0;

out vec2 vTexCoord;
out vec4 vPositionWS;
out vec3 vNormalWS;
out vec4 vDiffuse;
out vec4 vPositionPS;

void main()
{
	CommonVSOutputPixelLighting  cout = ComputeCommonVSOutputPixelLighting(uv_Position0, uv_Normal0);
	SetCommonVSOutputParamsPixelLighting;
	
	vec4 c = ConvertColor(uv_Color0);
	vDiffuse.rgb = c.rgb;
	vDiffuse.a = c.a * DiffuseColor.a;
	vTexCoord = FlipTextureCoordinates(uv_TextureCoordinate0);
}