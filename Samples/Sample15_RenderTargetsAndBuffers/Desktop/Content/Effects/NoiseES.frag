#ifver "es2.0" { #version 100 }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }

precision mediump float;
precision mediump int;

#ifdef GLES2
    #define texture texture2D
    #define fColor gl_FragColor
    #define DECLARE_INPUT_COLOR varying vec4 vColor
    #define DECLARE_INPUT_TEXCOORD varying vec2 vTextureCoordinate
    #define DECLARE_OUTPUT_COLOR
#else
    #define DECLARE_INPUT_COLOR in vec4 vColor
    #define DECLARE_INPUT_TEXCOORD in vec2 vTextureCoordinate
    #define DECLARE_OUTPUT_COLOR out vec4 fColor
#endif

uniform sampler2D Texture;
uniform float Time;

DECLARE_INPUT_COLOR;
DECLARE_INPUT_TEXCOORD;

DECLARE_OUTPUT_COLOR;

#include "NoiseCommon.fragh"

void main() 
{         
    fColor = texture(Texture, vTextureCoordinate) * vColor * calculate_noise(gl_FragCoord.xy, vec2(640.0, 480.0), Time);
}