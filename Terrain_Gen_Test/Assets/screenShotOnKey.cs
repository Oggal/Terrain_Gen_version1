using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenShotOnKey : MonoBehaviour {
    // Update is called once per frame
    void Start()
    {
        Debug.Log(Application.dataPath + "/ScreenShots");
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.dataPath + "/ScreenShots");
        try
        {
            // Determine whether the directory exists.
            if (di.Exists)
            {
                // Indicate that it already exists.
                return;
            }

            // Try to create the directory.
            di.Create();
            Debug.Log("The directory was created successfully.");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
         int x =   System.IO.Directory.GetFiles(Application.dataPath+"/ScreenShots").Length;
            Application.CaptureScreenshot("ScreenShots/Screen" + x+ ".png",2);
        }
	}
}
