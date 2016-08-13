uniform mat4 MatrixTransform;
uniform vec2 TextureSize;

attribute vec4 uv_Position0;
attribute vec4 uv_Color0;
attribute vec2 uv_TextureCoordinate0;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	gl_Position = MatrixTransform * uv_Position0;
	vColor = uv_Color0;
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}