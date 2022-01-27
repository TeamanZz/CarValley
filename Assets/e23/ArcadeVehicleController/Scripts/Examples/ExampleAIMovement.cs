using UnityEngine;

namespace e23.VehicleController.Examples
{
    public class ExampleAIMovement : MonoBehaviour
    {
        [SerializeField] private ExampleWaypath waypath;
        [SerializeField] private bool canDrive = false;
        [SerializeField] private float minimalDistanceToNextWaypoint = 0.05f;

        [Header("Sensors")]
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float sensorLength = 5.0f;
        [SerializeField] private float frontSensorStartPoint = 1.0f;
        [SerializeField] private float SensorHeight = 0.25f;
        [SerializeField] private float rearSensorStartPoint = 5.0f;
        [SerializeField] private float sideSensorEndOffset = 30.0f;

        private VehicleBehaviour vehicleBehaviour;
        private int targetWaypointIndex = 0;
        private Vector3 targetWaypoint;
        private int newDirection;
        private bool brake = false;

        private void Start() 
        {
            GetRequiredComponents();
            GetFirstWaypoint();
            SetLayerMask();
        }

        private void GetRequiredComponents()
        {
            vehicleBehaviour = GetComponent<VehicleBehaviour>();

            if (waypath == null)
            {
                waypath = FindObjectOfType<ExampleWaypath>();
            }
        }
        private void SetLayerMask()
        {
            layerMask = 1 << 0;
        }

        private void Update() 
        {
            if (canDrive == true)
            {
                EvaluateNextWaypoint();
                Sensors();
                SteerToWaypoint();
            }
        }

        private void Drive(bool forward)
        {
            if (forward) { vehicleBehaviour.ControlAcceleration(); }
            else { vehicleBehaviour.ControlBrake(); }
        }

        private void SteerVehicle(int direction)
        {
            vehicleBehaviour.ControlTurning(direction);
        }

        private void GetFirstWaypoint()
        {
            targetWaypoint = waypath.PathNode(targetWaypointIndex).position;
        }

        private void SteerToWaypoint()
        {
            Vector3 targetVector = targetWaypoint - transform.position;
            targetVector.y = transform.localPosition.y;
            
            Vector3 transformForwardPlane = transform.forward;
            transformForwardPlane.y = transform.localPosition.y;
            
            Vector3 cross = Vector3.Cross(transformForwardPlane, targetVector);
            
            newDirection = cross.y >= 0 ? 1 : -1;

            SteerVehicle(newDirection);
        }

        private void EvaluateNextWaypoint() {
            var distanceToWaypoint = DistanceToWaypoint();
            
            if (distanceToWaypoint < minimalDistanceToNextWaypoint) 
            {
                targetWaypointIndex++;

                if (targetWaypointIndex == waypath.PathNodeCount) 
                {
                    targetWaypointIndex = 0;
                }

                targetWaypoint = waypath.PathNode(targetWaypointIndex).position;
            }
        }

        private float DistanceToWaypoint()
        {
            Vector2 target = new Vector2(targetWaypoint.x, targetWaypoint.z);
            Vector2 position = new Vector2(transform.position.x, transform.position.z);

            return Vector2.Distance(target, position);
        }

        private void Sensors()
        {
            ForwardDrivingSensor();
            SideSensors();
        }

        private void ForwardDrivingSensor()
        {
            RaycastHit hit;

            Vector3 posFrontMid;
            Vector3 posFrontMidEnd;

            posFrontMid = transform.position + transform.forward * frontSensorStartPoint;
            posFrontMid.y = transform.position.y + SensorHeight;
            posFrontMidEnd = transform.position + (transform.forward * frontSensorStartPoint) * sensorLength;
            posFrontMidEnd.y = transform.position.y + SensorHeight;
            Debug.DrawLine(posFrontMid, posFrontMidEnd, Color.red);

            Physics.Raycast(posFrontMid, transform.forward, out hit, sensorLength, layerMask);
            if (hit.collider == null)
            {
                Drive(true);
            }
            else
            {
                Drive(false);
            }
        }

        private void SideSensors()
        {
            RaycastHit hit;

            Vector3 posSide;
            Vector3 posSideEnd;

            posSide = GetSideSensorStart(false);
            posSideEnd = GetSideSensorEnd(frontSensorStartPoint, sideSensorEndOffset, sensorLength/2, false);
            Debug.DrawLine(posSide, posSideEnd, Color.red);

            Physics.Raycast(posSide, transform.forward, out hit, sensorLength);
            if (hit.collider != null)
            {
                SteerVehicle(1);
            }

            posSide = GetSideSensorStart(true);
            posSideEnd = GetSideSensorEnd(frontSensorStartPoint, sideSensorEndOffset, sensorLength/2, true);
            Debug.DrawLine(posSide, posSideEnd, Color.red);

            Physics.Raycast(posSide, transform.forward, out hit, sensorLength);
            if (hit.collider != null)
            {
                SteerVehicle(-1);
            }
        }

        private Vector3 GetSideSensorStart(bool negative)
        {
            Vector3 sensor;
            Vector3 direction = GetRightDirection(negative);
            sensor = transform.position + direction * 0.5f;
            sensor.y = transform.position.y + SensorHeight;

            return sensor;
        }

        private Vector3 GetSideSensorEnd(float startOffset, float endOffset, float sensorLength, bool negative)
        {
            Vector3 sensor;
            Vector3 direction = GetRightDirection(negative);
            sensor = transform.position + (direction * endOffset) + (transform.forward * startOffset) * sensorLength/2;
            sensor.y = transform.position.y + SensorHeight;

            return sensor;
        }

        private Vector3 GetRightDirection(bool negative)
        {
            return negative == true ? -transform.right : transform.right;
        }
    }
}