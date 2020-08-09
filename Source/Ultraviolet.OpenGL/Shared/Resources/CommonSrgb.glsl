vec4 Srgb2Linear(vec4 color)
{
	float r = color.r < 0.04045 ? (1.0 / 12.92) * color.r : pow((color.r + 0.055) * (1.0 / 1.055), 2.4);
	float g = color.g < 0.04045 ? (1.0 / 12.92) * color.g : pow((color.g + 0.055) * (1.0 / 1.055), 2.4);
	float b = color.b < 0.04045 ? (1.0 / 12.92) * color.b : pow((color.b + 0.055) * (1.0 / 1.055), 2.4);
	return vec4(r, g, b, color.a);
}

vec4 Linear2Srgb(vec4 color)
{
	float r = color.r < 0.0031308 ? 12.92 * color.r : 1.055 * pow(color.r, 1.0 / 2.4) - 0.055;
	float g = color.g < 0.0031308 ? 12.92 * color.g : 1.055 * pow(color.g, 1.0 / 2.4) - 0.055;
	float b = color.b < 0.0031308 ? 12.92 * color.b : 1.055 * pow(color.b, 1.0 / 2.4) - 0.055;
	return vec4(r, g, b, color.a);
}

vec4 ConvertColor(vec4 c)
{
	return SrgbColor ? Srgb2Linear(c) : c;
}