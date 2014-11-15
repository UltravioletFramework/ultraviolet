uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

attribute vec4 uv_Position0;
attribute vec4 uv_Color0;
attribute vec2 uv_TextureCoordinate0;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	gl_Position        = Projection * View * World * uv_Position0;
	vColor             = DiffuseColor * uv_Color0;
	vTextureCoordinate = uv_TextureCoordinate0;
}