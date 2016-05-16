#version 140

uniform sampler2D textureSampler;
uniform vec4 colors[4];

in  vec4 vColor;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
    fColor = texture(textureSampler, vTextureCoordinate) * colors[3];
}