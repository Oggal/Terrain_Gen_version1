using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(GlobalDataController))]
public class TerrainControlerGUI : Editor {


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GlobalDataController localCtrl = (GlobalDataController)target;

		if (GUILayout.Button("Save"))
		{
			localCtrl.Save();
		}
		if (GUILayout.Button("Load"))
		{
			localCtrl.Load();
		}
		GUILayout.Space(20);

		if (GUILayout.Button("TEST"))
		{
			localCtrl.Test();
		}
	}

}
