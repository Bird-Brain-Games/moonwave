Shader "Unlit/Sun"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Thickness("Thickness", Range(0, 5.0)) = 1.0
		_Color ("Color", Color) = (0,0,0,1)
	}


	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
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

	ENDCG

	SubShader
	{
		Pass // Render the Outline
		{
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			half4 frag(v2f i) : COLOR
			{

				return _OutlineColor;

			}
			ENDCG
		}

				Pass // Normal Render
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
	}
}
