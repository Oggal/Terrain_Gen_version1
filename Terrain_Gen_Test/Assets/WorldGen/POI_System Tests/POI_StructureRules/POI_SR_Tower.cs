using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class POI_SR_Tower : POI_StructureRule
{
	#region Tower Vertex Placement Vars
	public Vector2Int Sides = new Vector2Int(3,4);                           //Number of sides/corners on the tower
	public float Radius = 1;                        //Base Radius from center to corner
	public Vector2 RadiusVariance = Vector2.zero;   //Distance to vary radius to center of corners (Min,Max)
	public Vector2 CornerDrift = Vector2.zero;      //Amount the corners should drift from center (Min,Max)
	public Vector2 RadiDrift = Vector2.zero;        //Amount the angle of the walls should drift from even (min,max)
	public bool UseEvenLayers = true;               //If false, we build new corners each level
	public Vector2Int Layers = new Vector2Int(1,2);                          //Number of levels to build, 0 is 2D
	public float LayerHeight = 1;                   //Units between levels
	public Vector2 LayerHeightVariance = Vector2.zero;  //Distance to varry height of each level
	public Vector2 LayerCenterDrift = Vector2.zero;
	public bool UseCompoundLayerDrift = false;
	public Vector2 CornerHeightDrift = Vector2.zero;    //Distance to varry height of each corner
	public bool BuildEndCaps = false;
	public Vector2 BottomEndCapHeight = Vector2.zero;
	public Vector2 TopEndCapHeight = Vector2.zero;
	#endregion


	#region Tower Vertex UV Vars

	public enum UV_Generation{
		X_AXIS,			//Distance along x from 0
		Y_AXIS,			//Distance along y from 0
		Z_AXIS,			//Distance along z from 0
		GLOBAL,			//Direct distance from 0x0z
		LAYER,			//Distance from bottom of layer center
		RADIAL,			//Radius around the circle
		DISTANCE,		//Acumulative distance from first point of layer ring
		FIXED,			//Fixed distance between points
		FIXED_TILED,	//Fixed distance that goes from 0 to 1 to 0 again
		WRAP,			
		ZERO
		}

	public UV_Generation U_Axis = UV_Generation.X_AXIS;
	public UV_Generation V_Axis = UV_Generation.RADIAL;

	public float Scale_U = 1.0f;
	public float Scale_V = 1.0f;
	#endregion




	public override int[] BuildPartTris(int Seed,ref Mesh mesh)
	{
		System.Random Rands = new System.Random(Seed);
		int l_Sides = getValueInRange(Sides, ref Rands);
		int l_Layers = getValueInRange(Layers, ref Rands);
		int triangleCount = !BuildEndCaps ? (l_Sides * l_Layers) : (l_Sides * l_Layers) + (l_Sides);
		int[] triangles = new int[triangleCount * 6];
		for (int LayerIndex = 0; LayerIndex <l_Layers; LayerIndex++)
		{


			for (int SideIndex = 0; SideIndex < l_Sides; SideIndex++)
			{
				int x = l_Sides * LayerIndex;
				int t_Index = (x + SideIndex) * 6;
				int p_index = x + SideIndex;
				triangles[t_Index] = p_index;
				triangles[t_Index + 2] = x + ((SideIndex + 1) % l_Sides);
				triangles[t_Index + 1] = p_index + l_Sides;
				triangles[t_Index + 3] = x + ((SideIndex + 1) % l_Sides);
				triangles[t_Index + 5] = (x + ((SideIndex + 1) % l_Sides)) + l_Sides;
				triangles[t_Index + 4] = p_index + l_Sides;
			}
		}

		if (BuildEndCaps)
		{
			for (int s = 0; s < l_Sides; s++)
			{
				int index = (((l_Layers) * l_Sides) + s) * 6;
				triangles[index] = ((l_Layers + 1) * l_Sides);
				triangles[index + 1] = s;
				triangles[index + 2] = (s + 1) % l_Sides;
				triangles[index + 3] = ((l_Layers + 1) * l_Sides) + 1;
				triangles[index + 5] = (l_Layers * (l_Sides)) + s;
				triangles[index + 4] = (l_Layers * (l_Sides)) + ((s + 1) % l_Sides);

			}
		}
		return triangles;
	}


	//We're going to have to re-write this to build Normals and UVs.
	public override Vector3[] BuildPartVerts(int Seed,ref Mesh mesh)
	{
	
	
		System.Random Rands = new System.Random(Seed);
		int l_Sides = getValueInRange(Sides, ref Rands);
		int l_Layers = getValueInRange(Layers, ref Rands);
		int Vertcount = BuildEndCaps ? 2 : 0;
		Vertcount += (l_Layers + 1) * l_Sides;
		Vector3[] Points = new Vector3[Vertcount];
		Vector2[] UVs = new Vector2[Vertcount];
		int pointIndex = 0;

		//We wait to build endcaps at then end to ensure Random isn't changed as we progress up the tower

		//Layer Controls are defined before sides and set after each later, so layer 0 stays centered at 0,0,0
		float LayerXOffset, LayerYOffset, LayerZOffset;
		LayerXOffset = 0.0f;
		LayerYOffset = 0.0f;
		LayerZOffset = 0.0f;
		for (int LayerIndex = 0; LayerIndex <= l_Layers; LayerIndex++)
		{
			for (int SideIndex = 0; SideIndex < l_Sides; SideIndex++)
			{
				float CornerXPos, CornerYPos, CornerZPos, CornerRadis,CornerAngle;
				CornerRadis = (Radius + (float)(Rands.NextDouble() * (RadiusVariance.y - RadiusVariance.x) + RadiusVariance.x)); // R will be Radius with Variance
				CornerYPos = LayerIndex * LayerHeight + LayerYOffset;
				if (LayerIndex != 0)
				{
					CornerYPos += (float)(Rands.NextDouble() * (CornerHeightDrift.y - CornerHeightDrift.x) + CornerHeightDrift.x);
				}
				CornerAngle = ((float)SideIndex) / l_Sides;
				CornerAngle *= 360.0f;//As of now, our point is in degrees.
				CornerAngle += (float)(Rands.NextDouble() * (RadiDrift.y - RadiDrift.x)) - RadiDrift.x;
				CornerAngle *= Mathf.Deg2Rad;
				CornerZPos = Mathf.Sin(CornerAngle) * CornerRadis + (float)(Rands.NextDouble() * (CornerDrift.y - CornerDrift.x) + CornerDrift.x) + LayerZOffset;
				CornerXPos = Mathf.Cos(CornerAngle) * CornerRadis + (float)(Rands.NextDouble() * (CornerDrift.y - CornerDrift.x) + CornerDrift.x) + LayerXOffset;
				Points[pointIndex++] = new Vector3(CornerXPos, CornerYPos, CornerZPos);
				UVs[pointIndex - 1] = GetUV(Points[pointIndex - 1], new Vector2(CornerRadis, CornerAngle), LayerIndex * LayerHeight + LayerYOffset,l_Layers, LayerIndex,l_Sides);
				if (UseEvenLayers && LayerIndex != 0)
				{
					Points[pointIndex - 1] = new Vector3(Points[pointIndex -l_Sides - 1].x, CornerYPos, Points[pointIndex - l_Sides - 1].z);
				}

			}
			if (LayerIndex == l_Layers) { break; }
			if (UseCompoundLayerDrift)
			{
				LayerXOffset += (float)(Rands.NextDouble() * (LayerCenterDrift.y - LayerCenterDrift.x) + LayerCenterDrift.x);
				LayerZOffset += (float)(Rands.NextDouble() * (LayerCenterDrift.y - LayerCenterDrift.x) + LayerCenterDrift.x);
			}
			else
			{
				LayerXOffset = (float)(Rands.NextDouble() * (LayerCenterDrift.y - LayerCenterDrift.x) + LayerCenterDrift.x);
				LayerZOffset = (float)(Rands.NextDouble() * (LayerCenterDrift.y - LayerCenterDrift.x) + LayerCenterDrift.x);
			}
			LayerYOffset = (float)(Rands.NextDouble() * (LayerHeightVariance.y - LayerHeightVariance.x) + LayerHeightVariance.x);
		}
		if (BuildEndCaps)
		{
			Points[pointIndex++] = new Vector3(0, getValueInRange(BottomEndCapHeight,ref Rands), 0);
			Points[pointIndex++] = new Vector3(LayerXOffset, (l_Layers) * LayerHeight + LayerYOffset + getValueInRange(TopEndCapHeight,ref Rands), LayerZOffset);
		}

		if (mesh != null) { mesh.vertices = Points; mesh.uv = UVs; }
		return Points;
	}

#if UNITY_EDITOR
	private int page = 0;
	public override void EditorGUI()
	{
		GUILayout.Label("Tower Structure Rules",EditorStyles.boldLabel);
		EditorGUILayout.Space(20,true);
		if(page%2==0)
			MenuPageOne();
		if (page % 2 == 1)
			MenuPageTwo();
		if(GUILayout.Button("Next Page"))
		{
			++page;
		}

	}

	private void MenuPageOne()
	{
		BuildEndCaps = EditorGUILayout.Toggle("Build End Caps", BuildEndCaps);
		if (BuildEndCaps)
		{
			EditorGUI_SetMinMax(ref TopEndCapHeight, new GUIContent("Top Endcap Height", "Range of height for top endcap"), LayerHeight * -0.5f, LayerHeight * Radius);
			EditorGUI_SetMinMax(ref BottomEndCapHeight, new GUIContent("Bottom Endcap Height", "Range of height for bottom endcap"), LayerHeight * Radius * -1f, LayerHeight * 0.5f);
		}
		EditorGUI_SetMinMax(ref Sides, new GUIContent("Side Count: ", "Number of sides to the tower"), 3, 20);
		EditorGUI_SetMinMax(ref Layers, new GUIContent("Layer Count: ", "Number of floors to the tower"), 1, 10);
		Radius = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Radius: ", "Distance from center for each corner"), Radius), 0.5f, 50);
		LayerHeight = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Layer Height: ", "Distance between centers for each floor"), LayerHeight), 0.5f, 50);

		EditorGUI_SetMinMax(ref CornerDrift, new GUIContent("Coner Drift", "Dictates how far from true even spacing the corners should drift"), Radius * -0.5f, Radius * 0.5f);
		EditorGUI_SetMinMax(ref CornerHeightDrift, new GUIContent("Coner Height Drift", "Dictates how far from true level the corners of each level should drift"), LayerHeight * -0.5f, LayerHeight * 0.5f);
		EditorGUI_SetMinMax(ref RadiDrift, new GUIContent("Coner Radial Drift", "Dictates how far from even the corners should drift radially"), -10.0f, 10.0f);
		EditorGUI_SetMinMax(ref RadiusVariance, new GUIContent("Radius Variance", "Dictates how far from center the corners should Varry drift"), -0.9f * Radius, Radius * 10f);

		EditorGUILayout.Space(10);
		GUILayout.Label(new GUIContent("Level Variance Controls"), EditorStyles.boldLabel);
		UseEvenLayers = EditorGUILayout.Toggle(new GUIContent("Use Even Layers: ", "If false, we build new corners each level"), UseEvenLayers);
		UseCompoundLayerDrift = EditorGUILayout.Toggle(new GUIContent("Use Compound Layer Drift: ", "Allows towers to compound how much they drift each level for large overhangs"), UseCompoundLayerDrift);

		GUILayout.Label(new GUIContent("Layer Center Drift", "Dictates how far each layer's center should drift"));
		GUILayout.BeginHorizontal();
		LayerCenterDrift.x = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Minimum:"), LayerCenterDrift.x), -0.9f * Radius, LayerCenterDrift.y);
		LayerCenterDrift.y = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Maximum:"), LayerCenterDrift.y), LayerCenterDrift.x, Radius * 0.9f);
		GUILayout.EndHorizontal();
		GUILayout.Label(new GUIContent("Layer Height Variance", "Dictates how far each layer's height should vary"));
		GUILayout.BeginHorizontal();
		LayerHeightVariance.x = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Minimum:"), LayerHeightVariance.x), -0.9f * LayerHeight, LayerHeightVariance.y);
		LayerHeightVariance.y = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Maximum:"), LayerHeightVariance.y), LayerHeightVariance.x, LayerHeight * 0.9f);
		GUILayout.EndHorizontal();
	}

	private void MenuPageTwo()
	{
		U_Axis = (UV_Generation)EditorGUILayout.EnumPopup(new GUIContent("U Axis"), U_Axis) ;
		V_Axis = (UV_Generation)EditorGUILayout.EnumPopup(new GUIContent("V Axis"), V_Axis);
	}
#endif
	private Vector2 GetUV(Vector3 pointPos,Vector2 RadiusAnglePos,float LayerHeight,int LayerCount,int CurrentLayer,int SideCount)
	{

		float u, v;
		switch (U_Axis)
		{
			case UV_Generation.X_AXIS:
				u = pointPos.x;
				break;
			case UV_Generation.Y_AXIS:
				u = pointPos.y;
				break;
			case UV_Generation.Z_AXIS:
				u = pointPos.z;
				break;
			case UV_Generation.GLOBAL:
				u = Vector3.Distance(Vector3.zero, pointPos);
				break;
			case UV_Generation.LAYER:
				u =((float)CurrentLayer)/LayerCount;
				break;
			case UV_Generation.RADIAL:
				u = RadiusAnglePos.y/SideCount;
				break;
			case UV_Generation.WRAP:
				u = RadiusAnglePos.y;
				break;
			default:
				u = 0;
				break;
		}

		switch (V_Axis)	
		{
			case UV_Generation.X_AXIS:
				v = pointPos.x;
			break;
			case UV_Generation.Y_AXIS:
				v = pointPos.y;
			break;
			case UV_Generation.Z_AXIS:
				v = pointPos.z;
			break;
			case UV_Generation.GLOBAL:
				v = Vector3.Distance(Vector3.zero, pointPos);
			break;
			case UV_Generation.LAYER:
				v = ((float)CurrentLayer) / LayerCount; 
			break;
			case UV_Generation.RADIAL:
				v = RadiusAnglePos.y / SideCount;
				break;
			case UV_Generation.WRAP:
				v = RadiusAnglePos.y;
				break;
			default:
				v = 0;
			break;
		}
		
		return new Vector2(u*Scale_U,v*Scale_V);

	}
}
