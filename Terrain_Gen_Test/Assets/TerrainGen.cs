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
	// Use this for initialization
	void Start () {
        M_Coll = GetComponent<MeshCollider>();
        M_Filt = GetComponent<MeshFilter>();
        M_Rend = GetComponent<MeshRenderer>();
        T_Trans = transform;
        TD_Data = new TerrainData(Seed);
        M_mesh = new Mesh();
        M_mesh.name = "World Mesh";
        buildMesh();
        M_Filt.mesh = M_mesh;
        M_Coll.sharedMesh = M_mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
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
    int SideSize = 5;

    private System.Random rand;
    private System.Random[] QuadRand = new System.Random[5];
    public TerrainData(object i)
    {
        this.Seed = i.GetHashCode();
        rand = new System.Random(Seed);
        for(int a = 0; a < 5; a++)
        {
            QuadRand[a] = new System.Random(rand.Next());
        }
    }

    public float getHeight(int x,int y)
    {
        //Get Quad We are in, 0 = axis, 1-4 are the quads
        int quad = 0;
        if (x == 0 || y == 0)
        {
            quad = 0;
        }
        else
        {
            if (x < 0)
            {
                quad = (y < 0 ? 3 : 2);
            }
            else
            {
                quad = (y < 0 ? 4 : 1);
            }
        }
        //Our Corner Points
        int[] c1 = new int[2];
        int[] c2 = new int[2];
        int[] c3 = new int[2];
        int[] c4 = new int[2];
        //The Vectors Assigned to said points
        float[] V1 = new float[2];
        float[] V2 = new float[2];
        float[] V3 = new float[2];
        float[] V4 = new float[2];
        //Find The Corners
        c1[0] = (x / SideSize) * SideSize;
        c1[1] = (y / SideSize) * SideSize;

        c2[0] = ((x / SideSize) * SideSize)+SideSize;
        c2[1] = ((y / SideSize) * SideSize);

        c3[0] = ((x / SideSize) * SideSize) + SideSize;
        c3[1] = ((y / SideSize) * SideSize) + SideSize;

        c4[0] = ((x / SideSize) * SideSize);
        c4[1] = ((y / SideSize) * SideSize) + SideSize;
        //Give Each Corner a new Random with unique Seed, this will alow us to quickly recreate the world if need be
        System.Random R1 = new System.Random(QuadSeeds[quad] + getID(c1[0], c1[1]) + 500);
        System.Random R2 = new System.Random(QuadSeeds[quad] + getID(c2[0], c2[1]) + 500);
        System.Random R3 = new System.Random(QuadSeeds[quad] + getID(c3[0], c3[1]) + 500);
        System.Random R4 = new System.Random(QuadSeeds[quad] + getID(c4[0], c4[1]) + 500);

        //Generate the Vectors
        double t = R1.NextDouble();//Temp Var
        V1[0] = Mathf.Cos((float)t * 2 * Mathf.PI);
        V1[1] = Mathf.Sin((float)t * 2 * Mathf.PI);
        t = R2.NextDouble();
        V2[0] = Mathf.Cos((float)t * 2 * Mathf.PI);
        V2[1] = Mathf.Sin((float)t * 2 * Mathf.PI);
        t = R3.NextDouble();
        V3[0] = Mathf.Cos((float)t * 2 * Mathf.PI);
        V3[1] = Mathf.Sin((float)t * 2 * Mathf.PI);
        t = R4.NextDouble();
        V4[0] = Mathf.Cos((float)t * 2 * Mathf.PI);
        V4[1] = Mathf.Sin((float)t * 2 * Mathf.PI);

        //Take DOT Products
        float i,j,k,l;
        i = DotProduct(V1[0], V1[1], (x - c1[0]), (y - c1[1]));
        j = DotProduct(V2[0], V2[1], (x - c2[0]), (y - c2[1]));
        k = DotProduct(V3[0], V3[1], (x - c3[0]), (y - c3[1]));
        l = DotProduct(V4[0], V4[1], (x - c4[0]), (y - c4[1]));

        //Adverage
       float v = wAdv(wAdv(i, j, (x-c1[0]) ), wAdv(k, l, (x-c1[0]) ),(y- c1[1]));

        //Return Value
        //TODO Perlin Noise Function! W00T!
        return v;// (float)(rand.NextDouble()*3)-1 ;
    }

    private int getID(int x, int y)
    {
        return Mathf.RoundToInt((float)(Math.Pow(x, 3) + Math.Pow(y, 3)));
    }

    private float DotProduct(float x,float y,int xi,int yi)
    {
        return (x * xi) + (y * yi);
    }

    private float wAdv(float x,float y,float w)
    {
        return (x + y) / 2;
        //return (1 - w) * x+ w * y;
    }

}