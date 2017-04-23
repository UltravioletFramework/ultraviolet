#includeres "TwistedLogik.Ultraviolet.OpenGL.Resources.HeaderES.fragh" executing

uniform sampler2D Texture;

DECLARE_INPUT_COLOR;	// vColor
DECLARE_INPUT_TEXCOORD;	// vTextureCoordinate

DECLARE_OUTPUT_COLOR;	// fColor

void main()
{
	fColor = vColor * texture(Texture, vTextureCoordinate);
}