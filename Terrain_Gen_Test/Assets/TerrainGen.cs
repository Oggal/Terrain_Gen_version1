using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGen : MonoBehaviour {

    [SerializeField]
    private int EdgeSize = 20;
    [SerializeField]
    private int Seed = 10;


    Transform T_Trans;
    MeshCollider    M_Coll;
    MeshFilter      M_Filt;
    MeshRenderer    M_Rend;
    Mesh            M_mesh;
    TerrainData     TD_Data;
    TerrainData TD_Octive2;
	// Use this for initialization
	void Start () {
        M_Coll = GetComponent<MeshCollider>();
        M_Filt = GetComponent<MeshFilter>();
        M_Rend = GetComponent<MeshRenderer>();
        T_Trans = transform;
        TD_Data = new TerrainData(Seed);
        TD_Octive2 = new TerrainData(Seed+59);
        TD_Octive2.SideSize = 30;
        M_mesh = new Mesh();
        M_mesh.name = "World Mesh";
        buildMesh();
        M_Filt.mesh = M_mesh;
        M_Coll.sharedMesh = M_mesh;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3[] verts = M_Filt.mesh.vertices;
        for(int v = 0; v<verts.Length;v++)
        {
            verts[v].y = TD_Data.getHeight(verts[v].x+T_Trans.position.x, verts[v].z + T_Trans.position.z);
        }
        M_Filt.mesh.vertices = verts;
        M_Coll.sharedMesh = M_Filt.mesh;

    }
     
    public void buildMesh()
    {
        if(M_mesh == null)
        {
            M_mesh = new Mesh();
            M_mesh.name = "World Mesh";
        }
        Vector3[] Verts = new Vector3[EdgeSize * EdgeSize];
        Vector2[] UVs   = new Vector2[EdgeSize * EdgeSize];
        int[] Trys = new int[6*((EdgeSize-1)*(EdgeSize-1))];
        //Build Verts
        for(int y = 0; y < EdgeSize; y++)
        {
            for(int x = 0; x < EdgeSize; x++)
            {
                int id = y * EdgeSize + x;
                int Xi = x - (EdgeSize / 2);
                int Yi = y - (EdgeSize / 2);
                Verts[id] = new Vector3(Xi, TD_Data.getHeight(Xi, Yi), Yi);
                UVs[id] = new Vector2(Mathf.Abs(x % 2),Mathf.Abs( y % 2));
            }
        }
        //Build Trangles
        for (int y = 0; y < EdgeSize - 1; y++)
        {
            for(int x = 0; x < EdgeSize - 1; x++)
            {
                int ID = y * EdgeSize + x;
                int T_id = (y*(EdgeSize-1) + x)*6;
                Trys[T_id]   = ID;
                Trys[T_id+1] = ID+EdgeSize;
                Trys[T_id+2] = ID+1;

                Trys[T_id+3] = ID+1;
                Trys[T_id+4] = ID+EdgeSize;
                Trys[T_id+5] = ID+1+EdgeSize;

            }
        }
        //Assign Values
        M_mesh.vertices = Verts;
        M_mesh.triangles = Trys;
        M_mesh.uv = UVs;
    }


}

public class TerrainData
{

    int Seed;
    int[] QuadSeeds = new int[5];
    public int SideSize = 50;

    private System.Random rand;
    private System.Random[] QuadRand = new System.Random[5];
    public TerrainData(object i)
    {
        this.Seed = i.GetHashCode();
        rand = new System.Random(Seed);
        for(int a = 0; a < 5; a++)
        {
            QuadRand[a] = new System.Random(rand.Next());
            QuadSeeds[a] = (int)(QuadRand[a].NextDouble()*9999999);
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

        System.Random R1 = new System.Random(getID(x1, y1) * Seed);
        System.Random R2 = new System.Random(getID(x1, y2) * Seed);
        System.Random R3 = new System.Random(getID(x2, y1) * Seed);
        System.Random R4 = new System.Random(getID(x2, y2) * Seed);

        double v1, v2, v3, v4;
        v1 = R1.NextDouble();
        v2 = R2.NextDouble();
        v3 = R3.NextDouble();
        v4 = R4.NextDouble();



        float d1, d2, d3, d4;
            d1 = DotProduct(Math.Cos(v1 * 2 * Math.PI), Math.Sin(v1 * 2 * Math.PI), x - x1, y - y1);
            d2 = DotProduct(Math.Cos(v2 * 2 * Math.PI), Math.Sin(v2 * 2 * Math.PI), x - x1, y - y2);
            d3 = DotProduct(Math.Cos(v3 * 2 * Math.PI), Math.Sin(v3 * 2 * Math.PI), x - x2, y - y1);
            d4 = DotProduct(Math.Cos(v4 * 2 * Math.PI), Math.Sin(v4 * 2 * Math.PI), x - x2, y - y2);


            return wAdv(wAdv(d1, d3, x - x1), wAdv(d2, d4, x - x1), y-y1);
    }

    private int getID(int x, int y)
    {
        if (x == 0 && y == 0) { 
            return 0;

        } else if (x == 0) {
            return y + y%SideSize;
        } else if (y == 0) {
            return x + x%SideSize;
        } else {
            return x + y + x%SideSize + y%SideSize + x%y + y%x;
        }
    }

    private float DotProduct(double x,double y,float xi,float yi)
    {
        return (float)((x * xi) + (y * yi));
    }

    private float wAdv(float a1,float a2,float w)
    {
        w = w / SideSize;
       // Debug.Log(w);
       //eturn (x + y) / 2;
        return (1 - w) * a1+ w * a2;
    }

}