﻿Shader "BASICxSHADER/Shadow/Shadow" {
    Properties{
      _MainTex("Main Texture", 2D) = "white" {}
      _Ambient("Ambient", Range(0.0, 1.0)) = 0.1
      _ShadowDensity("Shadow Density", Range(0.0, 1.0)) = 0.1

    }
        SubShader{
          Cull off
          Blend SrcAlpha OneMinusSrcAlpha // 混合的参数
          Pass {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
          "Queue" = "Transparent" "RenderType" = "Transparent"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            // Properties
            uniform sampler2D _MainTex;
            float _Ambient;
            float _ShadowDensity;

            // Vertex Input
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            // Vertex to Fragment
            struct v2f {
                float4 pos    : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            //------------------------------------------------------------------------
            // Vertex Shader
            //------------------------------------------------------------------------
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);
                o.uv = v.uv;
                o.viewDir = WorldSpaceViewDir(v.vertex);

                // Pass shadow coordinates to pixel shader
                TRANSFER_SHADOW(o);
              return o;
            }

            //------------------------------------------------------------------------
            // Fragment Shader
            //------------------------------------------------------------------------
            fixed4 frag(v2f i) : SV_Target {
                // Color Map
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 diffuse = fixed4(_LightColor0.rgb, 1) * mainTex.rgba;

                //shadow
                //half3 normal = normalize(i.normal);
                //half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //half NdotL = saturate(dot(normal, lightDir));
                fixed shadow = SHADOW_ATTENUATION(i);
                shadow = smoothstep(0.5, 0.6, shadow);
                shadow = clamp(shadow, _ShadowDensity, 1);
                float mid = (_ShadowDensity + 1) * 0.5f;
                //float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

                // Color
                fixed4 ambient = _Ambient * mainTex.rgba;
                //fixed4 color = fixed4(ambient + diffuse) * shadow;
                fixed3 color3 = diffuse.rgb * shadow;
                fixed4 color = fixed4(color3, diffuse.a);

                return color;
              }
              ENDCG
            }
                    UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        }
}