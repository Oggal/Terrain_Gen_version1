using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStart : MonoBehaviour {
    public TerrainTileControler TTC;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TTC.buildWorld();
            TTC.Player.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TTC.Load();
            TTC.Player.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
	}
}
