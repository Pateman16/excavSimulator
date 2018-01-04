using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
public class ServerObjectAircraft : MonoBehaviour
{
    public bool Connected;
    private NetMqPublisher _netMqPublisher;
    private string _response;
    private AccelerometerScript accScript;

    private void Start()
    {
        GameObject exAccScript = GameObject.Find("AircraftJet");
        accScript = exAccScript.GetComponent<AccelerometerScript>();
        _netMqPublisher = new NetMqPublisher(HandleMessage);
        _netMqPublisher.Start();
    }

    private void Update()
    {
        var position = transform.position;
        _response = accScript.getPRY();//$"{position.x} {position.y} {position.z}";
        Connected = _netMqPublisher.Connected;
    }

    private string HandleMessage(string message)
    {
        // Not on main thread
        return _response;
    }

    private void OnDestroy()
    {
        _netMqPublisher.Stop();
    }
}
