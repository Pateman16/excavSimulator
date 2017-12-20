using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{

    public GameObject ArmA, ArmB, BucketMain, Base1;
    private HingeJoint ArmAHinge, ArmBHinge, BucketHinge, Base1Hinge;
    private float rotateArmA, rotateArmB, rotateBucketMain, rotateBase1;

    public float rotationSpeed;

    private ClientObject co;
    //private PipeThreadingOnlyJoysticks pt;
    private CarController driveController;
    private float xRight, yRight, xLeft, yLeft, driveButtonLeft, driveButtonRight;
    private string[] xyArray;

    // Use this for initialization
    void Start()
    {
        ArmAHinge = ArmA.GetComponent<HingeJoint>();
        ArmBHinge = ArmB.GetComponent<HingeJoint>();
        BucketHinge = BucketMain.GetComponent<HingeJoint>();
        Base1Hinge = Base1.GetComponent<HingeJoint>();
        rotateArmA = 0f;
        rotateArmB = 0f;
        rotateBucketMain = 0f;
        rotateBase1 = 0f;

        GameObject clientScript = GameObject.Find("ExcavatorV2Simple");
        //pt = pipeThreadScript.GetComponent<PipeThreadingOnlyJoysticks>();
        co = clientScript.GetComponent<ClientObject>();
        GameObject exControllerScript = GameObject.Find("Excavator");
        driveController = exControllerScript.GetComponent<CarController>();
        xyArray = new string[6];
    }

    // Update is called once per frame
    void Update()
    {
        xyArray = new string[6];
        /**(xyArray[0-1]: id 0x185 values, xyArray[2-3]: 0x186, xyArray[4-8]: 0x187, xyArray[9-13]: 0x188)*/
        xyArray = co.datareceived.Split(',');
        if (xyArray.Length == 6)
        {
            xyArray[xyArray.Length - 1] = formatLastIndex(xyArray[xyArray.Length - 1]);
            for (int i = 0; i < xyArray.Length; i++)
            {
                if (xyArray[i].Equals(""))
                {
                    xyArray[i] = "0";
                }
            }
            xRight = float.Parse(xyArray[0]);
            yRight = float.Parse(xyArray[1]);
            xRight = xRight / 1023f;
            yRight = yRight / 1023f;

            xLeft = float.Parse(xyArray[2]);
            yLeft = float.Parse(xyArray[3]);
            xLeft = xLeft / 1023f;
            yLeft = yLeft / 1023f;

            driveButtonRight = float.Parse(xyArray[4]);
            driveButtonRight = driveButtonRight / 1023f;

            driveButtonLeft = float.Parse(xyArray[5]);
            driveButtonLeft = driveButtonLeft / 1023f;

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
        }else
        {
            Debug.Log("xyArray wasnt length 6");
        }
    }
    public static string formatLastIndex(string theString)
    {

        if (theString.Length > 4 && theString.Contains("1023"))
        {
            theString = "1023";
            return theString;
        }
        else if (theString.Length > 1 && theString.StartsWith("0"))
        {
            theString = "0";
            return theString;
        }

        return theString;

    }
}
