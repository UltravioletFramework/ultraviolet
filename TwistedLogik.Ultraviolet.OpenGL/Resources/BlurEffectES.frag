precision mediump float;
precision mediump int;

const float PI = 3.14159265;

uniform sampler2D Texture;

uniform float Resolution;
uniform float Radius;
uniform float Mix;

uniform vec2 Direction;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	// Modified from http://callumhay.blogspot.com/2010/09/gaussian-blur-shader-glsl.html

	float step = 1.0 / Resolution;
	float sigma = Radius;

	vec3 incrementalGaussian;
	incrementalGaussian.x = 1.0 / (sqrt(2.0 * PI) * sigma);
	incrementalGaussian.y = exp(-0.5 / (sigma * sigma));
	incrementalGaussian.z = incrementalGaussian.y * incrementalGaussian.y;

	vec4 avgValue = vec4(0.0, 0.0, 0.0, 0.0);
	float coefficientSum = 0.0;

	avgValue += texture2D(Texture, vTextureCoordinate.xy) * incrementalGaussian.x;
	coefficientSum += incrementalGaussian.x;
	incrementalGaussian.xy *= incrementalGaussian.yz;

	for (float i = 1.0; i <= Radius; i++) 
	{ 
		avgValue += texture2D(Texture, vTextureCoordinate.xy - i * step * Direction) * incrementalGaussian.x;         
		avgValue += texture2D(Texture, vTextureCoordinate.xy + i * step * Direction) * incrementalGaussian.x;         
		coefficientSum += 2.0 * incrementalGaussian.x;
		incrementalGaussian.xy *= incrementalGaussian.yz;
	}

	vec4 outBlurred = avgValue / coefficientSum;	
	vec4 outColored = vec4((vColor.rgb / vColor.a) / (vColor.a * outBlurred.a), vColor.a * outBlurred.a);

	gl_FragColor = mix(outBlurred, outColored, Mix);
}