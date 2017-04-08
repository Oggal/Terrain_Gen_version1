#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"
				
	float4 _Tint;
	sampler2D _Rtex;
	sampler2D _Gtex;
	sampler2D _Btex;
	sampler2D _aTex;
	sampler2D _Roadtex;
	float4 _Rtex_ST;
	float _RRange;
	float _GRange;
	float _BRange;
	float _RMid;
	float _GMid;
	float _BMid;
	float _RoadRange;
	
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

		float x = v.pos.y - _RoadRange;
		i.localPos.r = max(0, min(1,2- ((x )*(x ))));

		i.norm = UnityObjectToWorldNormal(v.norm);
		return i;
	}
			
	float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
		float3 TerrainMat = (tex2D(_Roadtex, i.uv) * i.localPos.r);
		float4 tex = (float4(TerrainMat, (i.localPos.r)));
		clip(tex.a-0.05 );


		//i.norm = normalize(i.norm); 
		float3 lightSun = _WorldSpaceLightPos0.xyz;
		float3 colSun = _LightColor0.rgb;
		float3 diffuse =tex* colSun * DotClamped(lightSun, i.norm);

		//return DotClamped(lightSun, i.norm);
		return float4(diffuse,1) *_Tint;
	}
#endif