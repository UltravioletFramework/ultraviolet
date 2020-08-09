#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.glsl" executing

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;
uniform bool SrgbColor;

#includeres "Ultraviolet.OpenGL.Resources.CommonSrgb.glsl" executing

 in vec4 uv_Position0;
 in vec4 uv_Color0;
 in vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

void main()
{
	gl_Position        = uv_Position0 * MatrixTransform;
	vColor             = ConvertColor(uv_Color0);
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}