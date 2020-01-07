#version 140
#includeres "Ultraviolet.OpenGL.Resources.SrgbConversion.verth" executing

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;
uniform bool SrgbColor;

in  vec4 uv_Position0;
in  vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = uv_Position0 * World * View * Projection;
	vColor             = SrgbColor ? srgb2linear(DiffuseColor) : DiffuseColor;
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x, 1.0 - uv_TextureCoordinate0.y);
}