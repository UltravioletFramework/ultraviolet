#includeres "TwistedLogik.Ultraviolet.OpenGL.Resources.HeaderES.fragh" executing

uniform sampler2D textureSampler;

DECLARE_INPUT_COLOR;	// vColor
DECLARE_INPUT_TEXCOORD;	// vTextureCoordinate

DECLARE_OUTPUT_COLOR;	// fColor

void main()
{
	fColor = texture(textureSampler, vTextureCoordinate) * vColor;
}