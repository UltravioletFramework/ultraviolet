#version 130

uniform mat4 MatrixTransform;

in  vec4 uv_Position0;
in  vec4 uv_Color0;
in  vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = MatrixTransform * uv_Position0;
	vColor             = uv_Color0;
	vTextureCoordinate = uv_TextureCoordinate0;
}