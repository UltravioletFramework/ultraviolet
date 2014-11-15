uniform sampler2D Texture;

varying vec4 vColor;
varying vec2 vTextureCoordinate;

void main()
{
	gl_FragColor = texture2D(Texture, vTextureCoordinate) * vColor;
}