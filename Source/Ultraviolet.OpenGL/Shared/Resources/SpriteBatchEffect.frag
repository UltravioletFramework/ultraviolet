#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.fragh" executing

uniform sampler2D Texture;

in  vec4 vColor;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
	fColor = texture(Texture, vTextureCoordinate) * vColor;
}