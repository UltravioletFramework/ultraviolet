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

	vec4 avgValue = texture2D(Texture, vTextureCoordinate.xy) * 0.0443269200446036;	
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(1) * step * Direction) * 0.0443269200446036;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(1) * step * Direction) * 0.0443269200446036;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(2) * step * Direction) * 0.0440541398616764;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(2) * step * Direction) * 0.0440541398616764;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(3) * step * Direction) * 0.0432458299079718;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(3) * step * Direction) * 0.0432458299079718;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(4) * step * Direction) * 0.0419314697436659;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(4) * step * Direction) * 0.0419314697436659;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(5) * step * Direction) * 0.0401582033203049;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(5) * step * Direction) * 0.0401582033203049;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(6) * step * Direction) * 0.0379880326851255;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(6) * step * Direction) * 0.0379880326851255;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(7) * step * Direction) * 0.035494222835817;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(7) * step * Direction) * 0.035494222835817;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(8) * step * Direction) * 0.0327572081038753;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(8) * step * Direction) * 0.0327572081038753;
	avgValue += texture2D(Texture, vTextureCoordinate.xy - float(9) * step * Direction) * 0.0298603179490361;
	avgValue += texture2D(Texture, vTextureCoordinate.xy + float(9) * step * Direction) * 0.0298603179490361;

	vec4 blur = avgValue / 0.743959608948757;
	vec4 outBlurred = blur * vColor.a;	
	vec4 outColored = vColor * blur.a;
		
	gl_FragColor = mix(outBlurred, outColored, Mix);
}
