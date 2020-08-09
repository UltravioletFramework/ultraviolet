float ComputeFogFactor(vec4 position)
{
	return clamp(dot(position, FogVector), 0.0, 1.0);
}

void ApplyFog(inout vec4 color, float fogFactor)
{
	color.rgb = mix(color.rgb, FogColor * color.a, fogFactor);
}