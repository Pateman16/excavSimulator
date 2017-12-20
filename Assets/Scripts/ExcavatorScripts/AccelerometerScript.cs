using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AccelerometerScript : MonoBehaviour {

    /*tänk på sin och cos för att jämna ut vinkeln på plattformen relativt till stolens axlar
     
         stolPitch/cos(rotationsgrader) == plattanspitch
         stolRoll/sin(rotationsgrader) == plattanspitch*/


    public GameObject AcceleroMeterObject;
    private Quaternion quat;
    private float x, y, z,w, roll, pitch, yaw;
    public string pitchSend, rollSend, yawSend;
	// Use this for initialization
	void Start () {
        x = 0f;
        y = 0f;
        z = 0f;
        w = 0f;
        roll = 0f;
        pitch = 0f;
        yaw = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        x = AcceleroMeterObject.transform.rotation.x;
        y = AcceleroMeterObject.transform.rotation.y;
        z = AcceleroMeterObject.transform.rotation.z;
        w = AcceleroMeterObject.transform.rotation.w;

        yaw = Mathf.Atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z);
        roll = Mathf.Asin(2 * x * y + 2 * z * w);
        pitch = Mathf.Atan2(2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z);

        //trueRollPitch = calculatePlatformAxes(new Vector3(pitch, roll, yaw));

        //pitch = trueRollPitch.x;
        //roll = trueRollPitch.y;

        pitchSend = "" + Convert.ToString(Math.Round(Convert.ToDecimal(Mathf.Rad2Deg * pitch), 2), new CultureInfo("en-US"));
        rollSend = "" + Convert.ToString(Math.Round(Convert.ToDecimal(Mathf.Rad2Deg * (-1)*roll), 2), new CultureInfo("en-US"));
        yawSend = "" + Convert.ToString(Math.Round(Convert.ToDecimal(Mathf.Rad2Deg * yaw), 2), new CultureInfo("en-US"));

    }
    public string getPRY()
    {
        return pitchSend +", " + rollSend + ", " + yawSend;
    }

    /**
    Input order: Pitch, Roll, Yaw
    Output order: Pitch, Roll
        */
    private Vector2 calculatePlatformAxes(Vector3 acc)
    {
        float rotation = acc.z;
        acc = new Vector3(acc.x, acc.y);
        Vector3 col1 = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), -Mathf.Sin(rotation * Mathf.Deg2Rad));
        Vector3 col2 = new Vector3(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad));
        Vector3 col3 = new Vector3(0, 0, 1);
        Vector3 pitchRoll = new Vector3(acc.x, acc.y);
        Vector3 res = new Vector3(pitchRoll.x * col1.x + pitchRoll.y * col1.y + pitchRoll.z * col1.z, pitchRoll.x * col2.x + pitchRoll.y * col2.y + pitchRoll.z * col2.z, pitchRoll.x * col3.x + pitchRoll.y * col3.y + pitchRoll.z * col3.z);

        return res;
    }
}
