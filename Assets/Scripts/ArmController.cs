using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

    public GameObject ArmA, ArmB, BucketMain, Base1;
    private HingeJoint ArmAHinge, ArmBHinge, BucketHinge, Base1Hinge;
    private ConfigurableJoint BucketConfJoint;
    public Vector3 BucketVec;
    private Quaternion QBucketVec;
    private float rotateArmA, rotateArmB, rotateBucketMain, rotateBase1;
    private bool driveMode;
    public float rotationSpeed,rotationSpeedBucket;
    private ExcavatorController exController;

    // Use this for initialization
    void Start () {
        exController = new ExcavatorController();
        ArmAHinge = ArmA.GetComponent<HingeJoint>();
        ArmBHinge = ArmB.GetComponent<HingeJoint>();
        BucketHinge = BucketMain.GetComponent<HingeJoint>();
        BucketConfJoint = BucketMain.GetComponent<ConfigurableJoint>();
        Base1Hinge = Base1.GetComponent<HingeJoint>();
        rotateArmA = 0f;
        rotateArmB = 0f;
        rotateBucketMain = 0f;
        rotateBase1 = 0f;
        rotationSpeed = 0.5f;
        driveMode = false;
    }
	
	// Update is called once per frame
	void Update () {
        /**
         Press the tab button to switch from drivingmode to working mode*/
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (driveMode)
            {
                driveMode = false;
                Debug.Log(driveMode);
            }
            else
            {
                driveMode = true;
                Debug.Log(driveMode);
            }
            
        }

        /**
         Press uparrow to controll main arm up*/
        if (Input.GetKey(KeyCode.UpArrow) && rotateArmA > ArmAHinge.limits.min)
        {
            rotateArmA = rotateArmA - rotationSpeed;
 
        }
        /**
         press downarrow to controll main arm down*/
        if (Input.GetKey(KeyCode.DownArrow) && rotateArmA < ArmAHinge.limits.max)
        {
            rotateArmA = rotateArmA + rotationSpeed;
        }


        /** This jointspring is ArmA's jointspring which is set.*/
        JointSpring ArmASpring = ArmAHinge.spring;
        
        ArmASpring.targetPosition = rotateArmA;

        ArmAHinge.spring = ArmASpring; 

        /**
         Press leftarrow to controll the baserotation to the left*/
        if (Input.GetKey(KeyCode.LeftArrow) && rotateBase1 > Base1Hinge.limits.min)
        {
            rotateBase1 = rotateBase1 - rotationSpeed;
        }
        /**press the rightarrow to controll the baserotation to the right*/
        if (Input.GetKey(KeyCode.RightArrow) && rotateBase1 < Base1Hinge.limits.max)
        {
            rotateBase1 = rotateBase1 + rotationSpeed;
        }
        /** This jointspring is Base1's jointspring which is set.*/
        JointSpring Base1Spring = Base1Hinge.spring;

        Base1Spring.targetPosition = rotateBase1;

        Base1Hinge.spring = Base1Spring;
        /**
         If drivemode = true this is the acceleraton else its the controll of the second arm*/
        if (Input.GetKey(KeyCode.W))
        {
            if (driveMode)
            {
                exController.drive(15000000f, Time.deltaTime);
            }
            else if(rotateArmB > ArmBHinge.limits.min)
            {
                rotateArmB = rotateArmB - rotationSpeed;
            }
        }
        /*If drivemode = true this is the deceleraton else its the controll of the second arm 
         * **/
        if (Input.GetKey(KeyCode.S))
        {
            if (driveMode)
            {
                exController.drive(-15000000f, Time.deltaTime);
            }
            else if (rotateArmB < ArmBHinge.limits.max)
            {
                rotateArmB = rotateArmB + rotationSpeed;
            }

        }

        /** This jointspring is ArmB's jointspring which is set.*/
        JointSpring ArmBSpring = ArmBHinge.spring;

        ArmBSpring.targetPosition = rotateArmB;

        ArmBHinge.spring = ArmBSpring;

        if (BucketMain.GetComponent<Rigidbody>().IsSleeping()) {
            BucketMain.GetComponent<Rigidbody>().WakeUp();
        }
        /**
         If drivemode is enabled this is the turn of the vehicle, else its the bucket rotation.*/
        if (Input.GetKey(KeyCode.A))
        {
            if (driveMode)
            {
                exController.rotateDrive(15f);
            }
            else
            {
                BucketVec[0] = BucketVec[0] + rotationSpeed;
                //rotateBucketMain = rotateBucketMain - rotationSpeed;
            }

        }
        /**
         If drivemode is enabled this is the turn of the vehicle, else its the bucket rotation.*/
        if (Input.GetKey(KeyCode.D))
        {
            if (driveMode)
            {
                exController.rotateDrive(-15f);
            }
            else
            {
                //BucketVec[0] = BucketVec[0] - rotationSpeed;
                BucketVec[0] = BucketVec[0] - rotationSpeed;
                //rotateBucketMain = rotateBucketMain + rotationSpeed;
            }
        }
        /** This jointspring is Base1's jointspring which is set.*/

        BucketConfJoint.targetPosition = BucketVec;
        /*QBucketVec = Quaternion.Euler(BucketVec);
        BucketConfJoint.targetRotation = QBucketVec;*/

        /*JointSpring BucketMainSpring = BucketHinge.spring;

        BucketMainSpring.targetPosition = rotateBucketMain;

        BucketHinge.spring = BucketMainSpring;*/
    }
}
