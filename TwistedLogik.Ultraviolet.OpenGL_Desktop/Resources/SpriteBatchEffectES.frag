#ifver "es2.0" { #version 100 es }
#ifver "es2.0" { #define GLES2 }
#ifver_gt "es2.0" { #version 300 es }
#ifver_gt "es2.0" { #define GLES3 }

precision mediump float;
precision mediump int;

uniform sampler2D textureSampler;

#ifdef GLES2
	varying vec4 vColor;
	varying vec2 vTextureCoordinate;
#else
	in vec4 vColor;
	in vec2 vTextureCoordinate;
#endif

void main()
{
	gl_FragColor = texture2D(textureSampler, vTextureCoordinate) * vColor;
}