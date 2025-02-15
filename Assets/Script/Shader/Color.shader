﻿Shader "BASICxSHADER/Texturing/Color" {
    Properties{
      _MainTex("Main Texture", 2D) = "white" {}
      _Ambient("Ambient", Range(0.0, 1.0)) = 0.1
    }
        SubShader{
          Pass {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
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
              // Vector
              half3 normal = normalize(i.normal);
              half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

              // Dot
              half NdotL = saturate(dot(normal, lightDir));

              // Color Map
              fixed4 mainTex = tex2D(_MainTex, i.uv);
              fixed3 diffuse = _LightColor0.rgb * mainTex.rgb * NdotL;

              // Color
              fixed3 ambient = _Ambient * mainTex.rgb;
              fixed shadow = SHADOW_ATTENUATION(i);
              fixed4 color = fixed4(ambient + diffuse * shadow, 1.0);

              return color;
            }
            ENDCG
          }
          // Shadow casting support.
      UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
      }
}