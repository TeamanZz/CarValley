using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace e23.VehicleController.Editor
{
    public class VehicleBuilderSettings : ScriptableObject
    {
        [HideInInspector] [SerializeField] string vehicleName;
        [HideInInspector] [SerializeField] GameObject vehicleModel;
        [HideInInspector] [SerializeField] VehicleType vehicleType;
        [HideInInspector] [SerializeField] PhysicMaterial physicsMaterial;
        [HideInInspector] [SerializeField] string bodyName;
        [HideInInspector] [SerializeField] string frontLeftWheelName;
        [HideInInspector] [SerializeField] string frontRightWheelName;
        [HideInInspector] [SerializeField] string backLeftWheelName;
        [HideInInspector] [SerializeField] string backRightWheelName;
        [HideInInspector] [SerializeField] VehicleBehaviourSettings vehicleSettings;
        [HideInInspector] [SerializeField] bool addEffectsComponent;
        [HideInInspector] [SerializeField] GameObject smokeParticleSystemPrefab;
        [HideInInspector] [SerializeField] int smokeCount;
        [HideInInspector] [SerializeField] GameObject trailRendererPrefab;
        [HideInInspector] [SerializeField] int trailCount;
        [HideInInspector] [SerializeField] bool addExampleInput;
        [HideInInspector] [SerializeField] List<MonoScript> monoBehaviours;

        public string VehicleName { get { return vehicleName; } set { vehicleName = value; } }
        public GameObject VehicleModel { get { return vehicleModel; } set { vehicleModel = value; } }
        public VehicleType VehicleType { get { return vehicleType; } set { vehicleType = value; } }
        public PhysicMaterial PhysicsMaterial { get { return physicsMaterial; } set { physicsMaterial = value; } }
        public string BodyName { get { return bodyName; } set { bodyName = value; } }
        public string FrontLeftWheelName { get { return frontLeftWheelName; } set { frontLeftWheelName = value; } }
        public string FrontRightWheelName { get { return frontRightWheelName; } set { frontRightWheelName = value; } }
        public string BackLeftWheelName { get { return backLeftWheelName; } set { backLeftWheelName = value; } }
        public string BackRightWheelName { get { return backRightWheelName; } set { backRightWheelName = value; } }
        public VehicleBehaviourSettings VechicleSettings { get { return vehicleSettings; } set { vehicleSettings = value; } }
        public bool AddEffectsComponent { get { return addEffectsComponent; } set { addEffectsComponent = value; } }
        public GameObject SmokeParticleSystemPrefab { get { return smokeParticleSystemPrefab; } set { smokeParticleSystemPrefab = value; } }
        public int SmokeCount { get { return smokeCount; } set { smokeCount = value; } }
        public GameObject TrailRendererPrefab { get { return trailRendererPrefab; } set { trailRendererPrefab = value; } }
        public int TrailCount { get { return trailCount; } set { trailCount = value; } }
        public bool AddExampleInput { get { return addExampleInput; } set { addExampleInput = value; } }
        public List<MonoScript> MonoBehaviours { get { return monoBehaviours; } set { monoBehaviours = value; } }
    }
}