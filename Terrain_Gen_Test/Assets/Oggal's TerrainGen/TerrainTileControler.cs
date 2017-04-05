﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int size = 1;
    public int TrueSize = 3;//TrueSize = size*2+1;

    public int TileSize = 100;
    public uint OctaveCount = 5;
    public float OctiveWeight = 1.5f;
    private float localWeight = 1f;

    private int iSeed;
    private int wSeed;

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
            //East
            for(int i = 0; i < TrueSize; i++)
            {
                int id = i * TrueSize;
                TerrainTiles[id].transform.position = new Vector3((localX+size) * TileSize * transform.localScale.x,0,(localZ + (size-i))*TileSize*transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
            }
            /*
            TerrainTiles[0].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
            TerrainTiles[0].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[3].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ) * TileSize * transform.localScale.z);
            TerrainTiles[3].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[6].transform.position = new Vector3((localX + 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
            TerrainTiles[6].GetComponent<TerrainGen>().buildMesh();
            */
            for(int y =0; y < TrueSize; y++)
            {
                GameObject tem = TerrainTiles[y * TrueSize];
                for(int x = 0; x < TrueSize - 1; x++)
                {
                    TerrainTiles[y * TrueSize + ((x))] = TerrainTiles[y * TrueSize + ((x + 1) % TrueSize)];
                }
                TerrainTiles[y * (TrueSize) + TrueSize-1] = tem;
            }
            /*
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
            */
        }
        else {
            //West
            for (int i = 0; i < TrueSize; i++)
            {
                int id = ((i+1) * TrueSize)-1;
                TerrainTiles[id].transform.position = new Vector3((localX - size) * TileSize * transform.localScale.x, 0, (localZ + (size-i)) * TileSize * transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
            }


            /*
            TerrainTiles[2].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ + 1) * TileSize * transform.localScale.z);
            TerrainTiles[2].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[5].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ) * TileSize * transform.localScale.z);
            TerrainTiles[5].GetComponent<TerrainGen>().buildMesh();

            TerrainTiles[8].transform.position = new Vector3((localX - 1) * TileSize * transform.localScale.x, 0, (localZ - 1) * TileSize * transform.localScale.z);
            TerrainTiles[8].GetComponent<TerrainGen>().buildMesh();
            */
            for (int y = 0; y < TrueSize; y++)
            {
                GameObject tem = TerrainTiles[y * TrueSize + TrueSize - 1];
                for(int x = TrueSize-1; x >0; x--)
                {
                    TerrainTiles[y * TrueSize + x] = TerrainTiles[y * TrueSize + ((x + TrueSize-1) % TrueSize)];
                }
                TerrainTiles[y * TrueSize ] = tem;
            }
            /*
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
            */

        }

    }
 
    public void MoveY(bool t)
    {
        if (t)
            {
            //North
                for (int i = 0; i < TrueSize; i++)
                {

                    TerrainTiles[(TrueSize * (TrueSize - 1)) + i].transform.position = new Vector3((localX + (i-size))*TileSize*transform.localScale.x, 0, (localZ + size)*TileSize*transform.localScale.z);
                    TerrainTiles[(TrueSize * (TrueSize - 1)) + i].GetComponent<TerrainGen>().buildMesh();
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
                    TerrainTiles[i].transform.position = new Vector3((localX + (i-size)) *TileSize* transform.localScale.x, 0, (localZ - size) *TileSize* transform.localScale.z);
                    TerrainTiles[i].GetComponent<TerrainGen>().buildMesh();
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
                TerrainTiles[id].GetComponent<TerrainGen>().localWeight = localWeight;
                //TerrainTiles[id].transform.SetParent(gameObject.transform);
                TerrainTiles[id].transform.position = new Vector3(x * TileSize * transform.localScale.x, 0, y * TileSize*transform.localScale.z);
                TerrainTiles[id].GetComponent<TerrainGen>().buildMesh();
                TerrainTiles[id].GetComponent<MeshRenderer>().material = mat;

            }
        }
    }
}

