using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class startPython : MonoBehaviour {
    public Process p;
	// Use this for initialization
	void Start () {
        /*ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = @"C:\Program Files\Python36\python.exe";
        UnityEngine.Debug.Log("starting server");
        start.Arguments = @"C:\Users\patkar15\Documents\pythonrepository\canOpenBUS.py";
        start.UseShellExecute = false;
        start.CreateNoWindow = true;
        start.RedirectStandardOutput = true;
        Process process = new Process();
        process = Process.Start(start);*/


        p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Program Files\Python36\python.exe";
        startInfo.Arguments = @"C:\Users\patkar15\Documents\pythonrepository\canOpenBUS.py";
        startInfo.CreateNoWindow = true;
        //startInfo.UseShellExecute = false;
        p.StartInfo = startInfo;
        p.Start();
    }
    void OnApplicationQuit()
    {
        p.Dispose();
        p.Kill();

    }
}
