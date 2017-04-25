using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;




/*
Oggal's Terrain Gen
*/
public class TerrainTileControler : MonoBehaviour {
    [SerializeField]
    public bool useSeed = true;
    public bool BuildOnRun = true;
    public bool LoadOnRun;

    public Material mat;
    public GameObject Player;

    [SerializeField]
    public string Seed = "Terrain Test 1";

    public int size = 1;
    public int TrueSize = 3;//TrueSize = size*2+1;

    public int TileSize = 100;
    public uint OctaveCount = 5;
    public float OctiveWeight = 1.5f;
    public float layerWeight = 1f;
    public List<GameObject> Trees = new List<GameObject>();

    private int iSeed;
    [HideInInspector]
    public int wSeed;

    public GameObject[] TerrainTiles;
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

        if(!BuildOnRun && LoadOnRun)
        {
            Load();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Load();
        }



        if (Player.transform.position.x / (TileSize * transform.localScale.x) > localX+((float)size/2)){
            Debug.Log("Player is East!");
            localX++;
            MoveX(true);
        }
        if(Player.transform.position.x / (TileSize * transform.localScale.x) <  localX- ((float)size / 2))
        {
            Debug.Log("Player is West!");
            localX--;
            MoveX(false);
        }
        if (Player.transform.position.z / (TileSize * transform.localScale.z) > localZ + ((float)size / 2))
        {
            Debug.Log("Player is North!");
            localZ++;
            MoveY(true);
        }
        if (Player.transform.position.z / (TileSize * transform.localScale.z) < localZ - ((float)size / 2))
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
            //East
            for(int i = 0; i < TrueSize; i++)
            {
                int id = i * TrueSize;
                //   TerrainTiles[id].transform.position = new Vector3((localX+size) * TileSize * transform.localScale.x,0,(localZ + (size-i))*TileSize*transform.localScale.z);
                // TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainGen ti = TerrainTiles[id].GetComponent<TerrainGen>();
                Vector3 p = new Vector3((localX + size) * TileSize * transform.localScale.x, 0, (localZ + (size - i)) * TileSize * transform.localScale.z);
                ti.queMove(p);

            }

