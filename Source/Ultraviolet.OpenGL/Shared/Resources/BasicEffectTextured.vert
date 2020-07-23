#version 140

#define INCLUDE_MATRICES
#define INCLUDE_LIGHTING
#define INCLUDE_TEXTURES
#define INCLUDE_SRGB

#includeres "Ultraviolet.OpenGL.Resources.BasicEffectCommon.verth" executing

in  vec4 uv_Position0;
in  vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = transform_position(uv_Position0);
	vColor             = process_color(DiffuseColor);
	vTextureCoordinate = flip_texture_coords(uv_TextureCoordinate0);
}