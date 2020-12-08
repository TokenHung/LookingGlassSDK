using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class LogTracker : MonoBehaviour
{
    private bool IsPlay = false;
    string f = "f";
    static public string Age;
    static public string Gender;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void WriteQualityMatrix(string message)
    {
        StreamWriter LogFile = new StreamWriter(@"C:\vscode\csvtest.csv", true);
        LogFile.WriteLine (message);
        LogFile.Flush();
        LogFile.Close();
    }
    void Update()
    {
        	if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				WriteQualityMatrix(f);
			}
            print(IsPlay);
    }
    public void IsPlaying()
    {
        IsPlay = true;
    }
}
