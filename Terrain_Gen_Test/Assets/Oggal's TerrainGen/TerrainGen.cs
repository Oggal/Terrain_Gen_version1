using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
Oggal's Terrain Gen
*/

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGen : MonoBehaviour {

    [SerializeField]
    public int EdgeSize = 20;
    [SerializeField]
    public int Seed = 10;
    public float localWeight = 1f;
    public int OocSize = 20;
    public float octWeight = 0.25f;
    public uint OctSize = 1;

    Transform T_Trans;
    MeshCollider    M_Coll;
    MeshFilter      M_Filt;
    MeshRenderer    M_Rend;
    Mesh            M_mesh;
    TerrainData[] TD_Octaves;
    int[] Seeds;
    TerrainData     TD_Data;
    TerrainData     TD_Octive2;
    System.Random Randy;
	// Use this for initialization
	void Start () {
        //buildMesh();
 
	}
	
	// Update is called once per frame
	void Update () {
       

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
        M_Rend = GetComponent<MeshRenderer>();

        Seeds = null;

        T_Trans = transform;
        float wX = T_Trans.localPosition.x;
        float wY = T_Trans.localPosition.z;
        /*
        TD_Data = new TerrainData(Seed);
        TD_Octive2 = new TerrainData(Seed + 59);
        TD_Octive2.SideSize = OocSize;
        */
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
                }else{
                    xu = xu % 3;
                }
                u = xu / 2.0f;
                float v;
                if (yu > 2)
                {
                    yu = (yu - 2) % 3;
                }else{
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
        if(Seeds == null || TD_Octaves == null)
        {
            Seeds = new int[OctSize];
            TD_Octaves = new TerrainData[OctSize];
            for (int i = 0; i < OctSize; i++){
                Seeds[i] = Randy.Next();
                TD_Octaves[i] = new TerrainData(Seeds[i], (i + 1) * 5);
            }
        }
        float output = 0;
        for(int i = 0; i < OctSize; i++)
        {
            output += TD_Octaves[i].getHeight(x, y) * ((i+1)/(octWeight*OctSize));
        }
        return output;
    }
}

public class TerrainData
{

    int Seed;
    public int SideSize = 5;
    Dictionary<int, double> VectorLib = new Dictionary<int, double>();

    public TerrainData(object i,int Size = 5)
    {
       Seed = i.GetHashCode();
       SideSize = Size;
    }


    private double getVector(int x, int y)
    {
        int index = getID(x, y) * Seed;
        if (VectorLib.ContainsKey(index))
        {
            double O; 
                VectorLib.TryGetValue(index, out O);
            return O;
        } else
        {

            System.Random R1 = new System.Random(index);
            double O = R1.NextDouble();
            VectorLib.Add(index, O);
            return O;
        }
    }

        public float getHeight(float x, float y)
    {
        int x1 = (((int)x) / SideSize);
        int y1 = (((int)y) / SideSize);
        x1 = (x < 0 ? (x1 - 1) * SideSize : x1 * SideSize);
        y1 = (y < 0 ? (y1 - 1) * SideSize : y1 * SideSize);
        int x2 = x1 + SideSize;
        int y2 = y1 + SideSize;

        double v1, v2, v3, v4;
        v1 = getVector(x1, y1);
        v2 = getVector(x1, y2);
        v3 = getVector(x2, y1);
        v4 = getVector(x2, y2);



        float d1, d2, d3, d4;
            d1 = DotProduct(Math.Cos(v1 * 2 * Math.PI), Math.Sin(v1 * 2 * Math.PI), x1 - x, y1 - y);
            d2 = DotProduct(Math.Cos(v2 * 2 * Math.PI), Math.Sin(v2 * 2 * Math.PI), x1 - x, y2 - y);
            d3 = DotProduct(Math.Cos(v3 * 2 * Math.PI), Math.Sin(v3 * 2 * Math.PI), x2- x, y1 - y);
            d4 = DotProduct(Math.Cos(v4 * 2 * Math.PI), Math.Sin(v4 * 2 * Math.PI), x2 - x, y2 - y);


            return wAdv(wAdv(d1, d3, x - x1), wAdv(d2, d4, x - x1), y-y1);
    }

    private int getID(int x, int y)
    {
        return (x >> 5)+y|x;

    }

    private float DotProduct(double x,double y,float xi,float yi)
    {
        return (float)((x * xi) + (y * yi));
    }

    private float wAdv(float a1,float a2,float w)
    {
        w = w / SideSize;

        return (1 - w) * a1+ w * a2;
    }

}