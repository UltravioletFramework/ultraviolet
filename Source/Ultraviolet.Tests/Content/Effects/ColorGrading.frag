#version 140

#sampler 0 "Texture"
#sampler 1 "ColorGradingLUT"

uniform sampler2D Texture;
uniform sampler3D ColorGradingLUT;

in  vec4 vColor;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
	vec3 rawColor = texture(Texture, vTextureCoordinate).rgb;
	fColor = vec4(texture(ColorGradingLUT, vec3(rawColor.r, 1.0 - rawColor.b, rawColor.g)).xyz, 1.0) * vColor;
}