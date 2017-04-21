#includeres "TwistedLogik.Ultraviolet.OpenGL.Resources.HeaderES.verth" executing

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;

DECLARE_INPUT_POSITION;		// uv_Position0
DECLARE_INPUT_COLOR;		// uv_Color0
DECLARE_INPUT_INT_TEXCOORD;	// uv_TextureCoordinate0

DECLARE_OUTPUT_COLOR;		// vColor
DECLARE_OUTPUT_TEXCOORD;	// vTextureCoordinate

void main()
{
    gl_Position = MatrixTransform * uv_Position0;
    vColor = uv_Color0;
#ifdef NORMALIZED_TEXCOORDS
    vTextureCoordinate = uv_TextureCoordinate0;
#else
    vTextureCoordinate = vec2(float(uv_TextureCoordinate0.x) / TextureSize.x, 1.0 - (float(uv_TextureCoordinate0.y) / TextureSize.y));
#endif
}