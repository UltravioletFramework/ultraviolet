#version 140

#define INCLUDE_MATRICES
#define INCLUDE_LIGHTING
#define INCLUDE_SRGB

#includeres "Ultraviolet.OpenGL.Resources.BasicEffectCommon.verth" executing

in  vec4 uv_Position0;
in  vec4 uv_Color0;

out vec4 vColor;

void main()
{
	gl_Position = transform_position(uv_Position0);
	vColor      = process_color(DiffuseColor) * process_color(uv_Color0);
}