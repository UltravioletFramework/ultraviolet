#includeres "Ultraviolet.OpenGL.Resources.HeaderES.verth" executing

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

DECLARE_INPUT_POSITION;	// uv_Position0

DECLARE_OUTPUT_COLOR;	// vColor

void main()
{
    gl_Position = uv_Position0 * World * View * Projection;
	vColor      = DiffuseColor;
}