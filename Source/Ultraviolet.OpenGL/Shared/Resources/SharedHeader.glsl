#ifver_gt "2.0" { #version 140 }
#ifver "es2.0" { #version 100 }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }
#ifstage "fragment" { #define STAGE_FRAGMENT_SHADER }
#ifstage "vertex" { #define STAGE_VERTEX_SHADER }

#ifdef GL_ES

precision mediump float;
precision mediump int;

#endif

#ifdef GLES2

#if defined(STAGE_FRAGMENT_SHADER)
	#define in varying
	#define out varying
#else
	#define in attribute
	#define out varying
#endif

#define DECLARE_OUTPUT_COLOR
#define OUTPUT_COLOR gl_FragColor
#define SAMPLE_TEXTURE2D texture2D

#else

#define DECLARE_OUTPUT_COLOR out vec4 fFragColor;
#define OUTPUT_COLOR fFragColor
#define SAMPLE_TEXTURE2D texture

#endif