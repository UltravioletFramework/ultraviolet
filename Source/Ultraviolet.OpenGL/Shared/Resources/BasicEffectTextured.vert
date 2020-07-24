#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.verth" executing

#define INCLUDE_MATRICES
#define INCLUDE_LIGHTING
#define INCLUDE_TEXTURES
#define INCLUDE_SRGB

#includeres "Ultraviolet.OpenGL.Resources.BasicEffectCommon.verth" executing

in  vec4 uv_Position0;
in  vec2 uv_TextureCoordinate0;

out vec4 vDiffuse;
out vec4 vSpecular;
out vec2 vTextureCoordinate;

void main()
{
	vec4 pos_ws = uv_Position0 * World;
	vec4 pos_vw = pos_ws * View;
	vec4 pos_ps = pos_vw * Projection;

	gl_Position        = pos_ps;
	vDiffuse           = DiffuseColor;
	vSpecular          = vec4(0, 0, 0, 0);
	vTextureCoordinate = flip_texture_coords(uv_TextureCoordinate0);
}