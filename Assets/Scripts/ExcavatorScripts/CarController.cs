using System;
using UnityEngine;




internal enum SpeedType
{
    MPH,
    KPH
}

public class CarController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider[] m_WheelCollidersLeft = new WheelCollider[9];
    [SerializeField]
    private WheelCollider[] m_WheelCollidersRight = new WheelCollider[9];

    [SerializeField]
    private Vector3 m_CentreOfMassOffset;
    [Range(0, 1)]
    [SerializeField]
    private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField]
    private float m_FullTorqueOverAllWheels;
    [SerializeField]
    private float m_ReverseTorque;
    [SerializeField]
    private float m_Downforce = 100f;
    [SerializeField]
    private SpeedType m_SpeedType;
    [SerializeField]
    private float m_Topspeed = 200;

    [SerializeField]
    private float m_SlipLimit;
    [SerializeField]
    private float m_BrakeTorque;


    private float m_CurrentTorque;
    private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    public float BrakeInputLeft { get; private set; }
    public float BrakeInputRight { get; private set; }
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 2.23693629f; } }
    public float MaxSpeed { get { return m_Topspeed; } }
    public float Revs { get; private set; }
    public float AccelInputLeft { get; private set; }
    public float AccelInputRight { get; private set; }

    // Use this for initialization
    private void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    public void Move(float yLeft, float yRight)
    {
        float accelLeft, footbrakeLeft, accelRight, footbrakeRight;

        if (yLeft < 0f)
        {
            accelLeft = 0f;
            footbrakeLeft = yLeft;
        }
        else
        {
            footbrakeLeft = 0f;
            accelLeft = yLeft;
        }
        if (yRight < 0f)
        {
            accelRight = 0f;
            footbrakeRight = yRight;
        }
        else
        {
            footbrakeRight = 0f;
            accelRight = yRight;
        }



        //clamp input values
        AccelInputLeft = accelLeft = Mathf.Clamp(accelLeft, 0, 1);
        BrakeInputLeft = footbrakeLeft = -1 * Mathf.Clamp(footbrakeLeft, -1, 0);
        AccelInputRight = accelRight = Mathf.Clamp(accelRight, 0, 1);
        BrakeInputRight = footbrakeRight = -1 * Mathf.Clamp(footbrakeRight, -1, 0);

        adjustHillSlide(accelLeft, footbrakeLeft, accelRight, footbrakeRight);

        ApplyDrive(accelLeft, footbrakeLeft, accelRight, footbrakeRight);
        CapSpeed();


        AddDownForce();
        TractionControl();
    }


    private void CapSpeed()
    {
        float speed = m_Rigidbody.velocity.magnitude;
        switch (m_SpeedType)
        {
            case SpeedType.MPH:

                speed *= 2.23693629f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;
                break;

            case SpeedType.KPH:
                speed *= 3.6f;
                if (speed > m_Topspeed)
                    m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized;
                break;
        }
    }


    private void ApplyDrive(float accelLeft, float footbrakeLeft, float accelRight, float footbrakeRight)
    {
        float thrustTorqueLeft, thrustTorqueRight;

        thrustTorqueLeft = accelLeft * (m_CurrentTorque / 9f);
        thrustTorqueRight = accelRight * (m_CurrentTorque / 9f);
        for (int i = 0; i < 9; i++)
        {
            m_WheelCollidersLeft[i].motorTorque = thrustTorqueLeft;
            m_WheelCollidersRight[i].motorTorque = thrustTorqueRight;
        }



        for (int i = 0; i < 9; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 100f)
            {
                m_WheelCollidersLeft[i].brakeTorque = m_BrakeTorque * footbrakeLeft;
                m_WheelCollidersRight[i].brakeTorque = m_BrakeTorque * footbrakeRight;
            }
            else if (footbrakeLeft > 0 || footbrakeRight > 0)
            {
                if (footbrakeLeft > 0)
                {
                    m_WheelCollidersLeft[i].brakeTorque = 0f;
                    m_WheelCollidersLeft[i].motorTorque = -m_ReverseTorque * footbrakeLeft;
                }
                if(footbrakeRight > 0)
                {
                    m_WheelCollidersRight[i].brakeTorque = 0f;
                    m_WheelCollidersRight[i].motorTorque = -m_ReverseTorque * footbrakeRight;
                }


            }

        }
    }





    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_WheelCollidersLeft[0].attachedRigidbody.AddForce(-transform.up * m_Downforce * m_WheelCollidersLeft[0].attachedRigidbody.velocity.magnitude);
        m_WheelCollidersRight[0].attachedRigidbody.AddForce(-transform.up * m_Downforce * m_WheelCollidersRight[0].attachedRigidbody.velocity.magnitude);
    }


    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHitLeft;
        WheelHit wheelHitRight;


        // loop through all wheels
        for (int i = 0; i < 9; i++)
        {
            m_WheelCollidersLeft[i].GetGroundHit(out wheelHitLeft);
            m_WheelCollidersRight[i].GetGroundHit(out wheelHitRight);



            AdjustTorque(wheelHitLeft.forwardSlip);
            AdjustTorque(wheelHitRight.forwardSlip);
        }
    }


    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_FullTorqueOverAllWheels)
            {
                m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }
    }

    private void adjustHillSlide(float accelLeft, float footbrakeLeft, float accelRight, float footbrakeRight)
    {
        if (accelLeft == 0f && accelRight == 0f && footbrakeLeft == 0f && footbrakeRight == 0f)
        {
            for (int i = 0; i < 9; i++)
            {
                if (m_WheelCollidersLeft[i].rpm != 0)
                {

                    m_WheelCollidersLeft[i].brakeTorque = 1f;

                    m_WheelCollidersRight[i].brakeTorque = 1f;
                }
                if (m_WheelCollidersRight[i].rpm != 0)
                {

                    m_WheelCollidersLeft[i].brakeTorque = 1f;

                    m_WheelCollidersRight[i].brakeTorque = 1f;
                }

            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                m_WheelCollidersLeft[i].brakeTorque = 0f;

                m_WheelCollidersRight[i].brakeTorque = 0f;
            }
        }
    }



}
