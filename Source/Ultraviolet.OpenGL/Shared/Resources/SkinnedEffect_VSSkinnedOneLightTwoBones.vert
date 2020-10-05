#includeres "Ultraviolet.OpenGL.Resources.SkinnedEffectPreamble.glsl" executing

 in  vec4 uv_Position0;
 in  vec3 uv_Normal0;
 in  vec2 uv_TextureCoordinate0;
 in ivec4 uv_BlendIndices0;
 in  vec4 uv_BlendWeight0;

out vec4 vDiffuse;
out vec4 vSpecular;
out vec2 vTexCoord;
out vec4 vPositionPS;

void main()
{
	vec4 position = uv_Position0;
	vec3 normal = uv_Normal0;

    Skin(position, normal, uv_BlendIndices0, uv_BlendWeight0, 2);

	CommonVSOutput cout = ComputeCommonVSOutputWithLighting(position, normal, 1);
	SetCommonVSOutputParams;
	
	vTexCoord = FlipTextureCoordinates(uv_TextureCoordinate0);
}