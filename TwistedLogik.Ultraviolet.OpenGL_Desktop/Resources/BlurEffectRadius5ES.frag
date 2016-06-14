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

	vec4 avgValue = texture2D(Texture, vTextureCoordinate.xy) * 0.0797884560802865;	
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(1) * step * Direction) * 0.0797884560802865;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(1) * step * Direction) * 0.0797884560802865;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(2) * step * Direction) * 0.0782085387950912;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(2) * step * Direction) * 0.0782085387950912;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(3) * step * Direction) * 0.0736540280606647;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(3) * step * Direction) * 0.0736540280606647;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(4) * step * Direction) * 0.0666449205783599;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(4) * step * Direction) * 0.0666449205783599;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(5) * step * Direction) * 0.0579383105522965;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(5) * step * Direction) * 0.0579383105522965;

	vec4 blur = avgValue / 0.792256964213684;
	vec4 outBlurred = blur * vColor.a;	
	vec4 outColored = vColor * blur.a;
		
	gl_FragColor = mix(outBlurred, outColored, Mix);
}
