Shader "Custom/TwoTextureShader"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _FogColor ("Fog Color", Color) = (1,1,1,1)
        _FogDensity ("Fog Density", Range(0.0, 1.0)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex1;
            float4 _MainTex1_ST;
            sampler2D _MainTex2;
            float4 _MainTex2_ST;
            float4 _Color1;
            float4 _Color2;
            float4 _FogColor;
            float _FogDensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the first texture
                fixed4 texColor1 = tex2D(_MainTex1, i.uv);
                // Apply the first color
                fixed4 color1 = texColor1 * _Color1;

                // Sample the second texture
                fixed4 texColor2 = tex2D(_MainTex2, i.uv);
                // Apply the second color
                fixed4 color2 = texColor2 * _Color2;

                // Calculate distance from the camera to the fragment
                float3 camPos = _WorldSpaceCameraPos;
                float _distance = distance(i.worldPos, camPos);

                // Combine distance and height fog factors
                float distanceFactor  = exp2(-_FogDensity * _distance * _FogDensity * _distance);
                fixed4 color3 = lerp(_FogColor, color1, distanceFactor);

                // Combine the two colored textures
                fixed4 finalColor = color3 * color3.a + color2 * color2.a;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
