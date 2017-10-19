using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController {

	private GameObject Base, Arm1, Arm2, Bucket, Machine;
    private Rigidbody BaseBody, Arm1Body, Arm2Body, BucketBody, MachineBody;
    private JointMotor arm1Motor, arm2Motor, bucketMotor;
    private float maxSpeed;

	public ExcavatorController(){
		Base = GameObject.Find ("Base1");
        BaseBody = Base.GetComponent<Rigidbody>();
		Arm1 = GameObject.Find ("ArmA");
        Arm1Body = Arm1.GetComponent<Rigidbody>();
        Arm2 = GameObject.Find ("Arm2Joint");
        Arm2Body = Arm2.GetComponent<Rigidbody>();
        Bucket = GameObject.Find ("BucketJoint1");
        BucketBody = Bucket.GetComponent<Rigidbody>();
        Machine = GameObject.Find("ExcavatorV2Simple");
        MachineBody = Machine.GetComponent<Rigidbody>();
        maxSpeed = 30f;

        arm1Motor = Arm1.GetComponent<HingeJoint>().motor;
        arm1Motor.targetVelocity = 1;
        arm1Motor.freeSpin = false;

    }

    public void rotateArm1(float upOrDown) {
        //Arm1.transform.Rotate(Vector3.forward * Time.deltaTime * upOrDown);
        arm1Motor.force = upOrDown;
    }
    public void rotateArm2(float upOrDown)
    {
        //Arm2.transform.Rotate(Vector3.forward * Time.deltaTime * upOrDown);
    }
    public void rotateBucket(float upOrDown)
    {
        Bucket.transform.Rotate(Vector3.forward * Time.deltaTime * upOrDown);
    }
    public void rotateBase(float leftOrRight)
    {
        Base.transform.Rotate(Vector3.back * Time.deltaTime * leftOrRight);
    }
    public void rotateDrive(float leftOrRight)
    {
        Machine.transform.Rotate(Vector3.back * Time.deltaTime * leftOrRight);
    }
    public void drive(float driveForOrBac, float Time)
    {
        if (MachineBody.velocity.sqrMagnitude < maxSpeed)
        {
            MachineBody.AddRelativeForce(Vector3.right * driveForOrBac * Time / MachineBody.mass, ForceMode.Acceleration); //Car.transform.forward * Forward * Time.deltaTime / rigidbody.mass, ForceMode.Acceleration
        }
        //Machine.transform.Translate(Vector3.right * Time.deltaTime * driveForOrBac);
    }

    
}
