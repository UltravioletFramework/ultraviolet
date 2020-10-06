#includeres "Ultraviolet.OpenGL.Resources.BasicEffectPreamble.glsl" executing

#define SKINNED_EFFECT_MAX_BONES 72
uniform mat3x4 Bones[SKINNED_EFFECT_MAX_BONES];

void Skin(inout vec4 position, inout vec3 normal, in ivec4 indices, in vec4 weights, const int boneCount)
{
	mat3x4 skinning = mat3x4(0);

	for (int i = 0; i < boneCount; i++) 
	{
		skinning += mat3x4(Bones[indices[i]]) * weights[i];
	}

	position.xyz = position * skinning;
	normal = normal * mat3x3(skinning);
}
