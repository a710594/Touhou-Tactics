Shader "BASICxSHADER/Unlit" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _FlashAmount("Flash Amount", Range(0,1)) = 0
    }
    SubShader{
        Tags
        {
            "Queue" = "Transparent"
        }
      Pass {
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        // Properties
        float4 _Color;
        float _FlashAmount;

        float4 vert(float4 vertex : POSITION) : SV_POSITION {
          return UnityObjectToClipPos(vertex);
        }

        fixed4 frag() : SV_Target {
          fixed4 color = fixed4(0, 0, 0, 0);
          color = lerp(color, _Color, _FlashAmount);
          return color;
        }
        ENDCG
      }
    }
}