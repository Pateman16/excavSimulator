using CielaSpike;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using UnityEngine;

public class PipeThreading : MonoBehaviour
{

    public string datareceived = "";
    private bool isConnected = false, coroutinesStarted = false;

    private NamedPipeServerStream server;
    private BinaryReader br;
    private BinaryWriter bw;
    // Use this for initialization
    void Start()
    {
        // Open the named pipe.
        server = new NamedPipeServerStream("NPtest");

        Debug.Log("Waiting for connection...");

        server.WaitForConnection();
        isConnected = true;
        StartCoroutine("manage_data");//Start coroutine for manage the data
        coroutinesStarted = true;
        Debug.Log("Connected.");
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
            Debug.Log("all Coroutines stopped");
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Client disconnected.");
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
        int sleep = int.MaxValue;

        //test cycle, I tried with the while cicle but it crashes Unity,
        // when this "for" cicle is executing , it reads data and prints them almost in sync when the serial device sends data
        //If you have a best idea, please share !!!
        for (int i = 0; i <= int.MaxValue; i++)
        {
            read_data();
            //wait 0.1 seconds and read again
            yield return new WaitForSeconds(0.05F);

        }
        Thread.Sleep(sleep);
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

            //Debug.Log(datareceived);
        }
        catch (EndOfStreamException)
        {
            Debug.Log("server disconnected");
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
