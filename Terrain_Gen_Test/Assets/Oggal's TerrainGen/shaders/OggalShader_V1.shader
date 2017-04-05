Shader "Custom/OggalShader_V1" {
//Oggal's Terrain Gen
	Properties{
		_Tint("Tint",Color) = (1,1,1,1)
		[NoScaleOffset]_Rtex("High Texture", 2D) = "red" {}
		_RMid("Center",Float) = 15
		_RRange("Range",Float) = 10
		[NoScaleOffset]_Gtex("Mid Texture", 2D) = "green" {}
		_GMid("Center",Float) = 0
		_GRange("Range",Float) = 10
		[NoScaleOffset]_Btex("Low Texture", 2D) = "blue" {}
		_BMid("Center",Float) = -15
		_BRange("Range" , Float) = 10

		[NoScaleOffset]_aTex("Base Texture", 2D) = "blue" {}

	}

	SubShader{

		Pass{
			Tags{
			"LightMode" = "ForwardBase"
			}
			CGPROGRAM

			#pragma target 3.0

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "MyLighting.cginc"

			ENDCG
		}
		Pass{
			Tags{
				"LightMode" = "ForwardBase"
			}
			Blend One One
			ZWrite Off
			CGPROGRAM
			#pragma target 3.0

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "MyLighting.cginc"
			ENDCG
		}

	}
}
