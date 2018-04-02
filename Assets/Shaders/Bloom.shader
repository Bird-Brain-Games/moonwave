// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Bloom" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_BloomThreshold ("Bloom Threshold", Range(0,1)) = 0.7
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			
		}
		ENDCG

        // Grab the screen from the surface render
        GrabPass
        {
            "_PreBloom"
        }

		Pass // BRIGHT PASS
        {
            CGPROGRAM
            
			#pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
			#include "UnityCG.cginc"
            
            struct v2f {
                float2 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _MainTex;
			sampler2D _PreBloom;
			float _BloomThreshold;

            fixed4 frag (v2f i) : SV_Target
            {
				
				fixed4 tex;
				tex = tex2D (_PreBloom, i.grabPos);

				// If the pixel is above the threshold keep its color
				float clipValue = tex - _BloomThreshold;
				clip(-clipValue);

				// Anything below the threshold becomes black
				return 0;
            }
            ENDCG
		}
		//Save the bright pass
        GrabPass
        {
            "_BrightPass"
        }
		
		Pass // BLUR PASS
        {
            CGPROGRAM

            
			#pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
			#include "UnityCG.cginc"
            
            struct v2f {
                float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPos.xy = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _MainTex;
			sampler2D _BrightPass;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 tex;
				tex = tex2D (_BrightPass, i.grabPos);
				

				return tex;
            }
            ENDCG
		}
	}
	FallBack "Diffuse"
}
