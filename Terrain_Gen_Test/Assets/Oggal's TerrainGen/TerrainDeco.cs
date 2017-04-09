using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeco : MonoBehaviour {

    [SerializeField]
    private float TreeDensity = 0.5f;
    [SerializeField]
    private float StoneDensity = 0.1f;
    [SerializeField]
    private bool Generate = false;

    private bool GenTrees = false;
    private bool GenStones = false;
    private bool FailedTreeIsStone = false;

    float World_X;
    float World_Z;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Deco()
    {
        World_X = transform.localPosition.x;
        World_Z = transform.localPosition.z;
        if (!Generate) return;
        Vector3[] Verts = GetComponent<MeshFilter>().mesh.vertices;
        if (GenTrees)
        {
            foreach(Vector3 v in Verts)
            {
                RaycastHit hitInfo;
                if(Physics.Raycast(new Vector3(v.x, v.y + 1, v.z), Vector3.down, out hitInfo))
                {
                    hitInfo.normal;
                }
            }
        }
    }
}
