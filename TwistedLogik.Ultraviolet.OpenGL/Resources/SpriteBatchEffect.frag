#version 130

uniform sampler2D textureSampler;

in  vec4 vColor;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
	fColor = texture(textureSampler, vTextureCoordinate) * vColor;
}