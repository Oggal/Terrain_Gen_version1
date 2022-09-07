using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class POI_Builder : MonoBehaviour
{
    public string Seed = "seed";
    
    public bool UseSeed = false;

    public GameObject last;


    public POI_DataObject Template;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(getSeed());
    }


    public int getSeed()
    {
        int IntSeed;
        if( int.TryParse(Seed, out IntSeed))
        {
            return IntSeed;
        }
        else
        {
            return Seed.GetHashCode();
        }
    } 

    public static GameObject BuildPOI(Vector3 pos,POI_DataObject template,int Seed)
    {
        if(template != null)
        {
            
            GameObject poop = new GameObject("Poop",typeof(MeshFilter),typeof(MeshRenderer));
           var temp = template.BuildMesh(Seed);
            poop.GetComponent<MeshFilter>().mesh = temp;
            return poop;
        }
        return null;
    }

    public static void RebuildPOI(ref GameObject target, POI_DataObject template, int seed)
    {
        if (target == null) { return; }
        if (template != null && target.GetComponent<MeshFilter>() !=null)
        {
            target.GetComponent<MeshFilter>().mesh = template.BuildMesh(seed);

        }
        else { Debug.LogError("HALP"); }
    }

    public void RebuildLast()
    {
        if (!(UseSeed) ){ Seed = Random.Range(-500, 500).ToString(); }
        RebuildPOI(ref last, Template, getSeed());
    }
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(POI_Builder))]
public class POI_BuilderEditor : Editor
{
    bool UseCurentPos = false;
    bool showDefault = true;
    bool cityInfo = false;


    public override void OnInspectorGUI()
    {
        
        showDefault = GUILayout.Toggle(showDefault, "Show Default Inspector");
        if (showDefault)
            DrawDefaultInspector();

        if (GUILayout.Button(new GUIContent("Build Example", "Does Nothing!")))
        {   
            if(!(target as POI_Builder).UseSeed) { (target as POI_Builder).Seed = Random.Range(-500, 500).ToString(); }
          (target as POI_Builder).last= POI_Builder.BuildPOI(Vector3.zero, (target as POI_Builder).Template, (target as POI_Builder).getSeed());
        }
        if(GUILayout.Button(new GUIContent("Rebuild Last Example")))
        {
            if (!(target as POI_Builder).UseSeed) { (target as POI_Builder).Seed = Random.Range(-500, 500).ToString(); }
            POI_Builder.RebuildPOI(ref (target as POI_Builder).last, (target as POI_Builder).Template, (target as POI_Builder).getSeed());
        }

    }

}

#endif