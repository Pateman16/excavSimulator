using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController {

	private GameObject Base, Arm1, Arm2, Bucket, Machine;
    private Rigidbody BaseBody, Arm1Body, Arm2Body, BucketBody;
    private bool drivemode;
	public ExcavatorController(){
		Base = GameObject.Find ("Base1");
        BaseBody = Base.GetComponent<Rigidbody>();
		Arm1 = GameObject.Find ("Arm1Joint");
        Arm1Body = Arm1.GetComponent<Rigidbody>();
        Arm2 = GameObject.Find ("Arm2Joint");
        Arm2Body = Arm2.GetComponent<Rigidbody>();
        Bucket = GameObject.Find ("BucketJoint1");
        BucketBody = Bucket.GetComponent<Rigidbody>();
        Machine = GameObject.Find("ExcavatorV2Simple");
        drivemode = false;

    }

    public void rotateArm1(float upOrDown) {
        Arm1.transform.Rotate(Vector3.forward * Time.deltaTime * upOrDown);
    }
    public void rotateArm2(float upOrDown)
    {
        Arm2.transform.Rotate(Vector3.forward * Time.deltaTime * upOrDown);
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
    public void drive(float driveForOrBac)
    {
        Machine.transform.Translate(Vector3.right * Time.deltaTime * driveForOrBac);
    }

    
}
