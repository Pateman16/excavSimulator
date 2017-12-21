using CielaSpike;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using UnityEngine;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

public class UDPThreading : MonoBehaviour
{

    public string datareceived = "";
    private bool isConnected = false, coroutinesStarted = false;
    private UdpClient UDPclient;
    private AccelerometerScript accScript;
    // Use this for initialization
    //"194.47.11.60"
    //@"C:\Program Files\Python36\python.exe";
    //start.Arguments = @"C:\Users\patkar15\PycharmProjects\canOpenPython\getAccelerometer.py"
    void Start()
    {
        /*Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Program Files\Python36\python.exe";
        startInfo.Arguments = @"C:\Users\patkar15\Documents\pythonrepository\Servomotors2.py";
        p.StartInfo = startInfo;
        p.Start();*/
        // Open UDP script

        UnityEngine.Debug.Log("Waiting for connection...");

        UDPclient = new UdpClient();
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        UDPclient.Connect(ep);

        UnityEngine.Debug.Log(UDPclient.Available);
        GameObject exAccScript = GameObject.Find("Excavator");
        accScript = exAccScript.GetComponent<AccelerometerScript>();
        isConnected = true;
        StartCoroutine("manage_data_UDP");//Start coroutine for manage the data
        coroutinesStarted = true;
        UnityEngine.Debug.Log("Connected.");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConnected && coroutinesStarted)
        {
            StopAllCoroutines();
            coroutinesStarted = false;
            UnityEngine.Debug.Log("all Coroutines stopped");
        }
    }

    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Client disconnected.");
        StopCoroutine("manage_data_UDP");
    }

    IEnumerator manage_data_UDP()
    {
        Task taskUDP;
        this.StartCoroutineAsync(Blocking_UDP(), out taskUDP);
        yield return StartCoroutine(taskUDP.Wait());
    }

    IEnumerator Blocking_UDP()
    {
        int sleep = int.MaxValue;

        //test cycle, I tried with the while cicle but it crashes Unity,
        // when this "for" cicle is executing , it reads data and prints them almost in sync when the serial device sends data
        //If you have a best idea, please share !!!
        for (int i = 0; i <= int.MaxValue; i++)
        {
            send_data(accScript.getPRY());
            //wait 0.1 seconds and read again
            yield return new WaitForSeconds(0.05F);

        }
        Thread.Sleep(sleep);
    }

    
    void send_data(string data)
    {

        try
        {
            var sendData = System.Text.Encoding.ASCII.GetBytes(data);
            UDPclient.Send(sendData, sendData.Length);
        }
        catch (EndOfStreamException)
        {
            UnityEngine.Debug.Log("server disconnected");
            UDPclient.Close();
            UDPclient.Dispose();
        }
    }

    public bool pipeIsConnected()
    {
        return isConnected;
    }

}
