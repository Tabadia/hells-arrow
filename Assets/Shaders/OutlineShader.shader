Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strength("Line Strength", Range(1, 100)) = 1.0
        _Mode("Mode", Range(0, 1)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Strength;

            sampler2D _CameraDepthTexture;
            float4 _MainTex_TexelSize;
            float _Mode;

            fixed4 frag (v2f i) : SV_Target
            {
                //Pixel locations of neighbouring pixels
                float2 rightNeighbour = i.uv + float2(_MainTex_TexelSize.x, 0);
                float2 leftNeighbour = i.uv - float2(_MainTex_TexelSize.x, 0);
                float2 topNeighbour = i.uv + float2(0, _MainTex_TexelSize.y);
                float2 bottomNeighbour = i.uv - float2(0, _MainTex_TexelSize.y);

                float cDepth = tex2D(_CameraDepthTexture, i.uv);
                float depthN0 = tex2D(_CameraDepthTexture, rightNeighbour);
                float depthN1 = tex2D(_CameraDepthTexture, leftNeighbour);
                float depthN2 = tex2D(_CameraDepthTexture, topNeighbour);
                float depthN3 = tex2D(_CameraDepthTexture, bottomNeighbour);

                //_Mode = step(_Mode, .5);

                //summing up all side values and stuff, idk i wrote it and hoped it worked and it did
                int operation = -1;
                if (step(_Mode, .5) == 1) operation = 1;
               
                //The non inspector strength that is associated with the inspector strength
                float dependentStrength = _Strength * (abs((_Mode - .5)) * 2);
                float pixelColor = _Mode + (abs(cDepth - depthN0) + abs(cDepth - depthN1) + abs(cDepth - depthN2) + abs(cDepth - depthN3)) * dependentStrength * operation;

                return pixelColor;
            }
            ENDCG
        }
    }
}
