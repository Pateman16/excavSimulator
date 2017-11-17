using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float vRight = Input.GetAxis("Vertical"); 
            float vLeft = Input.GetAxis("VerticalW");
            //m_Car.Move(vLeft, vLeft, vRight,vRight);
        }
    }
}
