Shader "Custom/PickUp" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Scan ("Scan Position", Float) = 0.0
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Thickness("Thickness", Range(0, 5.0)) = 1.0
	}

		SubShader
    {
        Pass // Render the Outline
		{
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            struct appdata{
		        float4 vertex : POSITION;
	        };

	        struct v2f {
		        float4 pos : POSITION;
				float2 model : TEXCOORD1;
	        };

	        float4 _OutlineColor;
	        float _Thickness;
			float _Scan;

	        v2f vert(appdata v)
	        {
		        v.vertex.xyz *= _Thickness;
		        v2f o;
		        o.pos = UnityObjectToClipPos(v.vertex);
				o.model = v.vertex.xy;
		        return o;
	        }

			half4 frag(v2f i) : COLOR
			{
				clip(i.model.y - _Scan); 
				return _OutlineColor;

			}
			ENDCG
		}

		Pass // Render the inside
		{
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            struct appdata{
		        float4 vertex : POSITION;
	        };

	        struct v2f {
		        float4 pos : POSITION;
				float2 model : TEXCOORD1;
	        };

	        float4 _Color;
			float _Scan;

	        v2f vert(appdata v)
	        {
		        v2f o;
		        o.pos = UnityObjectToClipPos(v.vertex);
				o.model = v.vertex.xy;
		        return o;
	        }

			half4 frag(v2f i) : COLOR
			{
				clip(i.model.y - _Scan); 
				return _Color;

			}
			ENDCG
		}
	
	}
    Fallback "Diffuse"
}
