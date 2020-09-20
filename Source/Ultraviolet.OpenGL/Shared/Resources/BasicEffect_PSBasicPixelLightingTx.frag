#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

uniform sampler2D Texture;

 in vec2 vTexCoord;
 in vec4 vPositionWS;
 in vec3 vNormalWS;
 in vec4 vDiffuse;

DECLARE_OUTPUT_COLOR

void main()
{
	vec4 color = SAMPLE_TEXTURE2D(Texture, vTexCoord) * vDiffuse;

	vec3 eyeVector = normalize(EyePosition - vPositionWS.xyz);
	vec3 worldNormal = normalize(vNormalWS);

	ColorPair lightResult = ComputeLights(eyeVector, worldNormal, 3);

	color.rgb *= lightResult.Diffuse;

	AddSpecular(color, lightResult.Specular);
	ApplyFog(color, vPositionWS.w);

	OUTPUT_COLOR = color;
}