            for(int y =0; y < TrueSize; y++)
            {
                GameObject tem = TerrainTiles[y * TrueSize];
                for(int x = 0; x < TrueSize - 1; x++)
                {
                    TerrainTiles[y * TrueSize + ((x))] = TerrainTiles[y * TrueSize + ((x + 1) % TrueSize)];
                }
                TerrainTiles[y * (TrueSize) + TrueSize-1] = tem;
            }

        }
        else {
            //West
            for (int i = 0; i < TrueSize; i++)
            {
                int id = ((i+1) * TrueSize)-1;
              //  TerrainTiles[id].transform.position = new Vector3((localX - size) * TileSize * transform.localScale.x, 0, (localZ + (size-i)) * TileSize * transform.localScale.z);
              //  TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainGen ti = TerrainTiles[id].GetComponent<TerrainGen>();
                Vector3 p = new Vector3((localX - size) * TileSize * transform.localScale.x, 0, (localZ + (size - i)) * TileSize * transform.localScale.z);
                ti.queMove(p);

            }



            for (int y = 0; y < TrueSize; y++)
            {
                GameObject tem = TerrainTiles[y * TrueSize + TrueSize - 1];
                for(int x = TrueSize-1; x >0; x--)
                {
                    TerrainTiles[y * TrueSize + x] = TerrainTiles[y * TrueSize + ((x + TrueSize-1) % TrueSize)];
                }
                TerrainTiles[y * TrueSize ] = tem;
            }


        }

    }
 
    public void MoveY(bool t)
    {
        if (t)
            {
            //North
                for (int i = 0; i < TrueSize; i++)
                {
                TerrainGen ti = TerrainTiles[(TrueSize * (TrueSize - 1)) + i].GetComponent<TerrainGen>();
                Vector3 p = new Vector3((localX + (i - size)) * TileSize * transform.localScale.x, 0, (localZ + size) * TileSize * transform.localScale.z);
                 ti.queMove(p);
                }
 
                 for(int x = 0; x < TrueSize; x++)
                {
                    GameObject tem = TerrainTiles[TrueSize*(TrueSize-1)+x];
                    for(int y = TrueSize-1;y>0; y--)
                    {
                        TerrainTiles[y * TrueSize + x] = TerrainTiles[((y + (TrueSize - 1)) % TrueSize) * TrueSize + x];
                    }
                    TerrainTiles[x] = tem;
                }

            }
            else
            {
            //South
                for (int i = 0; i < TrueSize; i++)
                {
                TerrainGen ti = TerrainTiles[i].GetComponent<TerrainGen>();
                Vector3 p = new Vector3((localX + (i - size)) * TileSize * transform.localScale.x, 0, (localZ - size) * TileSize * transform.localScale.z);
                ti.queMove(p);
                //TerrainTiles[i].transform.position = new Vector3((localX + (i-size)) *TileSize* transform.localScale.x, 0, (localZ - size) *TileSize* transform.localScale.z);
                // TerrainTiles[i].GetComponent<TerrainGen>().buildMesh();
            }

                    for(int x = 0; x < TrueSize; x++)
                {
                    GameObject tem = TerrainTiles[x];
                    for(int y = 0; y < TrueSize - 1; y++)
                    {
                        TerrainTiles[y * TrueSize + x] = TerrainTiles[((y + 1) % TrueSize) * TrueSize + x];
                    }
                    TerrainTiles[(TrueSize*(TrueSize-1)) + x] = tem;
                }

            }
        }

    public void buildWorld()
    {
        TrueSize = size * 2 + 1;
        TerrainTiles = new GameObject[TrueSize * TrueSize];
        iSeed = Seed.GetHashCode();
        if (!useSeed)
        {
            randy = new System.Random();
            iSeed = randy.Next();
        }
        randy = new System.Random(iSeed);
        wSeed = randy.Next();
        for (int y = -1*size; y <= size; y++)
        {
            for (int x = -1*size; x <= size; x++)
            {
                int id = TrueSize * (size - y) + x + size;
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
                TerrainTiles[id].GetComponent<TerrainGen>().layerWeight = layerWeight;
                //TerrainTiles[id].transform.SetParent(gameObject.transform);
                TerrainTiles[id].transform.position = new Vector3(x * TileSize * transform.localScale.x, 0, y * TileSize*transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainTiles[id].GetComponent<MeshRenderer>().material = mat;

            }
        }
    }

    private void BuildFromSave()
    {
        if(transform.childCount != 0)
        {
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        TrueSize = size * 2 + 1;
        TerrainTiles = new GameObject[TrueSize * TrueSize];

        for (int z = -1 * size; z <= size; z++)
        {
            for (int x = -1 * size; x <= size; x++)
            {
                int id = TrueSize * (size - z) + x + size;
                if (TerrainTiles[id] == null)
                {
                    TerrainTiles[id] = new GameObject("Tile" + id);
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
                TerrainTiles[id].GetComponent<TerrainGen>().layerWeight = layerWeight;
                //TerrainTiles[id].transform.SetParent(gameObject.transform);
                TerrainTiles[id].transform.position = new Vector3((x+localX) * TileSize * transform.localScale.x, 0, (z + localZ) * TileSize * transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainTiles[id].GetComponent<MeshRenderer>().material = mat;

            }
        }

    }
    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
        WorldSave dat = bf.Deserialize(file) as WorldSave;
        if(dat != null)
        {
            loadWorld(dat);
        }
        else
        {
            Debug.LogError("WorldSave Corrupt!!!");
        }
        file.Close();
    }

    public void loadWorld(WorldSave ws)
    {
        this.wSeed = ws.getSeed();
        Vector3 wPos = ws.getWorldPos();
        localX = (int)wPos.x;
        localZ = (int)wPos.z;
        //BuildWorld;
        BuildFromSave();

        Player.transform.position = ws.getPlayerPos();
        Player.transform.rotation = ws.getPlayerRot();
       
    }
    public void Save()
    {
        saveWorld();
    }

    public WorldSave saveWorld()
    {
        WorldSave Data = new WorldSave(this);
        Data.SetWorldPos(new int[2]{localX,localZ});
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, Data);
        file.Close();
        return Data;
    }
}


[Serializable]
public class WorldSave
{
    [SerializeField]
    private float[] plyPos;
    [SerializeField]
    private int[] worldPos;
    [SerializeField]
    private int     WorldSeed;


    public WorldSave(TerrainTileControler TTC)
    {
        plyPos = new float[6];
        plyPos[0] = TTC.Player.transform.position.x;
        plyPos[1] = TTC.Player.transform.position.y;
        plyPos[2] = TTC.Player.transform.position.z;
        plyPos[3] = TTC.Player.transform.rotation.eulerAngles.x;
        plyPos[4] = TTC.Player.transform.rotation.eulerAngles.y;
        plyPos[5] = TTC.Player.transform.rotation.eulerAngles.z;

        worldPos = new int[2];
        WorldSeed = TTC.wSeed;
    }

    public void SetWorldPos(int[] i)
    {
        if(i.Length != 2) { Debug.LogError("World Pos Array Wrong Size!"); return; }
        worldPos = i;
    }
    public int getSeed()
    {
        return WorldSeed;
    }
    public Vector3 getWorldPos()
    {
        return new Vector3(worldPos[0], 0, worldPos[1]);
    }
    public Vector3 getPlayerPos()
    {
        return new Vector3(plyPos[0], plyPos[1], plyPos[2]);
    }
    public Quaternion getPlayerRot()
    {
        return Quaternion.Euler(plyPos[3], plyPos[4], plyPos[5]);
    }
}

