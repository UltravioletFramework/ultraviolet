struct CommonVSOutput
{
	 vec4 Pos_ps;
	 vec4 Diffuse;
	 vec3 Specular;
	float FogFactor;
};

CommonVSOutput ComputeCommonVSOutput(vec4 position)
{
	CommonVSOutput vout;

	vout.Pos_ps = position * WorldViewProj;
	vout.Diffuse = DiffuseColor;
	vout.Specular = vec3(0);
	vout.FogFactor = ComputeFogFactor(position);

	return vout;
}

#define SetCommonVSOutputParams \
	gl_Position = cout.Pos_ps; \
	vDiffuse = cout.Diffuse; \
	vSpecular = vec4(cout.Specular, cout.FogFactor);

#define SetCommonVSOutputParamsNoFog \
	gl_Position = cout.Pos_ps;\
	vDiffuse = cout.Diffuse;

vec2 FlipTextureCoordinates(vec2 coords)
{
	return vec2(coords.x, 1.0 - coords.y);
}