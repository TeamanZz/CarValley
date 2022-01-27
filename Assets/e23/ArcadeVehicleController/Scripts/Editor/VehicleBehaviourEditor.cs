using UnityEngine;
using UnityEditor;

namespace e23.VehicleController.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(VehicleBehaviour))]
    public class VehicleBehaviourEditor : UnityEditor.Editor
    {
        private SerializedProperty vehicleModel;            // transform for the model parent
        private SerializedProperty physicsSphere;           // rigidbody for the sphere that moves the vehicle
        private SerializedProperty vehicleBody;             // transform for the model of the vehicle body
        private SerializedProperty vehicleType;             // how many wheels
        private SerializedProperty frontLeftWheel;          // wheel transform
        private SerializedProperty frontRightWheel;         // wheel transform
        private SerializedProperty backLeftWheel;           // wheel transform
        private SerializedProperty backRightWheel;          // wheel transform

        private SerializedProperty vehicleSettings;         // ScriptableObject with the car settings data

        private void OnEnable()
        {
            vehicleType = serializedObject.FindProperty("vehicleType");
            vehicleModel = serializedObject.FindProperty("vehicleModel");
            physicsSphere = serializedObject.FindProperty("physicsSphere");
            vehicleBody = serializedObject.FindProperty("vehicleBody");
            frontLeftWheel = serializedObject.FindProperty("frontLeftWheel");
            frontRightWheel = serializedObject.FindProperty("frontRightWheel");
            backLeftWheel = serializedObject.FindProperty("backLeftWheel");
            backRightWheel = serializedObject.FindProperty("backRightWheel");

            vehicleSettings = serializedObject.FindProperty("vehicleSettings");
        }

        public override void OnInspectorGUI()
        {
            VehicleBehaviour vehicleBehaviour = (VehicleBehaviour)target;

            this.serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(vehicleModel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(physicsSphere);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(vehicleBody);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(vehicleType);
            vehicleBehaviour.VehicleWheelCount = (VehicleType)vehicleType.enumValueIndex;
            EditorGUILayout.EndHorizontal();

            if (vehicleType.enumValueIndex == 0)
            {
                DisplayTwoWheels();
                DisplayFourWheels();
            }
            else if (vehicleType.enumValueIndex == 1)
            {
                DisplayTwoWheels();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(vehicleSettings);
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Update Vehicle Settings", GUILayout.MinHeight(100), GUILayout.Height(50)))
            {
                vehicleBehaviour.SetVehicleSettings();
            }
        }

        private void DisplayTwoWheels()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(frontLeftWheel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(backLeftWheel);
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayFourWheels()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(frontRightWheel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(backRightWheel);
            EditorGUILayout.EndHorizontal();
        }
    }
}