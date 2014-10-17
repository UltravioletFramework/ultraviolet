#version 130

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

in  vec4 uv_Position0;
in  vec4 uv_Color0;

out vec4 vColor;

void main()
{
	gl_Position = Projection * View * World * uv_Position0;
	vColor      = DiffuseColor * uv_Color0;
}