#ifver_gt "2.0" { #version 140 }
#ifver "es2.0" { #version 100 }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }

#ifdef GL_ES

precision mediump float;
precision mediump int;

#endif

#ifdef GLES2

#define in attribute
#define out varying

#define DECLARE_OUTPUT_COLOR
#define OUTPUT_COLOR gl_FragColor
#define SAMPLE_TEXTURE2D texture2D

#else

#define DECLARE_OUTPUT_COLOR out vec4 fFragColor;
#define OUTPUT_COLOR fFragColor
#define SAMPLE_TEXTURE2D texture

#endif