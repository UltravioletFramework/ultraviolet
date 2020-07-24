#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.fragh" executing

uniform      vec4 FogColor;
uniform sampler2D Texture;

in  vec4 vDiffuse;
in  vec4 vSpecular;
in  vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
	vec4 color = texture(Texture, vTextureCoordinate) * vDiffuse + vec4(vSpecular.rgb, 0);
	color.rgb = mix(color.rgb, FogColor.rgb, vSpecular.w);
	fColor = color;
}