using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketCollision : MonoBehaviour {
    public GameObject exc, ArmA, ArmB, Bucket;
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision col)
    {
        // how much the excavator should be knocked back
        var magnitude = -1;
        // If the object we hit is the terrain
        if (col.gameObject.name == "Terrain")
        {
            Debug.Log("terrain collider");
            // calculate force vector 
            var force = col.impulse / Time.fixedDeltaTime;
            //var force = col.relativeVelocity*GetComponent<Rigidbody>().mass;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            Bucket.GetComponent<Rigidbody>().mass = 10000;
            exc.GetComponent<Rigidbody>().mass = 1;
            /*JointSpring BucketSpring = Bucket.GetComponent<HingeJoint>().spring;
            BucketSpring.damper = 1000000000000;
            BucketSpring.spring = 0;
            Bucket.GetComponent<HingeJoint>().spring = BucketSpring;
            Debug.Log(BucketSpring.damper);*/


            /*JointSpring ArmBSpring = ArmB.GetComponent<HingeJoint>().spring;
            ArmBSpring.damper = 1000000000000;
            ArmBSpring.spring = 0;
            Bucket.GetComponent<HingeJoint>().spring = ArmBSpring;
            Debug.Log(ArmBSpring.damper);*/

            //Bucket.GetComponent<Rigidbody>().AddForce(force);
            //exc.GetComponent<Rigidbody>().AddForce(magnitude * force, ForceMode.Acceleration);
            ArmA.GetComponent<Rigidbody>().AddForce(magnitude * force, ForceMode.Acceleration);
            ArmB.GetComponent<Rigidbody>().AddForce(magnitude * force, ForceMode.Acceleration);
        }
    }
    void OnCollisionExit(Collision col)
    {
        Bucket.GetComponent<Rigidbody>().mass = 1;
        exc.GetComponent<Rigidbody>().mass = 10000;
        /*Bucket.GetComponent<Rigidbody>().mass = 50;
        JointSpring BucketSpring = Bucket.GetComponent<HingeJoint>().spring;
        BucketSpring.damper = 10000;
        BucketSpring.spring = 50000;
        Bucket.GetComponent<HingeJoint>().spring = BucketSpring;


        ArmB.GetComponent<Rigidbody>().mass = 100;
        JointSpring ArmBSpring = ArmB.GetComponent<HingeJoint>().spring;
        ArmBSpring.damper = 10000;
        ArmBSpring.spring = 50000;
        Bucket.GetComponent<HingeJoint>().spring = ArmBSpring;


        ArmA.GetComponent<Rigidbody>().mass = 200;
        exc.GetComponent<Rigidbody>().mass = 10000;*/
    }
}
