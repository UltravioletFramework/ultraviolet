#version 140

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

in  vec4 my_position;
in  vec4 my_color;

out vec4 vColor;

void main()
{
	gl_Position = my_position * World * View * Projection;
	vColor      = DiffuseColor * my_color;
}