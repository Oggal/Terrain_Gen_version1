using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenShotOnKey : MonoBehaviour {
    // Update is called once per frame
    void Start()
    {
   
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TakeScreenShot();
        }
    }

    void TakeScreenShot()
    {
        int count = 0;
        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo("Screenshot/");
        if (!d.Exists)
        {
            System.IO.Directory.CreateDirectory(d.ToString());
            d= new System.IO.DirectoryInfo("Screenshot/");
        }
            System.IO.FileInfo[] fis = d.GetFiles();
            count = fis.Length;
            ScreenCapture.CaptureScreenshot("Screenshot/Screenshot_" + count + ".png", 2);
        
    }
}
