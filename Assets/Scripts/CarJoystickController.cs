using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJoystickController : MonoBehaviour
{
    public static CarJoystickController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StopCar()
    {
        carRB.velocity = Vector3.zero;
        carRB.angularVelocity = Vector3.zero;
        if (!CarFallDetector.Instance.canFall)
        {
            carRB.constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            carRB.constraints = RigidbodyConstraints.FreezePositionX;
            carRB.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void MoveCar()
    {
        carRB.constraints = RigidbodyConstraints.None;
    }

    public float speed;
    public FloatingJoystick joystick;
    public Rigidbody carRB;
}
