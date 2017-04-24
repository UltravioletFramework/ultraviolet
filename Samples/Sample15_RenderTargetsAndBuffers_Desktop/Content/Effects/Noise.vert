#ifver_gte "1.0" { #version 140 }
#ifdef GL_ES
    #define SHADER_UNIFORM uniform
    #define SHADER_INPUT attribute
    #define SHADER_OUTPUT varying
#else
    #define SHADER_UNIFORM uniform
    #define SHADER_INPUT in
    #define SHADER_OUTPUT out
#endif

SHADER_UNIFORM mat4 MatrixTransform;

SHADER_INPUT vec4 uv_Position0;
SHADER_INPUT vec4 uv_Color0;
SHADER_INPUT vec2 uv_TextureCoordinate0;

SHADER_OUTPUT vec4 vColor;
SHADER_OUTPUT vec2 vTextureCoordinate;

void main()
{
    gl_Position        = MatrixTransform * uv_Position0;
    vColor             = uv_Color0;
    vTextureCoordinate = uv_TextureCoordinate0;
}