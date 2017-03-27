using UnityEditor;
using UnityEngine;
using System;


[CustomEditor(typeof(TerrainTileControler))]
 class TerrainTileControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainTileControler localControl = (TerrainTileControler)target;
        if (GUILayout.Button("Build World"))
        {
            long start = DateTime.Now.Ticks;
            localControl.buildWorld();
            long end = DateTime.Now.Ticks;
            Debug.Log(((int)(end - start)) / 10000000);
        }
        if (GUILayout.Button("Clear Childern"))
        {
            int childs = localControl.transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(localControl.transform.GetChild(i).gameObject);
            }
        }
    }

}

