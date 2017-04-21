#ifver "es2.0" { #version 100 es }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

#ifdef GLES2    
    attribute vec4 uv_Position0;

    varying vec4 vColor;
#else
    in vec4 uv_Position0;

    out vec4 vColor;    
#endif

void main()
{
    gl_Position = Projection * View * World * uv_Position0;
    vColor = DiffuseColor;
}