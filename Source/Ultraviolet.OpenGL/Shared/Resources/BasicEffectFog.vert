#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.verth" executing

#define INCLUDE_MATRICES
#define INCLUDE_LIGHTING
#define INCLUDE_FOG
#define INCLUDE_SRGB

#includeres "Ultraviolet.OpenGL.Resources.BasicEffectCommon.verth" executing

in  vec4 uv_Position0;

out vec4 vDiffuse;
out vec4 vSpecular;

void main()
{
	vec4 pos_ws = uv_Position0 * World;
	vec4 pos_vw = pos_ws * View;
	vec4 pos_ps = pos_vw * Projection;
	
	float fog_factor = compute_fog_factor(length(EyePosition - pos_ws.xyz));

	gl_Position = pos_ps;
	vDiffuse    = DiffuseColor;
	vSpecular	= vec4(0, 0, 0, fog_factor);
}