using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorControll : MonoBehaviour {

    private ExcavatorController exController;
    private bool driveMode;
	// Use this for initialization
	void Start () {
        exController = new ExcavatorController();
        driveMode = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (driveMode)
            {
                driveMode = false;
            }
            else
            {
                driveMode = true;
            }
            Debug.Log(driveMode);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            exController.rotateArm1(-10f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            exController.rotateArm1(10f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            exController.rotateBase(10f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            exController.rotateBase(-10f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (driveMode)
            {
                exController.drive(5f);
            }
            else
            {
                exController.rotateArm2(-10f);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (driveMode)
            {
                exController.drive(-5f);
            }
            else
            {
                exController.rotateArm2(10f);
            }

        }
        if (Input.GetKey(KeyCode.A))
        {
            if (driveMode)
            {
                exController.rotateDrive(10f);
            }
            else
            {
                exController.rotateBucket(-10f);
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            if (driveMode)
            {
                exController.rotateDrive(-10f);
            }
            else
            {
                exController.rotateBucket(10f);


            }
        }
    }
}
