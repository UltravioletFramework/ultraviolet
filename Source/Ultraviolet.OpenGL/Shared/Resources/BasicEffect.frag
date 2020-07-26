#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.glsl" executing

uniform vec4 FogColor;

in  vec4 vDiffuse;
in  vec4 vSpecular;

DECLARE_OUTPUT_COLOR;

void main()
{
	vec4 color = vDiffuse + vec4(vSpecular.rgb, 0);
	color.rgb = mix(color.rgb, FogColor.rgb, vSpecular.w);
	OUTPUT_COLOR = color;
}