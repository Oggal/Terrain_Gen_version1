using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
   
    public Vector3 Goal;
    public bool ReachedGoal = true;
    public int Score = 0;
    public Text Scoretext;
    public GameObject Becon;
    public int Dis = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(ReachedGoal)
        {
            ReachedGoal = false;
            Goal = GetNewGoal();
            Becon.transform.position = Goal + new Vector3(0,-50,0);
        }

        Vector3 FlatPos = transform.position;
        FlatPos.y = 0;

        if (Vector3.Distance(FlatPos, Goal) < 5 && !ReachedGoal)
        {
            ReachedGoal = true;
            Score += 1;
            Scoretext.enabled = true;
            Scoretext.text = Score.ToString();
        }

	}

    void OnEnable()
    {
        Becon.SetActive(true);
    }



    Vector3 GetNewGoal()
    {

        Vector3 Pos = (Random.onUnitSphere);
        Pos = Pos * Dis;
        Pos.y = 0;
        return new Vector3(transform.position.x + Pos.x, 0, transform.position.z + Pos.z);
    }
}
 