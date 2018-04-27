#includeres "Ultraviolet.OpenGL.Resources.HeaderES.verth" executing
#includeres "Ultraviolet.OpenGL.Resources.SrgbConversion.verth" executing

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;
uniform bool SrgbColor;

DECLARE_INPUT_POSITION;		// uv_Position0
DECLARE_INPUT_COLOR;		// uv_Color0
DECLARE_INPUT_TEXCOORD;		// uv_TextureCoordinate0

DECLARE_OUTPUT_COLOR;		// vColor
DECLARE_OUTPUT_TEXCOORD;	// vTextureCoordinate

void main()
{
    gl_Position        = uv_Position0 * MatrixTransform;
    vColor             = SrgbColor ? srgb2linear(uv_Color0) : uv_Color0;
    vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}