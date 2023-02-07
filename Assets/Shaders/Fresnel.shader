Shader "Unlit/FresnelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelIntensity ("Intensity", Range(0, 10)) = 0
        _FresnelRamp ("Ramp", Range(0, 10)) = 0
        _Tint ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FresnelIntensity, _FresnelRamp;
            float4 _Tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float fresnelAmout = 1-max(0, dot(i.normal, i.viewDir));
                fresnelAmout = pow(fresnelAmout, _FresnelRamp) * _FresnelIntensity;
                return fresnelAmout * _Tint;
            }
            ENDCG
        }
    }
}
