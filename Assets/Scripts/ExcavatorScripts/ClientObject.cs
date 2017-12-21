using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;

public class NetMqListener
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost:12345");
            subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string frameString;
                if (!subSocket.TryReceiveFrameString(out frameString)) continue;
                //Debug.Log(frameString);
                _messageQueue.Enqueue(frameString);
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup();
    }

    public void Update()
    {
        while (!_messageQueue.IsEmpty)
        {
            string message;
            if (_messageQueue.TryDequeue(out message))
            {
                _messageDelegate(message);
            }
            else
            {
                break;
            }
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}

public class ClientObject : MonoBehaviour
{
    private NetMqListener _netMqListener;
    public string datareceived = "";
    private float xRight, yRight, xLeft, yLeft, driveButtonLeft, driveButtonRight;
    public GameObject ArmA, ArmB, BucketMain, Base1;
    private HingeJoint ArmAHinge, ArmBHinge, BucketHinge, Base1Hinge;
    private float rotateArmA, rotateArmB, rotateBucketMain, rotateBase1;

    private CarController driveController;

    public float rotationSpeed;

    private void HandleMessage(string message)
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
        //Debug.Log("datareceived" + datareceived);
        joyStickControll(xLeft, yLeft, xRight, yRight, driveButtonRight, driveButtonLeft);

        //transform.position = new Vector3(x, y, z);
    }

    private void Start()
    {
        ArmAHinge = ArmA.GetComponent<HingeJoint>();
        ArmBHinge = ArmB.GetComponent<HingeJoint>();
        BucketHinge = BucketMain.GetComponent<HingeJoint>();
        Base1Hinge = Base1.GetComponent<HingeJoint>();
        rotateArmA = 0f;
        rotateArmB = 0f;
        rotateBucketMain = 0f;
        rotateBase1 = 0f;

        GameObject exControllerScript = GameObject.Find("Excavator");
        driveController = exControllerScript.GetComponent<CarController>();

        _netMqListener = new NetMqListener(HandleMessage);
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
    private void joyStickControll(float xLeft, float yLeft, float xRight, float yRight, float driveButtonRight, float driveButtonLeft)
    {

        if (driveButtonRight != 0f && driveButtonLeft != 0f)
            {
                driveController.Move(yLeft, yRight);
            }
            else
            {
                driveController.Move(0f, 0f);
                /**
                 Move right joystick in y direction to controll main arm up and down*/
                if (yRight != 0f)
                {
                    rotateArmA = rotateArmA + rotationSpeed * yRight;
                    if (rotateArmA < ArmAHinge.limits.min)
                    {
                        rotateArmA = ArmAHinge.limits.min;
                    }
                    if (rotateArmA > ArmAHinge.limits.max)
                    {
                        rotateArmA = ArmAHinge.limits.max;
                    }

                }
                /** This jointspring is ArmA's jointspring which is set.*/
                JointSpring ArmASpring = ArmAHinge.spring;

                ArmASpring.targetPosition = rotateArmA;
                ArmAHinge.spring = ArmASpring;

                /**
                Move left joystick in y direction to controll second arm up and down*/
                if (yLeft != 0f)
                {
                    rotateArmB = rotateArmB - rotationSpeed * yLeft;
                    if (rotateArmB < ArmBHinge.limits.min)
                    {
                        rotateArmB = ArmBHinge.limits.min;
                    }
                    if (rotateArmB > ArmBHinge.limits.max)
                    {
                        rotateArmB = ArmBHinge.limits.max;
                    }

                }

                /** This jointspring is ArmB's jointspring which is set.*/
                JointSpring ArmBSpring = ArmBHinge.spring;

                ArmBSpring.targetPosition = rotateArmB;
                ArmBHinge.spring = ArmBSpring;


                /**
                Move right joystick in x direction to controll bucket up and down*/
                if (xRight != 0f)
                {
                    rotateBucketMain = rotateBucketMain - rotationSpeed * xRight;
                    if (rotateBucketMain < BucketHinge.limits.min)
                    {
                        rotateBucketMain = BucketHinge.limits.min;
                    }
                    if (rotateBucketMain > BucketHinge.limits.max)
                    {
                        rotateBucketMain = BucketHinge.limits.max;
                    }


                }
                /** This jointspring is Base1's jointspring which is set.*/
                JointSpring BucketMainSpring = BucketHinge.spring;

                BucketMainSpring.targetPosition = rotateBucketMain;
                BucketHinge.spring = BucketMainSpring;

                /**
                 Move left joystick in x direction to controll the baserotation to the left and right*/
                if (xLeft != 0)
                {
                    rotateBase1 = rotateBase1 + rotationSpeed * xLeft;
                    if (rotateBase1 < Base1Hinge.limits.min)
                    {
                        rotateBase1 = Base1Hinge.limits.min;
                    }
                    if (rotateBase1 > Base1Hinge.limits.max)
                    {
                        rotateBase1 = Base1Hinge.limits.max;
                    }

                }
                /** This jointspring is Base1's jointspring which is set.*/
                JointSpring Base1Spring = Base1Hinge.spring;

                Base1Spring.targetPosition = rotateBase1;

                Base1Hinge.spring = Base1Spring;
            }
        }
}
