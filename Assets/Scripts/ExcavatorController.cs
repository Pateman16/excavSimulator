using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController {

	private GameObject Machine;
    private Rigidbody MachineBody;
    private float maxSpeed;

	public ExcavatorController(){
        Machine = GameObject.Find("ExcavatorV2Simple");
        MachineBody = Machine.GetComponent<Rigidbody>();
        maxSpeed = 30f;

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
        Debug.Log(MachineBody.velocity.sqrMagnitude);
        //Machine.transform.Translate(Vector3.right * Time.deltaTime * driveForOrBac);
    }

    
}
