#includeres "Ultraviolet.OpenGL.Resources.SharedHeader.glsl" executing

uniform  vec4 DiffuseColor;
uniform  vec3 EmissiveColor;
uniform  vec3 SpecularColor;
uniform float SpecularPower;

uniform  vec3 DirLight0Direction;
uniform  vec3 DirLight0DiffuseColor;
uniform  vec3 DirLight0SpecularColor;

uniform  vec3 DirLight1Direction;
uniform  vec3 DirLight1DiffuseColor;
uniform  vec3 DirLight1SpecularColor;

uniform  vec3 DirLight2Direction;
uniform  vec3 DirLight2DiffuseColor;
uniform  vec3 DirLight2SpecularColor;

uniform  vec3 EyePosition;

uniform  vec3 FogColor;
uniform  vec4 FogVector;

uniform  mat4 World;
uniform  mat4 WorldInverseTranspose;
uniform  mat4 WorldViewProj;

uniform  bool SrgbColor;

#includeres "Ultraviolet.OpenGL.Resources.CommonFog.glsl" executing
#includeres "Ultraviolet.OpenGL.Resources.CommonVert.glsl" executing
#includeres "Ultraviolet.OpenGL.Resources.CommonLighting.glsl" executing
#includeres "Ultraviolet.OpenGL.Resources.CommonSrgb.glsl" executing
