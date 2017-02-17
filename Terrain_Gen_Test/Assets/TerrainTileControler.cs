using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TerrainTileControler : MonoBehaviour {
    [SerializeField]
    public bool useSeed = true;
    [SerializeField]
    public string Seed = "Terrain Test 1";

    public int TileSize = 100;
    public int OctiveSize = 50;
    public float OctiveWeight = 0.2f;
    public float localWeight = 1f;
    private int iSeed;
    private int wSeed;
    [SerializeField]
    private GameObject[] TerrainTiles = new GameObject[9];
    private System.Random randy;

    // Use this for initialization
    void Start () {
        buildWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buildWorld()
    {
        iSeed = Seed.GetHashCode();
        if (!useSeed)
        {
            randy = new System.Random();
            iSeed = randy.Next();
        }
        randy = new System.Random(iSeed);
        wSeed = randy.Next();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                int id = 3 * (1 - y) + x + 1;
                if (TerrainTiles[id].GetComponent<TerrainGen>() == null)
                {
                    TerrainTiles[id].AddComponent<TerrainGen>();
                }
                TerrainTiles[id].GetComponent<TerrainGen>().Seed = wSeed;
                TerrainTiles[id].GetComponent<TerrainGen>().EdgeSize = TileSize + 1;
                TerrainTiles[id].GetComponent<TerrainGen>().OocSize = OctiveSize;
                TerrainTiles[id].GetComponent<TerrainGen>().octWeight = OctiveWeight;
                TerrainTiles[id].GetComponent<TerrainGen>().localWeight = localWeight;
                //TerrainTiles[id].transform.SetParent(gameObject.transform);
                TerrainTiles[id].transform.position = new Vector3(x * TileSize, 0, y * TileSize);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();

            }
        }
    }
}

[CustomEditor(typeof(TerrainTileControler))]
public class TerrainTileControllerEditor : Editor
{

   

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainTileControler localControl = (TerrainTileControler)target;
        if(GUILayout.Button("Build World"))
        {
            long start = DateTime.Now.Ticks;
            localControl.buildWorld();
            long end = DateTime.Now.Ticks;
            Debug.Log(((int)(end - start))/10000000);
        }
    }

}
