﻿Shader "BASICxSHADER/Fog/Distance" {
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _HorizontalColor("Horizontal Color", Color) = (1,1,1,1)
        _VerticalColor("Vertical Color", Color) = (1,1,1,1)
        _HorizontalDensity("Horizontal Density", Range(0.0, 1.0)) = 0.1
        _VerticalDensity("Vertical Density", Range(0.0, 1.0)) = 0.1
    }
    SubShader{
      Cull off
      Pass {
        Tags { "LightMode" = "ForwardBase" }

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "Lighting.cginc"
        #include "AutoLight.cginc"

        // Properties
        uniform sampler2D _MainTex;
        uniform float4    _MainTex_ST;
        float4 _HorizontalColor;
        float4 _VerticalColor;
        float _HorizontalDensity;
        float _VerticalDensity;

        // Vertex Input
        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 uv     : TEXCOORD0;
        };

    // Vertex to Fragment
    struct v2f {
      float4 pos      : SV_POSITION;
      float3 normal   : NORMAL;
      float4 posWorld : TEXCOORD0;
      float2 uv     : TEXCOORD1;
      float3 viewDir : TEXCOORD2;
      SHADOW_COORDS(2)
    };

    //------------------------------------------------------------------------
    // Vertex Shader
    //------------------------------------------------------------------------
    v2f vert(appdata v) {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);
      o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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
        fixed4 mainTex = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);
        fixed3 diffuse = _LightColor0.rgb * mainTex.rgb;

        // Fog
        float2 horizontalDelta = float2(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.z) - float2(i.posWorld.x, i.posWorld.z);
        float horizontalDistance = length(horizontalDelta);
        float horizontalDensity = horizontalDistance * _HorizontalDensity;
        float horizontalFogCoord = exp2(-horizontalDensity * horizontalDensity);

        float verticalDensity = (i.posWorld.y - _WorldSpaceCameraPos.y) * _VerticalDensity;
        float verticalFogCoord = exp2(-verticalDensity * verticalDensity);

        //shadow
        //half3 normal = normalize(i.normal);
        //half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
        //half NdotL = saturate(dot(normal, lightDir));
        fixed shadow = SHADOW_ATTENUATION(i);
        //float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

        // Color
        fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * mainTex.rgb;
        fixed4 color = fixed4(ambient + diffuse, 1.0) * shadow;
        color.rgb = lerp(_HorizontalColor.rgb, color.rgb, horizontalFogCoord) + _VerticalColor.rgb * verticalDensity;

        return color;
      }
      ENDCG
    }
            UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}