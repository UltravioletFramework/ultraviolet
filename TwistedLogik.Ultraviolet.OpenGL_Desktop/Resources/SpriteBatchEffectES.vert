#ifver "es2.0" { #version 100 es }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }
#extension EXT_gpu_shader4 : enable

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;

#ifdef GLES2    

    attribute vec4 uv_Position0;
    attribute vec4 uv_Color0;
    #if GL_EXT_gpu_shader4
        attribute uvec2 uv_TextureCoordinate0;    
    #else
        attribute vec2 uv_TextureCoordinate0;
    #endif

    varying vec4 vColor;
    varying vec2 vTextureCoordinate;

#else

    in vec4 uv_Position0;
    in vec4 uv_Color0;
    in uvec2 uv_TextureCoordinate0;

    out vec4 vColor;
    out vec2 vTextureCoordinate;

#endif

void main()
{
    gl_Position = MatrixTransform * uv_Position0;
    vColor = uv_Color0;
    vTextureCoordinate = vec2(float(uv_TextureCoordinate0.x) / TextureSize.x, 1.0 - (float(uv_TextureCoordinate0.y) / TextureSize.y));
}

