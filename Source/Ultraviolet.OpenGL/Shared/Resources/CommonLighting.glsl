void AddSpecular(inout vec4 color, vec3 specular)
{
    color.rgb += specular * color.a;
}

struct ColorPair
{
     vec3 Diffuse;
     vec3 Specular;
};

ColorPair ComputeLights(vec3 eyeVector, vec3 worldNormal, const int numLights)
{
    mat3 lightDirections = mat3(0);
    mat3 lightDiffuse = mat3(0);
    mat3 lightSpecular = mat3(0);
    mat3 halfVectors = mat3(0);

    for (int i = 0; i < numLights; i++)
    {
        lightDirections[i] = mat3(DirLight0Direction, DirLight1Direction, DirLight2Direction)[i];
        lightDiffuse[i] = mat3(DirLight0DiffuseColor, DirLight1DiffuseColor, DirLight2DiffuseColor)[i];
        lightSpecular[i] = mat3(DirLight0SpecularColor, DirLight1SpecularColor, DirLight2SpecularColor)[i];

        halfVectors[i] = normalize(eyeVector - lightDirections[i]);
    }

    vec3 dotL = worldNormal * -lightDirections;
    vec3 dotH = worldNormal * halfVectors;

    vec3 zeroL = step(0.0, dotL);

    vec3 diffuse = zeroL * dotL;
    vec3 specular = pow(max(dotH, 0.0) * zeroL, vec3(SpecularPower));

    ColorPair result;

    result.Diffuse = (lightDiffuse * diffuse) * DiffuseColor.rgb + EmissiveColor;
    result.Specular = (lightSpecular * specular) * SpecularColor.rgb;

    return result;
}

CommonVSOutput ComputeCommonVSOutputWithLighting(vec4 position, vec3 normal, const int numLights)
{
    CommonVSOutput vout;

    vec4 pos_ws = position * World;
    vec3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    vec3 worldNormal = normalize((vec4(normal, 0) * WorldInverseTranspose).xyz);

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, numLights);

    vout.Pos_ps = position * WorldViewProj;
    vout.Diffuse = vec4(lightResult.Diffuse, DiffuseColor.a);
    vout.Specular = lightResult.Specular;
    vout.FogFactor = ComputeFogFactor(position);

    return vout;
}

struct CommonVSOutputPixelLighting
{
    vec4 Pos_ps;
    vec3 Pos_ws;
    vec3 Normal_ws;
    float FogFactor;
};

CommonVSOutputPixelLighting ComputeCommonVSOutputPixelLighting(vec4 position, vec3 normal)
{
    CommonVSOutputPixelLighting vout;

    vout.Pos_ps = (position * WorldViewProj);
    vout.Pos_ws = (position * World).xyz;
    vout.Normal_ws = normalize((vec4(normal, 0) * WorldInverseTranspose).xyz);
    vout.FogFactor = ComputeFogFactor(position);

    return vout;
}

#define SetCommonVSOutputParamsPixelLighting \
    gl_Position = cout.Pos_ps; \
    vPositionWS = vec4(cout.Pos_ws, cout.FogFactor); \
    vNormalWS = cout.Normal_ws;