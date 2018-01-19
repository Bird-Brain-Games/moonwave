Shader "Custom/SynthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SynthTex ("SynthWave Texture", 2D) = "white"{}
		_Scan ("Scan Position", Float) = 0.0
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Thickness("Thickness", Range(0, 5.0)) = 1.0
        _Color ("Color", Color) = (0,0,0,1)
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
	        };

	        float4 _OutlineColor;
	        float _Thickness;

	        v2f vert(appdata v)
	        {
		        v.vertex.xyz *= _Thickness;
		        v2f o;
		        o.pos = UnityObjectToClipPos(v.vertex);
		        return o;
	        }

			half4 frag(v2f i) : COLOR
			{

				return _OutlineColor;

			}
			ENDCG
		}
		Pass // Lighting Render
		{
			ZWrite On

			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}

		}

        Pass // SynthScan
        {
            CGPROGRAM
            
			#pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            
            struct v2f {
                float2 uv : TEXCOORD0;
				float2 model : TEXCOORD1;
				float4 pos : SV_POSITION;
            };

            v2f vert (
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0 // texture coordinate input
                )
            {
                v2f o;
                o.uv = uv;
				o.model = vertex.xy;
                o.pos = UnityObjectToClipPos(vertex);
				
                return o;
            }

            sampler2D _MainTex;
			sampler2D _SynthTex;
			float _Scan;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 c;

                // Any fragment below the scanner is discarded
                // clip() discards if the numeber is negative
                clip(i.model.y - _Scan); 

                // Otherwise, change to grid texture
				c = tex2D (_SynthTex, i.uv);

				return c;
            }
            ENDCG
		}
    }
	FallBack "Diffuse"
}