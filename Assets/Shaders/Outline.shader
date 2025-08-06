Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _OutlineColor;
            float _Outline;

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                pos.xyz += norm * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // 기본 머티리얼 Pass
        Pass
        {
            Name "BASE"
            Cull Back
            ZWrite On
            ZTest LEqual

            SetTexture [_MainTex] { combine texture }
        }
    }

    FallBack "Diffuse"
}
