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
using System.Text;

public class PipeThreading : MonoBehaviour
{

    public string datareceived = "";
    private bool isConnected = false, coroutinesStarted = false;

    private NamedPipeServerStream server;
    private AccelerometerScript accScript;
    private BinaryReader br;
    private BinaryWriter bw;
    private bool blockingFlag;
    // Use this for initialization
    //"194.47.11.60"
    //@"C:\Program Files\Python36\python.exe";
    //start.Arguments = @"C:\Users\patkar15\PycharmProjects\canOpenPython\getAccelerometer.py"
    void Awake()
    {
        server = new NamedPipeServerStream("NP", PipeDirection.InOut);
        UnityEngine.Debug.Log("starting server");

        /*ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = @"C:\Program Files\Python36\python.exe";
        UnityEngine.Debug.Log("starting server");
        start.Arguments = @"C:\Users\patkar15\Documents\pythonrepository\TestMotor.py";//@"C:\Users\patkar15\Documents\pythonrepository\TestMotor.py";
        start.UseShellExecute = false;
        start.CreateNoWindow = true;
        start.RedirectStandardOutput = true;
        Process process = new Process();
        process = Process.Start(start);*/




        UnityEngine.Debug.Log("Python namedpipe Starting");
    }
    void Start()
    {
        blockingFlag = false;
        /*Process p = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Program Files\Python36\python.exe";
        startInfo.Arguments = @"C:\Users\patkar15\Documents\pythonrepository\Servomotors2.py";
        p.StartInfo = startInfo;
        p.Start();*/
        // Open UDP script

        UnityEngine.Debug.Log("Waiting for connection...");
        GameObject exAccScript = GameObject.Find("Excavator");
        accScript = exAccScript.GetComponent<AccelerometerScript>();

        server.WaitForConnection();
        isConnected = true;
        StartCoroutine("manage_data");//Start coroutine for manage the data
        coroutinesStarted = true;
        UnityEngine.Debug.Log("Connected.");
        br = new BinaryReader(server);
        bw = new BinaryWriter(server);
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
        server.Close();
        server.Dispose();
        StopCoroutine("manage_data");
    }

    IEnumerator manage_data()
    {
        Task task;
        this.StartCoroutineAsync(Blocking(), out task);
        yield return StartCoroutine(task.Wait());
    }

    IEnumerator Blocking()
    {
        if (blockingFlag == true)
        {
            int sleep = int.MaxValue;

            //test cycle, I tried with the while cicle but it crashes Unity,
            // when this "for" cicle is executing , it reads data and prints them almost in sync when the serial device sends data
            //If you have a best idea, please share !!!
            for (int i = 0; i <= int.MaxValue; i++)
            {

                read_data();
                UnityEngine.Debug.Log("inne i read_data blocking" + blockingFlag);
                blockingFlag = false;
                //wait 0.1 seconds and read again
                yield return new WaitForSeconds(0.05F);

            }
            Thread.Sleep(sleep);
        }
        else if (blockingFlag == false)
        {
            int sleep = int.MaxValue;

            //test cycle, I tried with the while cicle but it crashes Unity,
            // when this "for" cicle is executing , it reads data and prints them almost in sync when the serial device sends data
            //If you have a best idea, please share !!!
            for (int i = 0; i <= int.MaxValue; i++)
            {
                send_data("hej från unity"/*accScript.getPRY()*/);
                UnityEngine.Debug.Log("inne i send_data blocking" + blockingFlag);
                blockingFlag = true;
                //wait 0.1 seconds and read again
                yield return new WaitForSeconds(0.05F);

            }
            Thread.Sleep(sleep);
        }
    }

    /*
*Read the data from the serial port
*/
    void read_data()
    {
        try
        {
            var len = (int)br.ReadUInt32();            // Read string length
            datareceived = new string(br.ReadChars(len));    // Read string
            UnityEngine.Debug.Log(datareceived);
            server.Flush();
        }
        catch (EndOfStreamException)
        {
            UnityEngine.Debug.Log("server disconnected");
            server.Close();
            server.Dispose();
            isConnected = false;
        }
    }

    void send_data(string data)
    {
        try
        {
            
            var buf = Encoding.ASCII.GetBytes(data);
            bw.Write((uint)buf.Length);
            bw.Write(buf);
            server.Flush();
        }
        catch (EndOfStreamException)
        {
            UnityEngine.Debug.Log("server disconnected");
            server.Close();
            server.Dispose();
            isConnected = false;
        }
    }

    public bool pipeIsConnected()
    {
        return isConnected;
    }

}
