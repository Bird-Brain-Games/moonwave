//  Shader for outlines from http://www.shaderslab.com/demo-19---outline-3d-model.html

Shader "Custom/Outline_Normals" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Outline ("_Outline",float) = 0
        _OutlineColor ("OutlineColor", Color) = (1, 1, 1, 1)
        _Color ("Color", Color) = (5,5,5,1)
    }
    SubShader {
        Pass {
            Tags { "RenderType"="Opaque" }
            Cull Front
 
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
            };
 
            float _Outline;
            float4 _OutlineColor;
 
            float4 vert(appdata_base v) : SV_POSITION {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
                normal.x *= UNITY_MATRIX_P[0][0];
                normal.y *= UNITY_MATRIX_P[1][1];
                o.pos.xy += normal.xy * _Outline;
                return o.pos;
            }
 
            half4 frag(v2f i) : COLOR {
                return _OutlineColor;
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}
