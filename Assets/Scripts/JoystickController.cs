using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour {

    public GameObject ArmA, ArmB, BucketMain, Base1;
    private HingeJoint ArmAHinge, ArmBHinge, BucketHinge, Base1Hinge;
    private float rotateArmA, rotateArmB, rotateBucketMain, rotateBase1;
    private bool driveMode;
    public float rotationSpeed;
    private ExcavatorController exController;

    private PipeThreading pt;
    private float xRight, yRight, xLeft, yLeft;

    // Use this for initialization
    void Start()
    {
        exController = new ExcavatorController();
        ArmAHinge = ArmA.GetComponent<HingeJoint>();
        ArmBHinge = ArmB.GetComponent<HingeJoint>();
        BucketHinge = BucketMain.GetComponent<HingeJoint>();
        Base1Hinge = Base1.GetComponent<HingeJoint>();
        rotateArmA = 0f;
        rotateArmB = 0f;
        rotateBucketMain = 0f;
        rotateBase1 = 0f;
        rotationSpeed = 0.3f;
        driveMode = false;

        GameObject pipeThreadScript = GameObject.Find("ExcavatorV2Simple");
        pt = pipeThreadScript.GetComponent<PipeThreading>();
    }

    // Update is called once per frame
    void Update()
    {
        /**(xyArray[0-1]: id 0x185 values, xyArray[2-3]: 0x186, xyArray[4-8]: 0x187, xyArray[9-13]: 0x188)*/
        string[] xyArray = pt.datareceived.Split(',');
        xRight = float.Parse(xyArray[0]);
        yRight = float.Parse(xyArray[1]);
        xRight = xRight / 1023f;
        yRight = yRight / 1023f;

        xLeft = float.Parse(xyArray[2]);
        yLeft = float.Parse(xyArray[3]);
        xLeft = xLeft / 1023f;
        yLeft = yLeft / 1023f;

        /**
         Move right joystick in y direction to controll main arm up and down*/
        if (yRight != 0f)
        {
            rotateArmA = rotateArmA + rotationSpeed* yRight;
            if(rotateArmA < ArmAHinge.limits.min)
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
                rotateArmB = rotateArmB - rotationSpeed*yLeft;
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
            rotateBucketMain = rotateBucketMain - rotationSpeed*xRight;
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

        //preliminary drive controll
        if (Input.GetKey(KeyCode.W))
        {
            exController.drive(1700f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
          exController.drive(-1700f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
           exController.rotateDrive(15f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            exController.rotateDrive(-15f);
        }
    }
}
