#ifver "es2.0" { #version 100 }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }

#ifdef GLES2
    #define DECLARE_INPUT_POSITION attribute vec4 uv_Position0
    #define DECLARE_INPUT_COLOR attribute vec4 uv_Color0
    #define DECLARE_INPUT_TEXCOORD attribute vec2 uv_TextureCoordinate0
    #define DECLARE_OUTPUT_COLOR varying vec4 vColor
    #define DECLARE_OUTPUT_TEXCOORD varying vec2 vTextureCoordinate
#else
    #define DECLARE_INPUT_POSITION in vec4 uv_Position0
    #define DECLARE_INPUT_COLOR in vec4 uv_Color0
    #define DECLARE_INPUT_TEXCOORD in vec2 uv_TextureCoordinate0 
    #define DECLARE_OUTPUT_COLOR out vec4 vColor
    #define DECLARE_OUTPUT_TEXCOORD out vec2 vTextureCoordinate	
#endif

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;

DECLARE_INPUT_POSITION;		// uv_Position0
DECLARE_INPUT_COLOR;		// uv_Color0
DECLARE_INPUT_TEXCOORD;		// uv_TextureCoordinate0

DECLARE_OUTPUT_COLOR;		// vColor
DECLARE_OUTPUT_TEXCOORD;	// vTextureCoordinate

void main()
{
    gl_Position        = uv_Position0 * MatrixTransform;
    vColor             = uv_Color0;
    vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}