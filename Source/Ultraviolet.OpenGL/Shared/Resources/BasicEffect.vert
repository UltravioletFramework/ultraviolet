#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.verth" executing

#define INCLUDE_MATRICES
#define INCLUDE_LIGHTING
#define INCLUDE_SRGB

#includeres "Ultraviolet.OpenGL.Resources.BasicEffectCommon.verth" executing

in  vec4 uv_Position0;

out vec4 vDiffuse;
out vec4 vSpecular;

void main()
{
	gl_Position = uv_Position0 * WorldViewProj;
	vDiffuse    = DiffuseColor;
	vSpecular   = vec4(0, 0, 0, 0);
}