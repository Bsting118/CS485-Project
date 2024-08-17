Shader"HeyBlairGames/PlanetTextureGenerator/Runtime/PlanetParallaxSpecularSRP"
{
    Properties
    {
        _GroundMap ("Ground Map", 2D) = "white" {}
        _SpecularColour ("Specular Colour", Color) = (1, 1, 1, 1)
        _Shininess ("Shininess", Range(1, 1000)) = 10

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Parallax ("Height", Range(0.005, 0.08)) = 0.02

        _IllumMap ("Illumination Map", 2D) = "black" {}
        _IlluminationFactor("Illumination Factor", Range(0, 3)) = 1.0
        _PlayerPositionToIlluminate ("Player Position For Lighting", Vector) = (0, 0, 0)
        _CenterWSPosition ("Centered Position", Vector) = (0, 0, 0) // New property
        //_LightDirection ("Light Direction", Vector) = (0, 1, 0, 0)
        _CityEmissionStrength ("City Emission Strength", Range(0, 1)) = 0.5

        _CloudMap ("Cloud Map", 2D) = "black" {}
        _CloudNormalMap ("Cloud Normal Map", 2D) = "bump" {}
        _CloudAlpha ("Cloud Alpha", Range(0, 1.0)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // CBuffer for uniform variables
            CBUFFER_START(UnityPerMaterial)

float4 _SpecularColour;
//float4 _LightDirection; // The light direction vector
float3 _PlayerPositionToIlluminate; // ADDED
float3 _CenterWSPosition;
float _Shininess;
float _Parallax;
float _IlluminationFactor;
float _CityEmissionStrength;
float _CloudAlpha;
            TEXTURE2D(_GroundMap);
            SAMPLER(sampler_GroundMap);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_IllumMap);
            SAMPLER(sampler_IllumMap);
            TEXTURE2D(_CloudMap);
            SAMPLER(sampler_CloudMap);
            TEXTURE2D(_CloudNormalMap);
            SAMPLER(sampler_CloudNormalMap);

            CBUFFER_END

struct Attributes
{
    float3 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float2 uv0 : TEXCOORD0;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    // List Texture Coordinate channels:
    float2 uv0 : TEXCOORD0;
    float3 normalWS : TEXCOORD1;
    float3 tangentWS : TEXCOORD2;
    float3 bitangentWS : TEXCOORD3;
    float3 positionWS : TEXCOORD4; // Add this line to store world space position
};

Varyings Vert(Attributes input)
{
    Varyings output;

    // Convert object space to world space
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
    float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz); // Resolved warning
    float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentOS.w;

    // Apply transformations
    output.positionHCS = TransformWorldToHClip(positionWS);
    output.uv0 = input.uv0;
    output.normalWS = normalWS;
    output.tangentWS = tangentWS;
    output.bitangentWS = bitangentWS;
    output.positionWS = positionWS; // Store world space position

    return output;
}



half4 Frag(Varyings input) : SV_Target
{
    // Sample textures
    float4 groundColor = SAMPLE_TEXTURE2D(_GroundMap, sampler_GroundMap, input.uv0);
    float4 illumColor = SAMPLE_TEXTURE2D(_IllumMap, sampler_IllumMap, input.uv0);
    float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv0));

    // Calculate TBN matrix
    float3x3 TBN = float3x3(input.tangentWS, input.bitangentWS, input.normalWS);
    float3 normalWS = mul(normalTS, TBN);

    // Obtain main light direction (SRP)
    //float3 lightDir = normalize(_LightDirection.xyz); // Using the new Light Direction
    float3 lightDir = normalize(_PlayerPositionToIlluminate - _CenterWSPosition); // Using player's position for lighting

    // Lighting and Specular calculations
    float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - input.positionHCS.xyz);
    float3 halfDir = normalize(lightDir + viewDir);

    float NdotL = saturate(dot(normalWS, lightDir));
    float NdotH = saturate(dot(normalWS, halfDir));
    float specular = pow(NdotH, _Shininess) * NdotL;

    // Apply illumination factor to the lighting
    float3 color = groundColor.rgb * NdotL + illumColor.rgb * _CityEmissionStrength;
    color += _SpecularColour.rgb * specular;
    color *= _IlluminationFactor; // Apply the illumination factor

    return half4(color, 1.0);
}

            ENDHLSL
        }
    }
}
