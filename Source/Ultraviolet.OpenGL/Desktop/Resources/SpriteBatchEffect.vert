#version 140

uniform mat4 MatrixTransform;
uniform vec2 TextureSize;
uniform bool SrgbColor;

in vec4 uv_Position0;
in vec4 uv_Color0;
in vec2 uv_TextureCoordinate0;

out vec4 vColor;
out vec2 vTextureCoordinate;

vec4 srgb2linear(vec4 color)
{
   float r = color.r < 0.04045 ? (1.0 / 12.92) * color.r : pow((color.r + 0.055) * (1.0 / 1.055), 2.4);
   float g = color.g < 0.04045 ? (1.0 / 12.92) * color.g : pow((color.g + 0.055) * (1.0 / 1.055), 2.4);
   float b = color.b < 0.04045 ? (1.0 / 12.92) * color.b : pow((color.b + 0.055) * (1.0 / 1.055), 2.4);
   return vec4(r, g, b, color.a);
}

void main()
{
	gl_Position        = uv_Position0 * MatrixTransform;
	vColor             = SrgbColor ? srgb2linear(uv_Color0) : uv_Color0;
	vTextureCoordinate = vec2(uv_TextureCoordinate0.x / TextureSize.x, 1.0 - (uv_TextureCoordinate0.y / TextureSize.y));
}