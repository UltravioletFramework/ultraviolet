#includeres "Ultraviolet.OpenGL.Resources.HeaderES.fragh" executing

uniform sampler2D Texture;

uniform float Resolution;
uniform float Mix;
uniform vec2 Direction;

DECLARE_INPUT_COLOR;	// vColor
DECLARE_INPUT_TEXCOORD;	// vTextureCoordinate

DECLARE_OUTPUT_COLOR;	// fColor

void main()
{
	// Modified from http://callumhay.blogspot.com/2010/09/gaussian-blur-shader-glsl.html

	float step = 1.0 / Resolution;

	vec4 avgValue = texture(Texture, vTextureCoordinate.xy) * 0.398942280401433;	
	avgValue += texture(Texture, vTextureCoordinate.xy - float(1) * step * Direction) * 0.398942280401433;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(1) * step * Direction) * 0.398942280401433;

	vec4 blur = avgValue / 1.1968268412043;
	vec4 outBlurred = blur * vColor.a;	
	vec4 outColored = vColor * blur.a;
		
	fColor = mix(outBlurred, outColored, Mix);
}
