using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace e23.VehicleController
{
    public class VehicleBehaviour : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Parent for the vehicle model.")]
        [SerializeField] private Transform vehicleModel;
        [Tooltip("Assign the sphere collider which is on the same GameObject as the rigidbody. TIP: Use the Vehicle Builder window to have this auto assigned when creating a vehicle.")]
        [SerializeField] private Rigidbody physicsSphere;

        [Header("Vehicle")]
        [Tooltip("Assign the parent transform which makes up the body of the vehicle.")]
        [SerializeField] private Transform vehicleBody;

        [Header("Vehicle Type")]
        [Tooltip("Choose how many wheels the vehicle has.")]
        [SerializeField] private VehicleType vehicleType;

        [Header("Wheels")]
        [Tooltip("Assign the transform of the front left wheel. TIP: If you are seeing incorrect rotations when driving, check the docs for guides on troubleshooting.")]
        [SerializeField] private Transform frontLeftWheel;
        [Tooltip("Assign the transform of the front right wheel. TIP: If you are seeing incorrect rotations when driving, check the docs for guides on troubleshooting.")]
        [SerializeField] private Transform frontRightWheel;
        [Tooltip("Assign the transform of the back left wheel. TIP: If you are seeing incorrect rotations when driving, check the docs for guides on troubleshooting.")]
        [SerializeField] private Transform backLeftWheel;
        [Tooltip("Assign the transform of the back right wheel. TIP: If you are seeing incorrect rotations when driving, check the docs for guides on troubleshooting.")]
        [SerializeField] private Transform backRightWheel;

        [Header("Settings")]
        [Tooltip("Create and assign a Vehicle Settings ScriptableObject, this object holds the vehicle data (Acceleration, MaxSpeed, Drift, etc). TIP: Clicking the button below, in play mode, allows you to tweak and test values at runtime.")]
        [SerializeField] private VehicleBehaviourSettings vehicleSettings;

        private Transform container, wheelFrontLeftParent, wheelFrontRightParent;

        private float speed, speedTarget, currentSpeed;
        private float rotate, tiltTarget, twoWheelVehicleBodyTilt;
        private float strafeSpeed, strafeTarget, strafeTilt;
        private float wheelRadius;
        private float rayMaxDistance;

        private bool isBoosting;

        private Vector3 containerBase;
        private Vector3 modelHeightOffGround;
        private List<Transform> vehicleWheels;

        public VehicleType VehicleWheelCount { get { return vehicleType; } set { vehicleType = value; } }
        public Transform VehicleModel { get { return vehicleModel; } set { vehicleModel = value; } }
        public Rigidbody PhysicsSphere { get { return physicsSphere; } set { physicsSphere = value; } }

        public Transform VehicleBody { get { return vehicleBody; } set { vehicleBody = value; } }
        public Transform FrontLeftWheel { get { return frontLeftWheel; } set { frontLeftWheel = value; } }
        public Transform FrontRightWheel { get { return frontRightWheel; } set { frontRightWheel = value; } }
        public Transform BackLeftWheel { get { return backLeftWheel; } set { backLeftWheel = value; } }
        public Transform BackRightWheel { get { return backRightWheel; } set { backRightWheel = value; } }

        public VehicleBehaviourSettings VehicleSettings { get { return vehicleSettings; } set { vehicleSettings = value; } }

        public float Acceleration => VehicleSettings.acceleration;
        public float MaxSpeed { get; set; }
        public float BreakSpeed => VehicleSettings.breakSpeed;
        public float BoostSpeed => VehicleSettings.boostSpeed;
        public float MaxSpeedToStartReverse => VehicleSettings.maxSpeedToStartReverse;
        public float Steering { get; set; }
        public float MaxStrafingSpeed => VehicleSettings.maxStrafingSpeed;
        public float Gravity => VehicleSettings.gravity;
        public float Drift => VehicleSettings.drift;
        public float VehicleBodyTilt => VehicleSettings.vehicleBodyTilt;
        public float ForwardTilt => VehicleSettings.forwardTilt;
        public bool TurnInAir => VehicleSettings.turnInAir;
        public bool TurnWhenStationary => VehicleSettings.turnWhenStationary;
        public bool TwoWheelTilt => VehicleSettings.twoWheelTilt;
        public bool StopSlopeSlide => VehicleSettings.stopSlopeSlide;
        public float RotateTarget { get; private set; }
        public bool NearGround { get; private set; }
        public bool OnGround { get; private set; }
        public LayerMask GroundMask => VehicleSettings.groundMask;
        public float DefaultMaxSpeed => VehicleSettings.maxSpeed;
        public float DefaultSteering => VehicleSettings.steering;

        public bool IsBoosting => isBoosting;
        public float GetVehicleVelocitySqrMagnitude { get { return PhysicsSphere.velocity.sqrMagnitude; } }
        public Vector3 GetVehicleVelocity { get { return PhysicsSphere.velocity; } }

        public float myAcceleration;

        public Transform p1;
        public Transform p2;

        private void Awake()
        {
            GetRequiredComponents();
            CreateWheelList();
            SetVehicleSettings();
        }

        private void GetRequiredComponents()
        {
            if (vehicleBody == null) { Debug.LogError("Vehicle body has not been assigned on the VehicleBehaviour", gameObject); }

            if (frontLeftWheel != null)
            {
                wheelFrontLeftParent = frontLeftWheel.parent;
                GetWheelRadius();
            }

            if (frontRightWheel != null) { wheelFrontRightParent = frontRightWheel.parent; }

            container = VehicleModel.GetChild(0);
            containerBase = container.localPosition;

            modelHeightOffGround = new Vector3(0, transform.localPosition.y, 0);
        }

        private void CreateWheelList()
        {
            if (frontLeftWheel != null || frontRightWheel != null || backLeftWheel != null || BackRightWheel != null)
            {
                vehicleWheels = new List<Transform>();

                if (frontLeftWheel != null) { vehicleWheels.Add(frontLeftWheel); }
                if (frontRightWheel != null) { vehicleWheels.Add(frontRightWheel); }
                if (backLeftWheel != null) { vehicleWheels.Add(backLeftWheel); }
                if (backRightWheel != null) { vehicleWheels.Add(backRightWheel); }
            }
        }

        private void GetWheelRadius()
        {
            Bounds wheelBounds = frontLeftWheel.GetComponentInChildren<Renderer>().bounds;
            wheelRadius = wheelBounds.size.y;
        }

        public void SetVehicleSettings()
        {
            if (VehicleSettings == null)
            {
                Debug.LogError("Vehicle is missing Vehicle Settings asset.", gameObject);
                return;
            }

            MaxSpeed = VehicleSettings.maxSpeed;
            Steering = VehicleSettings.steering;

            rayMaxDistance = Mathf.Abs(VehicleModel.localPosition.y);

            SetAngularDrag();
        }

        private void SetAngularDrag()
        {
            if (VehicleSettings.angularDrag == -1) { return; }

            physicsSphere.angularDrag = VehicleSettings.angularDrag;
        }

        private void Update()
        {
            Accelerate();
            Strafe();

            if (vehicleType == VehicleType.FourWheels || vehicleType == VehicleType.TwoWheels)
            {
                SpinWheels();
            }
        }

        private void FixedUpdate()
        {
            Turn();
            TurnFrontWheels();
            BodyTiltOnMovement();
            TwoWheelVehicleTilt();
            GroundVehicle();

            RaycastHit hitNear;

            OnGround = Physics.Raycast(transform.position, Vector3.down, rayMaxDistance + modelHeightOffGround.y, GroundMask);
            NearGround = Physics.Raycast(transform.position, Vector3.down, out hitNear, rayMaxDistance + modelHeightOffGround.y + 1f, GroundMask);

            VehicleModel.up = Vector3.Lerp(VehicleModel.up, hitNear.normal, Time.deltaTime * 8.0f);
            VehicleModel.Rotate(0, transform.eulerAngles.y, 0);

            if (NearGround)
            {
                PhysicsSphere.AddForce(transform.forward * speedTarget, ForceMode.Acceleration);
                PhysicsSphere.AddForce(transform.right * strafeTarget, ForceMode.Acceleration);
            }
            else
            {
                PhysicsSphere.AddForce(transform.forward * (speedTarget / 10), ForceMode.Acceleration);
                PhysicsSphere.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
            }

            Vector3 localVelocity = transform.InverseTransformVector(PhysicsSphere.velocity);
            localVelocity.x *= 0.9f + (Drift / 10);

            if (NearGround)
            {
                PhysicsSphere.velocity = transform.TransformVector(localVelocity);
            }

            transform.position = PhysicsSphere.transform.position + modelHeightOffGround;

            if (StopSlopeSlide) { CounterSlopes(hitNear.normal); }
        }

        private void Accelerate()
        {
            speedTarget = Mathf.SmoothStep(speedTarget, speed, Time.deltaTime * myAcceleration);
            speed = 0f;
        }

        private void Turn()
        {
            RotateTarget = Mathf.Lerp(RotateTarget, rotate, Time.deltaTime * 4f);
            CalculateTilt(rotate);
            rotate = 0f;

            float yRotation = speedTarget < 0 ? transform.eulerAngles.y - RotateTarget : transform.eulerAngles.y + RotateTarget;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, yRotation, 0)), Time.deltaTime * 2.0f);

            // v.19 - Legacy code.
            // AVC was shipped with the below line of code, and has incorrect turning for a reversing vehicle. Commenting out and keeping the code for now in case previous buyers want to keep this behaviour.
            // This will be removed in a future update
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + RotateTarget, 0)), Time.deltaTime * 2.0f);
        }

        private void Strafe()
        {
            strafeTarget = Mathf.SmoothStep(strafeTarget, strafeSpeed, Time.deltaTime * Acceleration);
            strafeSpeed = 0;

            CalculateTilt(strafeTilt);
            strafeTilt = 0;
        }

        private void CalculateTilt(float tilt)
        {
            tiltTarget = Mathf.Lerp(tiltTarget, tilt, Time.deltaTime * 4f);
        }

        private void TurnFrontWheels()
        {
            if (wheelFrontLeftParent != null) { wheelFrontLeftParent.localRotation = Quaternion.Euler(wheelFrontLeftParent.localRotation.x, RotateTarget / 2, 0); }
            if (wheelFrontRightParent != null) { wheelFrontRightParent.localRotation = Quaternion.Euler(wheelFrontRightParent.localRotation.x, RotateTarget / 2, 0); }
        }

        private void BodyTiltOnMovement()
        {
            float xRotation = ForwardTilt == 0 ? 0 : speedTarget / ForwardTilt;
            float zRotation = VehicleBodyTilt == 0 ? RotateTarget / 6 : (RotateTarget / 6) * VehicleSettings.vehicleBodyTilt;

            vehicleBody.localRotation = Quaternion.Slerp(vehicleBody.localRotation, Quaternion.Euler(new Vector3(xRotation, 0, zRotation)), Time.deltaTime * 4f);
        }

        private void TwoWheelVehicleTilt()
        {
            if (TwoWheelTilt == false) { return; }

            twoWheelVehicleBodyTilt = -tiltTarget / 1.5f;

            container.localPosition = containerBase + new Vector3(0, Mathf.Abs(twoWheelVehicleBodyTilt) / 2000, 0);
            container.localRotation = Quaternion.Slerp(container.localRotation, Quaternion.Euler(0, RotateTarget / 8, twoWheelVehicleBodyTilt), Time.deltaTime * 10.0f);
        }

        private void SpinWheels()
        {
            currentSpeed = Vector3.Dot(transform.forward, PhysicsSphere.velocity);

            float distanceTraveled = currentSpeed * Time.deltaTime;
            float rotationInRadians = distanceTraveled / wheelRadius;
            float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;

            for (int i = 0; i < vehicleWheels.Count; i++)
            {
                vehicleWheels[i].Rotate(rotationInDegrees, 0, 0);
            }
        }

        private void GroundVehicle()
        {
            // Keeps vehicle grounded when standing still
            if (speed == 0 && GetVehicleVelocitySqrMagnitude < 4f)
            {
                PhysicsSphere.velocity = Vector3.Lerp(PhysicsSphere.velocity, Vector3.zero, Time.deltaTime * 2.0f);
            }
        }

        private void CounterSlopes(Vector3 groundNormal)
        {
            Vector3 carForward = transform.right;
            Vector3 gravity = Physics.gravity;
            Vector3 directionOfFlat = Vector3.Cross(-gravity, groundNormal).normalized; //the direction that if you head in you wouldnt change altitude
            Vector3 directionOfSlope = Vector3.Cross(directionOfFlat, groundNormal); //the direction down the slope
            float affectOfGravity = Vector3.Dot(gravity, directionOfSlope); // returns 1 on a cliff face, 0 on a plane
            float affectOfWheelAlignment = Mathf.Abs(Vector3.Dot(carForward, directionOfSlope)); // returns 1 if facing down or up the slope, 0 if 90 degrees to slope
            PhysicsSphere.AddForce(-directionOfSlope * affectOfWheelAlignment * affectOfGravity, ForceMode.Acceleration);
        }

        /// <summary>
        /// Change the MaxSpeed of the vehicle. Use DefaultMaxSpeed to return to the original MaxSpeed
        /// </summary>
        /// <param name="speedPenalty"></param>
        public void MovementPenalty(float speedPenalty)
        {
            MaxSpeed = speedPenalty;
        }

        /// <summary>
        /// Change the Steering speed of the vehicle. Use DefaultSteering to return to the original Steering
        /// </summary>
        /// <param name="steerPenalty"></param>
        public void SteeringPenalty(float steerPenalty)
        {
            Steering = steerPenalty;
        }

        // Input controls	

        /// <summary>
        /// Move the vehicle foward
        /// </summary>
        public void ControlAcceleration(float acceleration)
        {
            if (!isBoosting)
            {
                myAcceleration = acceleration * Acceleration;
                speed = MaxSpeed;
            }
            else
            {
                speed = BoostSpeed;
            }
        }

        public void FuckAcceleration()
        {
            // if (speed > 0)
            speed -= 12;
            // speed = 0;
        }

        /// <summary>
        /// Slow down and reverse
        /// </summary>
        public void ControlBrake()
        {
            if (GetVehicleVelocitySqrMagnitude > MaxSpeedToStartReverse)
            {
                speed -= BreakSpeed;
            }
            else
            {
                Debug.Log(speed);
                speed = -MaxSpeed;
            }
        }

        /// <summary>
        /// Turn left (int -1) or right (int 1). 
        /// </summary>
        /// <param name="direction"></param>
        public void ControlTurning(float direction)
        {
            if (TurnWhenStationary == false && GetVehicleVelocitySqrMagnitude < 0.1f) { return; }

            if (NearGround || TurnInAir)
            {
                rotate = Steering * direction;
            }
        }

        /// <summary>
        /// Move sideways, left (int -1) or right (int 1)
        /// </summary>
        /// <param name="direction"></param>
        public void ControlStrafing(int direction)
        {
            strafeSpeed = MaxStrafingSpeed * (direction * 2);
            strafeTilt = Steering * direction;
        }

        /// <summary>
        /// Sets isBoosting to true. Set your boost speed in the VehicleSettings asset
        /// </summary>
        public void Boost()
        {
            isBoosting = true;
        }

        /// <summary>
        /// Performs a timed boost, pass in a float for how long the boost should last in seconds
        /// </summary>
        /// <param name="boostLength"></param>
        public void OneShotBoost(float boostLength)
        {
            if (isBoosting == false)
            {
                StartCoroutine(BoostTimer(boostLength));
            }
        }

        private IEnumerator BoostTimer(float boostLength)
        {
            Boost();

            yield return new WaitForSeconds(boostLength);

            StopBoost();
        }

        /// <summary>
        /// Sets isBoosting to false
        /// </summary>
        public void StopBoost()
        {
            isBoosting = false;
        }

        /// <summary>
        /// Set the position and rotation of the vehicle. This will also set the speed and turning to 0
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            PhysicsSphere.velocity = Vector3.zero;
            PhysicsSphere.angularVelocity = Vector3.zero;
            PhysicsSphere.position = position;

            speed = speedTarget = rotate = 0.0f;

            PhysicsSphere.Sleep();
            transform.SetPositionAndRotation(position, rotation);
            PhysicsSphere.WakeUp();
        }
    }
}