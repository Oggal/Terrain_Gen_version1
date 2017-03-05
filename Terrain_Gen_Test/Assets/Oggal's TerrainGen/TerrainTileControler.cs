using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



/*
Oggal's Terrain Gen
*/
public class TerrainTileControler : MonoBehaviour {
    [SerializeField]
    public bool useSeed = true;
    public bool BuildOnRun = true;

    public Material mat;
    public GameObject Player;

    [SerializeField]
    public string Seed = "Terrain Test 1";
  

    public int TileSize = 100;
    public uint OctaveCount = 5;
    public float OctiveWeight = 1.5f;
    private float localWeight = 1f;

    private int iSeed;
    private int wSeed;

    private GameObject[] TerrainTiles = new GameObject[9];
    private System.Random randy;

    private int localX = 0;
    private int localZ = 0;

    // Use this for initialization
    void Start () {
        if(Player == null)
        {
            Player = gameObject;
        }
        if(BuildOnRun)
            buildWorld();
	}
	
	// Update is called once per frame
	void Update () {

        if (Player.transform.position.x / (TileSize * transform.localScale.x) > localX+0.5f){
            Debug.Log("Player is East!");
            localX++;
            MoveX(true);
        }
        if(Player.transform.position.x / (TileSize * transform.localScale.x) <  localX-0.5f)
        {
            Debug.Log("Player is West!");
            localX--;
            MoveX(false);
        }
        if (Player.transform.position.z / (TileSize * transform.localScale.z) > localZ + 0.5f)
        {
            Debug.Log("Player is North!");
            localZ++;
            MoveY(true);
        }
        if (Player.transform.position.z / (TileSize * transform.localScale.z) < localZ - 0.5f)
        {
            Debug.Log("Player is South!");
            localZ--;
            MoveY(false);
        }
    }
    public void MoveX(bool t)
    {
        if (t)
        {
            TerrainTiles[0].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
            TerrainTiles[0].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[3].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ) * TileSize * transform.localScale.z);
            TerrainTiles[3].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[6].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
            TerrainTiles[6].GetComponent<TerrainGen>().buildMesh();

            GameObject temp = TerrainTiles[1];
            TerrainTiles[1] = TerrainTiles[2];
            TerrainTiles[2] = TerrainTiles[0];
            TerrainTiles[0] = temp;
            temp = TerrainTiles[4];
            TerrainTiles[4] = TerrainTiles[5];
            TerrainTiles[5] = TerrainTiles[3];
            TerrainTiles[3] = temp;
            temp = TerrainTiles[7];
            TerrainTiles[7] = TerrainTiles[8];
            TerrainTiles[8] = TerrainTiles[6];
            TerrainTiles[6] = temp;

        }
        else {

            TerrainTiles[2].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
            TerrainTiles[2].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[5].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ) * TileSize * transform.localScale.z);
            TerrainTiles[5].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[8].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
            TerrainTiles[8].GetComponent<TerrainGen>().buildMesh();

            GameObject temp = TerrainTiles[0];
            TerrainTiles[0] = TerrainTiles[2];
            TerrainTiles[2] = TerrainTiles[1];
            TerrainTiles[1] = temp;
            temp = TerrainTiles[3];
            TerrainTiles[3] = TerrainTiles[5];
            TerrainTiles[5] = TerrainTiles[4];
            TerrainTiles[4] = temp;
            temp = TerrainTiles[6];
            TerrainTiles[6] = TerrainTiles[8];
            TerrainTiles[8] = TerrainTiles[7];
            TerrainTiles[7] = temp;


        }

    }
 
    public void MoveY(bool t)
    {
        if (t)
            {
                TerrainTiles[6].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
                TerrainTiles[6].GetComponent<TerrainGen>().buildMesh();

                TerrainTiles[7].transform.position = new Vector3((localX) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
                TerrainTiles[7].GetComponent<TerrainGen>().buildMesh();

                TerrainTiles[8].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
                TerrainTiles[8].GetComponent<TerrainGen>().buildMesh();

                GameObject temp = TerrainTiles[6];
                TerrainTiles[6] = TerrainTiles[3];
                TerrainTiles[3] = TerrainTiles[0];
                TerrainTiles[0] = temp;
                temp = TerrainTiles[7];
                TerrainTiles[7] = TerrainTiles[4];
                TerrainTiles[4] = TerrainTiles[1];
                TerrainTiles[1] = temp;
                temp = TerrainTiles[8];
                TerrainTiles[8] = TerrainTiles[5];
                TerrainTiles[5] = TerrainTiles[2];
                TerrainTiles[2] = temp;

            }
            else
            {

                TerrainTiles[0].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
                TerrainTiles[0].GetComponent<TerrainGen>().buildMesh();

                TerrainTiles[1].transform.position = new Vector3((localX) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
                TerrainTiles[1].GetComponent<TerrainGen>().buildMesh();

                TerrainTiles[2].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
                TerrainTiles[2].GetComponent<TerrainGen>().buildMesh();

                GameObject temp = TerrainTiles[3];
                TerrainTiles[3] = TerrainTiles[0];
                TerrainTiles[0] = TerrainTiles[6];
                TerrainTiles[6] = temp;
                temp = TerrainTiles[4];
                TerrainTiles[4] = TerrainTiles[1];
                TerrainTiles[1] = TerrainTiles[7];
                TerrainTiles[7] = temp;
                temp = TerrainTiles[5];
                TerrainTiles[4] = TerrainTiles[2];
                TerrainTiles[2] = TerrainTiles[8];
                TerrainTiles[8] = temp;
            }
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
                if (TerrainTiles[id] == null)
                {
                    TerrainTiles[id] = new GameObject("Tile"+id);
                    TerrainTiles[id].transform.parent = gameObject.transform;
                    TerrainTiles[id].transform.localScale = new Vector3(1, 1, 1);
                }
                if (TerrainTiles[id].GetComponent<TerrainGen>() == null)
                {
                    TerrainTiles[id].AddComponent<TerrainGen>();
                }
                TerrainTiles[id].GetComponent<TerrainGen>().Seed = wSeed;
                TerrainTiles[id].GetComponent<TerrainGen>().EdgeSize = TileSize + 1;
                TerrainTiles[id].GetComponent<TerrainGen>().OctSize = OctaveCount;
                TerrainTiles[id].GetComponent<TerrainGen>().octWeight = OctiveWeight;
                TerrainTiles[id].GetComponent<TerrainGen>().localWeight = localWeight;
                //TerrainTiles[id].transform.SetParent(gameObject.transform);
                TerrainTiles[id].transform.position = new Vector3(x * TileSize * transform.localScale.x, 0, y * TileSize*transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainTiles[id].GetComponent<MeshRenderer>().material = mat;

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
