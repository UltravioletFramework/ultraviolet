uniform sampler2D textureSampler;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	gl_FragColor = texture2D(textureSampler, vTextureCoordinate) * vColor;
}