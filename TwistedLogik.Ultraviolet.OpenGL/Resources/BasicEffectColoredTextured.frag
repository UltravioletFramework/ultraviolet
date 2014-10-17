#version 130

uniform sampler2D Texture;

in  vec4 vColor;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
	fColor = texture(Texture, vTextureCoordinate) * vColor;
}