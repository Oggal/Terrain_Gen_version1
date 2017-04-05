#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"
				
	float4 _Tint;
	sampler2D _Rtex;
	sampler2D _Gtex;
	sampler2D _Btex;
	sampler2D _aTex;
	float4 _Rtex_ST;
	float _RRange;
	float _GRange;
	float _BRange;
	float _RMid;
	float _GMid;
	float _BMid;

	struct VertexData {
		float4 pos : POSITION;
		float3 norm : NORMAL;
		float2 uv : TEXCOORD0;
		
	};

	struct Interpolators {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 localPos : TEXCOORD1;
		float3 norm : TEXCOORD2;
	};


	Interpolators MyVertexProgram(VertexData v) {
		Interpolators i;

		i.uv = v.uv * _Rtex_ST.xy + _Rtex_ST.zw;
		//i.localPos = (v.pos.y+25)/50;
		i.localPos = 0;
		i.pos =mul(UNITY_MATRIX_MVP,v.pos);

		float x = v.pos.y - _RMid;
		i.localPos.r = max(0, min(1,2- ((x / _RRange)*(x / _RRange))));

		x = v.pos.y - _GMid;
		i.localPos.g = max(0,min(1, 2 - ((x / _GRange)*(x / _GRange))));

		x = v.pos.y - _BMid;
		i.localPos.b = max(0,min(1, 2 - ((x / _BRange)*(x / _BRange))));

		i.norm = UnityObjectToWorldNormal(v.norm);
		return i;
	}
			
	float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
		float3 TerrainMat = (tex2D(_Rtex, i.uv) * i.localPos.r + tex2D(_Gtex, i.uv) * i.localPos.g + tex2D(_Btex, i.uv) * i.localPos.b + tex2D(_aTex, i.uv)*(1 - i.localPos.r - i.localPos.g - i.localPos.b));



		//i.norm = normalize(i.norm); 
		float3 lightSun = _WorldSpaceLightPos0.xyz;
		float3 colSun = _LightColor0.rgb;
		float3 diffuse =TerrainMat* colSun * DotClamped(lightSun, i.norm);


		//return DotClamped(lightSun, i.norm);
		return float4(diffuse,1) *_Tint;
	}
#endif