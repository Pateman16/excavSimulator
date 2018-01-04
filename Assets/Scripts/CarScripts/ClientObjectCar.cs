using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;
namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class ClientObjectCar : MonoBehaviour
    {
        private NetMqListener _netMqListener;
        public string datareceived = "";
        private float xRight, yRight, xLeft, yLeft, driveButtonLeft, driveButtonRight;
        private CarController m_Car; // the car controller we want to use

        private CarUserControl driveController;
        private ClientObject co;
        private float h, v, handbrake;
        public float rotationSpeed;

        private void HandleMessageCar(string message)
        {
            Debug.Log(m_Car);
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
            m_Car.Move(xLeft, yRight, yRight, driveButtonRight);
            //transform.position = new Vector3(x, y, z);
        }

        private void Start()
        {
            m_Car = GetComponent<CarController>();
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
    }
}