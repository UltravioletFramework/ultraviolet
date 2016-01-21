#version 140

uniform mat4 MatrixTransform;

in vec4 uv_Position0;
in vec4 uv_Position1;
in vec4 uv_Position2;
in vec4 uv_Position3;
in vec4 uv_Position4;
in vec4 uv_Color0;

out vec4 vColor;

void main()
{
	mat4 world = mat4(uv_Position1, uv_Position2, uv_Position3, uv_Position4);
	gl_Position = MatrixTransform * world * uv_Position0;
	vColor = uv_Color0;
}