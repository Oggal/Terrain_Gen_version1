using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    Transform T_Trans;
    MeshCollider    M_Coll;
    MeshFilter      M_Filt;
    MeshRenderer    M_Rend;
    Mesh            M_mesh;
    TerrainData     TD_Data;
    TerrainData     TD_Octive2;
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
        M_Coll = GetComponent<MeshCollider>();
        M_Filt = GetComponent<MeshFilter>();
        M_Rend = GetComponent<MeshRenderer>();
        T_Trans = transform;
        TD_Data = new TerrainData(Seed);
        TD_Octive2 = new TerrainData(Seed + 59);
        TD_Octive2.SideSize = OocSize;
        Vector3[] Verts = new Vector3[EdgeSize * EdgeSize];
        Vector2[] UVs = new Vector2[EdgeSize * EdgeSize];
        int[] Trys = new int[6 * ((EdgeSize - 1) * (EdgeSize - 1))];
        float wX = T_Trans.position.x;
        float wY = T_Trans.position.z;
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
                    (localWeight * TD_Data.getHeight(Xi + wX, Yi + wY))
                        + (TD_Octive2.getHeight(Xi + wX, Yi + wY) * octWeight),
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

        M_Filt.mesh = M_mesh;
        M_Coll.sharedMesh = M_mesh;
    }


}

public class TerrainData
{

    int Seed;
    int[] QuadSeeds = new int[5];
    public int SideSize = 5;

    public TerrainData(object i)
    {
        this.Seed = i.GetHashCode();


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
            d1 = DotProduct(Math.Cos(v1 * 2 * Math.PI), Math.Sin(v1 * 2 * Math.PI), x1 - x, y1 - y);
            d2 = DotProduct(Math.Cos(v2 * 2 * Math.PI), Math.Sin(v2 * 2 * Math.PI), x1 - x, y2 - y);
            d3 = DotProduct(Math.Cos(v3 * 2 * Math.PI), Math.Sin(v3 * 2 * Math.PI), x2- x, y1 - y);
            d4 = DotProduct(Math.Cos(v4 * 2 * Math.PI), Math.Sin(v4 * 2 * Math.PI), x2 - x, y2 - y);


            return wAdv(wAdv(d1, d3, x - x1), wAdv(d2, d4, x - x1), y-y1);
    }

    private int getID(int x, int y)
    {
        return (x >> 5)+y|x;
        /*
        if (x == 0 && y == 0) { 
            return 0;

        } else if (x == 0) {
            return y + y*y;
        } else if (y == 0) {
            return x + x*x;
        } else {
            return x + y + (x % y) + (y % x)+(x* x) + (y* y) + (x* y); 
        }
        */
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