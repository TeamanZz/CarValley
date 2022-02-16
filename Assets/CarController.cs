using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public FloatingJoystick joystick;
    Rigidbody carRB;
    public float speed;
    float horizontal;
    public float turnSpeed;
    Quaternion startRotation;

    private void Awake()
    {
        carRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        horizontal = joystick.Horizontal;
        carRB.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(horizontal, Vector3.up);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
