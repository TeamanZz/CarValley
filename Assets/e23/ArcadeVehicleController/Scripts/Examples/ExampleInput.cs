using UnityEngine;

namespace e23.VehicleController.Examples
{
    public class ExampleInput : MonoBehaviour
    {
        [SerializeField] VehicleBehaviour vehicleBehaviour;

        [Header("Controls")]
        [SerializeField] KeyCode accelerate = KeyCode.W;
        [SerializeField] KeyCode brake = KeyCode.S;
        [SerializeField] KeyCode steerLeft = KeyCode.A;
        [SerializeField] KeyCode steerRight = KeyCode.D;
        // This stops a console warning because strafeLeft and strafeRight are not assigned here, they're open to be assigned in the inspector
#pragma warning disable 0649
        [SerializeField] KeyCode strafeLeft;
        [SerializeField] KeyCode strafeRight;
#pragma warning restore 0649
        [SerializeField] KeyCode boost = KeyCode.Space;
        [SerializeField] KeyCode oneShotBoost = KeyCode.B;

        [Header("Settings")]
        [SerializeField] float boostLength = 1f;

        public FloatingJoystick joystick;

        public VehicleBehaviour VehicleBehaviour
        {
            get { return vehicleBehaviour; }
            set { vehicleBehaviour = value; }
        }

        private void Update()
        {
            // Acceleration
            if (joystick.Vertical > 0.5f) { VehicleBehaviour.ControlAcceleration(joystick.Vertical); }
            if (joystick.Vertical == 0) { VehicleBehaviour.FuckAcceleration(); }

            // Steering
            if (joystick.Horizontal < 0) { VehicleBehaviour.ControlTurning(joystick.Horizontal * 1); }
            if (joystick.Horizontal > 0) { VehicleBehaviour.ControlTurning(joystick.Horizontal * 1); }

            // Strafing
            if (Input.GetKey(strafeLeft)) { VehicleBehaviour.ControlStrafing(-1); }
            if (Input.GetKey(strafeRight)) { VehicleBehaviour.ControlStrafing(1); }

            // Boost
            if (Input.GetKeyDown(boost)) { VehicleBehaviour.Boost(); }
            if (Input.GetKeyUp(boost)) { VehicleBehaviour.StopBoost(); }
            if (Input.GetKey(oneShotBoost)) { VehicleBehaviour.OneShotBoost(boostLength); }
        }
    }
}