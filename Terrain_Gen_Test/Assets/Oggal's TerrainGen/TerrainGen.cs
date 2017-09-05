using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;



/*
Oggal's Terrain Gen
*/

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGen : MonoBehaviour
{

    [SerializeField]
    public int EdgeSize = 20;
    [SerializeField]
    public int Seed = 10;
    public float layerWeight = 1f;
    public int OocSize = 20;
    public float octWeight = 0.25f;
    public uint OctSize = 1;


    Transform T_Trans;
    MeshCollider M_Coll;
    MeshFilter M_Filt;
    Mesh M_mesh;
    MeshData newMesh = null;
    bool wait = false;
    Vector3 newPos;
    TerrainNoise[] TD_Octaves;
    int[] Seeds;
    TerrainNoise TD_ScaleMap;
    System.Random Randy;
    // Use this for initialization


    void Start()
    {
        //buildMesh();

        Randy = new System.Random(Seed);
        M_Coll = GetComponent<MeshCollider>();
        M_Filt = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {

        if (wait && newMesh != null)
        {

            M_Filt.mesh = newMesh.ToMesh();
            M_Coll.sharedMesh = newMesh.ToMesh();
            wait = false;
        }

    }
    float wX;
    float wY;
    public void queMove(Vector3 p)
    {
        wait = true;
        M_Filt.mesh = null;
        M_Coll.sharedMesh = null;
        newMesh = null;

        T_Trans = transform;
        transform.position = p;
        wX = T_Trans.localPosition.x;
        wY = T_Trans.localPosition.z;
        Debug.Log("MoveStart");
        new Thread(Move).Start();
    }

    private void Move()
    {

        MeshData m = new MeshData();
        //m.name = "World Mesh        

        Vector3[] Verts = new Vector3[EdgeSize * EdgeSize];
        Vector2[] UVs = new Vector2[EdgeSize * EdgeSize];
        int[] Trys = new int[6 * ((EdgeSize - 1) * (EdgeSize - 1))];


        for (int y = 0; y < EdgeSize; y++)
        {
            for (int x = 0; x < EdgeSize; x++)
            {
                int id = y * EdgeSize + x;
                int Xi = x - (EdgeSize / 2);
                int Yi = y - (EdgeSize / 2);
                Verts[id] = new Vector3(
                    Xi,
                    GetHeight(Xi + wX, Yi + wY),
                    Yi);

                int xu = x % 4;
                int yu = y % 4;
                float u;
                if (xu > 2)
                {
                    xu = (xu - 2) % 3;
                }
                else
                {
                    xu = xu % 3;
                }
                u = xu / 2.0f;
                float v;
                if (yu > 2)
                {
                    yu = (yu - 2) % 3;
                }
                else
                {
                    yu = yu % 3;
                }
                v = yu / 2.0f;

                UVs[id] = new Vector2(Mathf.Abs(u), Mathf.Abs(v));

                // Build Triangles
                if (y < EdgeSize - 1 && x < EdgeSize - 1)
                {
                    int T_id = (y * (EdgeSize - 1) + x) * 6;
                    Trys[T_id] = id;
                    Trys[T_id + 1] = id + EdgeSize;
                    Trys[T_id + 2] = id + 1;

                    Trys[T_id + 3] = id + 1;
                    Trys[T_id + 4] = id + EdgeSize;
                    Trys[T_id + 5] = id + 1 + EdgeSize;
                }
            }
        }

        m.verts = Verts;
        m.Triangles = Trys;
        m.UVs = UVs;
        newMesh = m;
    }

    public void buildMesh()
    {
        #region DerfineVars
        if (M_mesh == null)
        {
            M_mesh = new Mesh();
            M_mesh.name = "World Mesh";
        }
        Randy = new System.Random(Seed);
        M_Coll = GetComponent<MeshCollider>();
        M_Filt = GetComponent<MeshFilter>();


        Seeds = null;

        T_Trans = transform;
        wX = T_Trans.localPosition.x;
        wY = T_Trans.localPosition.z;

        Vector3[] Verts = new Vector3[EdgeSize * EdgeSize];
        Vector2[] UVs = new Vector2[EdgeSize * EdgeSize];
        int[] Trys = new int[6 * ((EdgeSize - 1) * (EdgeSize - 1))];

        #endregion



        //Build Verts

        for (int y = 0; y < EdgeSize; y++)
        {
            for (int x = 0; x < EdgeSize; x++)
            {
                int id = y * EdgeSize + x;
                int Xi = x - (EdgeSize / 2);
                int Yi = y - (EdgeSize / 2);
                Verts[id] = new Vector3(
                    Xi,
                    GetHeight(Xi + wX, Yi + wY),
                    Yi);

                int xu = x % 4;
                int yu = y % 4;
                float u;
                if (xu > 2)
                {
                    xu = (xu - 2) % 3;
                }
                else
                {
                    xu = xu % 3;
                }
                u = xu / 2.0f;
                float v;
                if (yu > 2)
                {
                    yu = (yu - 2) % 3;
                }
                else
                {
                    yu = yu % 3;
                }
                v = yu / 2.0f;

                UVs[id] = new Vector2(Mathf.Abs(u), Mathf.Abs(v));

                // Build Triangles
                if (y < EdgeSize - 1 && x < EdgeSize - 1)
                {
                    int T_id = (y * (EdgeSize - 1) + x) * 6;
                    Trys[T_id] = id;
                    Trys[T_id + 1] = id + EdgeSize;
                    Trys[T_id + 2] = id + 1;

                    Trys[T_id + 3] = id + 1;
                    Trys[T_id + 4] = id + EdgeSize;
                    Trys[T_id + 5] = id + 1 + EdgeSize;
                }
            }
        }

        //Assign Values
        M_mesh.vertices = Verts;
        M_mesh.triangles = Trys;
        M_mesh.uv = UVs;
        M_mesh.RecalculateNormals();
        M_Filt.mesh = M_mesh;
        M_Coll.sharedMesh = M_mesh;

    }

    private float GetHeight(float x, float y)
    {
        if (OctSize == 0)
        {
            OctSize++;
        }
        if (Seeds == null || TD_Octaves == null)
        {
            Seeds = new int[OctSize];
            TD_Octaves = new TerrainNoise[OctSize];
            for (int i = 0; i < OctSize; i++)
            {
                Seeds[i] = Randy.Next();
                TD_Octaves[i] = new TerrainNoise(Seeds[i], (int)((i + 1) * layerWeight));
            }
        }
        float output = 0;
        for (int i = 0; i < OctSize; i++)
        {
            output += TD_Octaves[i].getHeight(x, y) * ((i + 1) / (octWeight * OctSize));
        }
        output *= GetScale(x, y);
        return output;
    }

    private float GetScale(float x, float y)
    {
        if (TD_ScaleMap == null)
        {
            TD_ScaleMap = new TerrainNoise(Seed, 1);
        }
        float o = (TD_ScaleMap.getHeight(x / 500f, y / 500f));
        o = Mathf.Max(0, Mathf.Min(o + 0.5f, 1.5f)) / 1.3f;
        return o;

    }

}


class MeshData
{
    public Vector3[] verts;
    public Vector2[] UVs;
    public int[] Triangles;

    public Mesh ToMesh()
    {
        Mesh n = new Mesh();
        n.vertices = verts;
        n.uv = UVs;
        n.triangles = Triangles;
        n.RecalculateNormals();
        n.name = "WorldMesh";
        return n;
    }

}