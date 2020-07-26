#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.glsl" executing

uniform sampler2D Texture;

in  vec4 vColor;
in  vec2 vTextureCoordinate;

DECLARE_OUTPUT_COLOR;

void main()
{
	OUTPUT_COLOR = texture(Texture, vTextureCoordinate) * vColor;
}