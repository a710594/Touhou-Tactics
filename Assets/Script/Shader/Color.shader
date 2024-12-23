﻿Shader "BASICxSHADER/Texturing/Color" {
  Properties {
    _MainTex ("Main Texture", 2D) = "white" {}
    _Ambient ("Ambient", Range(0.0, 1.0)) = 0.1
  }
  SubShader {
    Pass {
      Tags { "LightMode" = "ForwardBase" }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      // Properties
      uniform fixed4    _LightColor0;
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
      };

      //------------------------------------------------------------------------
      // Vertex Shader
      //------------------------------------------------------------------------
      v2f vert(appdata v) {
        v2f o;
        o.pos    = UnityObjectToClipPos(v.vertex);
        o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);
        //o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        o.uv     = v.uv;
        return o;
      }

      //------------------------------------------------------------------------
      // Fragment Shader
      //------------------------------------------------------------------------
      fixed4 frag(v2f i) : SV_Target {
        // Vector
        half3 normal   = normalize(i.normal);
        half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

        // Dot
        half NdotL = saturate(dot(normal, lightDir));

        // Fog
        float density = length(_WorldSpaceCameraPos - i.uv) * unity_FogParams.z;
        float fogCoord = exp2(-density * density);

        // Color Map
        fixed4 mainTex = tex2D(_MainTex, i.uv);
        fixed3 diffuse = _LightColor0.rgb * mainTex.rgb * NdotL;

        // Color
        fixed3 ambient = _Ambient * mainTex.rgb;
        fixed4 color = fixed4(ambient + diffuse, 1.0);
        color.rgb = lerp(unity_FogColor.rgb, color.rgb, fogCoord);

        return color;
      }
      ENDCG
    }
  }
}