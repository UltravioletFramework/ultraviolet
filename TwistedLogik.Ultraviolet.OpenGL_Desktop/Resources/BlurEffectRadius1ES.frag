precision mediump float;
precision mediump int;

uniform sampler2D Texture;

uniform float Resolution;
uniform float Mix;
uniform vec2  Direction;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	// Modified from http://callumhay.blogspot.com/2010/09/gaussian-blur-shader-glsl.html

	float step = 1.0 / Resolution;

	vec4 avgValue = texture2D(Texture, vTextureCoordinate.xy) * 0.398942280401433;	
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(1) * step * Direction) * 0.398942280401433;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(1) * step * Direction) * 0.398942280401433;

	vec4 blur = avgValue / 1.1968268412043;
	vec4 outBlurred = blur * vColor.a;	
	vec4 outColored = vColor * blur.a;
		
	gl_FragColor = mix(outBlurred, outColored, Mix);
}
