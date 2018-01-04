using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;
namespace UnityStandardAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof(AeroplaneController))]
    public class ClientObjectAircraft : MonoBehaviour
    {
        private NetMqListener _netMqListener;
        public string datareceived = "";
        private float xRight, yRight, xLeft, yLeft, driveButtonLeft, driveButtonRight;
        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;

        // reference to the aeroplane that we're controlling
        private AeroplaneController m_Aeroplane;

        private void HandleMessageCar(string message)
        {
            datareceived = message;
            var splittedStrings = message.Split(',');
            if (splittedStrings.Length != 6) return;

            xRight = float.Parse(splittedStrings[0]);
            yRight = float.Parse(splittedStrings[1]);
            xRight = xRight / 1023f;
            yRight = yRight / 1023f;

            xLeft = float.Parse(splittedStrings[2]);
            yLeft = float.Parse(splittedStrings[3]);
            xLeft = xLeft / 1023f;
            yLeft = yLeft / 1023f;

            driveButtonRight = float.Parse(splittedStrings[4]);
            driveButtonRight = driveButtonRight / 1023f;

            driveButtonLeft = float.Parse(splittedStrings[5]);
            driveButtonLeft = driveButtonLeft / 1023f;

            Debug.Log("datareceived" + datareceived);
            m_Aeroplane.Move(xLeft, yLeft, 0, yRight, isPushed(driveButtonRight));
        }

        private void Start()
        {
            m_Aeroplane = GetComponent<AeroplaneController>();
            _netMqListener = new NetMqListener(HandleMessageCar);
            _netMqListener.Start();
        }

        private void Update()
        {
            _netMqListener.Update();
        }

        private void OnDestroy()
        {
            _netMqListener.Stop();
        }
        private bool isPushed(float val)
        {
            if(val == 0f)
            {
                return false;
            }
            if(val == 1f)
            {
                return true;
            }
            return false;
        }
    }
}