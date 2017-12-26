#version 140

#include "NoiseCommon.fragh"

uniform sampler2D TextureSampler;
uniform float Time;

in vec4 vColor;
in vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
    fColor = texture(TextureSampler, vTextureCoordinate) * vColor * calculate_noise(gl_FragCoord.xy, vec2(640.0, 480.0), Time);
}