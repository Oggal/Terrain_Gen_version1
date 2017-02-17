Shader "Custom/OggalShader_V1" {
	
	Properties{
		_Tint("Tint",Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{

			Pass{
				CGPROGRAM

				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#include "UnityCG.cginc"
				
	float4 _Tint;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	
	struct VertexData {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
		
	};

	struct Interpolators {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 localPos : TEXCOORD1;
	};

				Interpolators MyVertexProgram(VertexData v) {
					Interpolators i;

					i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
					i.localPos = (v.pos.y+25)/50;
					i.pos =mul(UNITY_MATRIX_MVP,v.pos);
					return i;
				}
			
				float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
					return tex2D(_MainTex,i.uv) *float4(i.localPos,1) *_Tint;
				}

				ENDCG
			}
		}
}
