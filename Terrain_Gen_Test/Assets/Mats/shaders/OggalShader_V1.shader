Shader "Custom/OggalShader_V1" {
	
	Properties{
		_Tint("Tint",Color) = (1,1,1,1)
		[NoScaleOffset]_Rtex("Low Texture", 2D) = "red" {}
		[NoScaleOffset]_Gtex("Mid Texture", 2D) = "green" {}
		[NoScaleOffset]_Btex("High Texture", 2D) = "blue" {}
		_Range("Range" , Float) = 10
	}

		SubShader{

			Pass{
				CGPROGRAM

				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#include "UnityCG.cginc"
				
	float4 _Tint;
	sampler2D _Rtex;
	sampler2D _Gtex;
	sampler2D _Btex;
	float4 _Rtex_ST;
	float _Range;

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

					i.uv = v.uv * _Rtex_ST.xy + _Rtex_ST.zw;
					//i.localPos = (v.pos.y+25)/50;
					i.localPos = 0;
					i.pos =mul(UNITY_MATRIX_MVP,v.pos);
					float x = v.pos.y - 15;

					i.localPos.r = max(0, 1- ((x / _Range)*(x / _Range)));
					x += 18;
					i.localPos.g = max(0, 1 - ((x / _Range)*(x / _Range)));
					x += 18;
					i.localPos.b = max(0, 1 - ((x / _Range)*(x / _Range)));
					return i;
				}
			
				float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
					return (tex2D(_Rtex,i.uv) * i.localPos.r +tex2D(_Gtex,i.uv) * i.localPos.g +tex2D(_Btex, i.uv) * i.localPos.b)*_Tint;
				}

				ENDCG
			}
		}
}
