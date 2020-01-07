#version 140

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;

in vec4 uv_Position0;
in vec4 uv_Color0;
in vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = uv_Position0 * MatrixTransform;
	vColor             = uv_Color0;
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}