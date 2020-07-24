#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.fragh" executing

uniform vec4 FogColor;

in  vec4 vDiffuse;
in  vec4 vSpecular;

out vec4 fColor;

void main()
{
	vec4 color = vDiffuse + vec4(vSpecular.rgb, 0);
	color.rgb = mix(color.rgb, FogColor.rgb, vSpecular.w);
	fColor = color;
}