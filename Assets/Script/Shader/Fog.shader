﻿Shader "BASICxSHADER/Fog/Distance" {
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _HorizontalColor("Horizontal Color", Color) = (1,1,1,1)
        _VerticalColor("Vertical Color", Color) = (1,1,1,1)
        _HorizontalDensity("Fog Density", Range(0.0, 1.0)) = 0.1
        _VerticalDensity("Fog Density", Range(0.0, 1.0)) = 0.1
    }
    SubShader{
      Cull off
      Pass {
        Tags { "LightMode" = "ForwardBase" }

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        // Properties
        uniform fixed4    _LightColor0;
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
        fixed4 mainTex = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);
        //fixed3 diffuse = _LightColor0.rgb * mainTex.rgb * NdotL;
        fixed3 diffuse = _LightColor0.rgb * mainTex.rgb;

        // Fog
        float2 horizontalDelta = float2(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.z) - float2(i.posWorld.x, i.posWorld.z);
        float horizontalDistance = length(horizontalDelta);
        float horizontalDensity = horizontalDistance * _HorizontalDensity;
        float horizontalFogCoord = exp2(-horizontalDensity * horizontalDensity);

        float verticalDensity = (i.posWorld.y - _WorldSpaceCameraPos.y) * _VerticalDensity;
        float verticalFogCoord = exp2(-verticalDensity * verticalDensity);
        //float density = length(_WorldSpaceCameraPos - i.posWorld) * _HorizontalDensity + (i.posWorld.y - _WorldSpaceCameraPos.y) * _VerticalDensity;
        //float fogCoord = exp2(-density * density);

        // Color
        fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * mainTex.rgb;
        fixed4 color = fixed4(ambient + diffuse, 1.0);
        color.rgb = lerp(_HorizontalColor.rgb, color.rgb, horizontalFogCoord) + _VerticalColor.rgb * verticalDensity;

        return color;
      }
      ENDCG
    }
    }
}