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

	vec4 avgValue = texture(Texture, vTextureCoordinate.xy) * 0.0797884560802865;	
	avgValue += texture(Texture, vTextureCoordinate.xy - float(1) * step * Direction) * 0.0797884560802865;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(1) * step * Direction) * 0.0797884560802865;
	avgValue += texture(Texture, vTextureCoordinate.xy - float(2) * step * Direction) * 0.0782085387950912;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(2) * step * Direction) * 0.0782085387950912;
	avgValue += texture(Texture, vTextureCoordinate.xy - float(3) * step * Direction) * 0.0736540280606647;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(3) * step * Direction) * 0.0736540280606647;
	avgValue += texture(Texture, vTextureCoordinate.xy - float(4) * step * Direction) * 0.0666449205783599;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(4) * step * Direction) * 0.0666449205783599;
	avgValue += texture(Texture, vTextureCoordinate.xy - float(5) * step * Direction) * 0.0579383105522965;
	avgValue += texture(Texture, vTextureCoordinate.xy + float(5) * step * Direction) * 0.0579383105522965;

	vec4 blur = avgValue / 0.792256964213684;
	vec4 outBlurred = blur * vColor.a;	
	vec4 outColored = vColor * blur.a;
		
	fColor = mix(outBlurred, outColored, Mix);
}
