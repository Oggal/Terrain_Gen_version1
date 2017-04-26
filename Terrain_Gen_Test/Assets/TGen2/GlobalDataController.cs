using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataController : MonoBehaviour {

    private GlobalDataController GameControler;
    private TerrainController TerrainCtrl;

	// Use this for initialization
	void Start () {
        PrepSelf();
        if (TerrainCtrl == null)
        {
            TerrainCtrl = new TerrainController();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save()
    {
        //TODO
    }

    public void Load()
    {
        //TODO
    }

	public void Test()
	{
		if (TerrainCtrl == null)
		{
			TerrainCtrl = new TerrainController();
		}
		TerrainCtrl.Test();
	}

    private void PrepSelf()
    {
        if (GameControler != null)
        {
            if (GameControler != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            GameControler = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
