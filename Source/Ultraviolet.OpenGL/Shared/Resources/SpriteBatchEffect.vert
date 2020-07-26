#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.glsl" executing
#includeres "Ultraviolet.OpenGL.Resources.SrgbConversion.glsl" executing

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;
uniform bool SrgbColor;

in vec4 uv_Position0;
in vec4 uv_Color0;
in vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = uv_Position0 * MatrixTransform;
	vColor             = SrgbColor ? srgb2linear(uv_Color0) : uv_Color0;
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}