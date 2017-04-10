using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeco : MonoBehaviour {

    [SerializeField]
    private float TreeDensity = 0.1f;
    [SerializeField]
    private float StoneDensity = 0.1f;
    [SerializeField]
    private bool Generate = false;

    private bool GenTrees = true;
    private bool GenStones = false;
    private bool FailedTreeIsStone = false;
    TerrainGen Parrent;
    private TerrainData[] TreeMap;
    private int Seed;
    public List<GameObject> TreeList;

    private float World_X;
    private float World_Z;
 
	// Use this for initialization
	void Start () {
        CheckNulls();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   

    public void Deco()
    {
        CheckNulls();
        World_X = transform.localPosition.x;
        World_Z = transform.localPosition.z;
        if (!Generate) { return; }
        Vector3[] Verts = GetComponent<MeshFilter>().sharedMesh.vertices;
        if (GenTrees && TreeList.Count >0)
        {

            for (int a = 0; a < Verts.Length; a += 5)
            {
                Vector3 v = Verts[a];
                float TM1 = TreeMap[0].getHeight((v.x + World_X) * 0.01f, (v.z + World_Z) * 0.01f);
                float TM2 = TreeMap[1].getHeight((v.x + World_X) * 0.01f, (v.z + World_Z) * 0.01f);
                if ( !(TM1 < TreeDensity
                    && TM2 <TreeDensity))
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(new Vector3(v.x + World_X, v.y + 1, v.z + World_Z), Vector3.down, out hitInfo, 5f))
                    {
                        if (hitInfo.normal.y > 0.7f &&TreeMap[1].getHeight(hitInfo.point.x, hitInfo.point.y)>0)
                        {
                            int i = ((int)Mathf.Abs(hitInfo.normal.x * 10f * hitInfo.point.z)) % TreeList.Count;
                            Instantiate(TreeList[i], hitInfo.point+new Vector3(hitInfo.normal.z*2,-0.2f,hitInfo.normal.x * 2), Quaternion.Euler(0,36000*hitInfo.normal.x * hitInfo.normal.z,0),transform);
                            a++;
                        }
                    }
                }
            }
        }
    }
     private void CheckNulls()
    {
        if (Parrent == null)
        {
            Parrent = GetComponent<TerrainGen>();
        }
        if (Seed == 0)
        {
            Seed = Parrent.Seed;
        }
        if (TreeMap == null)
        {
            TreeMap = new TerrainData[2];
            TreeMap[0] = new TerrainData(Seed * 2,1);
            TreeMap[1] = new TerrainData(Seed / 2, 1);
        }
    }


}


