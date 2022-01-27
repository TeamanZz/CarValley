using e23.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace e23.VehicleController.Editor
{
    public class FixWheelEditorWindow : EditorWindow
    {
        public GameObject[] wheelParents = { };
        
        private Vector2 scrollPos;
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;
        private string helpText;

        [MenuItem("Tools/e23/Wheel Fixer")]
        private static void Init()
        {
#pragma warning disable 0219
            FixWheelEditorWindow window = (FixWheelEditorWindow)GetWindow(typeof(FixWheelEditorWindow));
#pragma warning restore 0219
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            serializedProperty = serializedObject.FindProperty("wheelParents");
            SetHelpText();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 75f));

            EditorBoilerPlate.CreateLabelField("Wheel Fixer", true);
            EditorGUILayout.HelpBox(helpText, MessageType.None);

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            

            EditorGUILayout.Space();

            if (GUILayout.Button("Fix Wheel(s)", GUILayout.MinHeight(100), GUILayout.Height(50)))
            {
                foreach (var wheelParent in wheelParents)
                {
                    FixWheel(wheelParent.transform);
                    EditorSceneManager.MarkSceneDirty(wheelParent.gameObject.scene);
                }
            }

            EditorGUILayout.EndScrollView();           
        }

        private void FixWheel(Transform wheel)
        {
            Renderer renderer = wheel.GetComponentInChildren<Renderer>();
            Bounds wheelBounds = renderer.bounds;

            Transform cachedParent = renderer.transform.parent;
            renderer.transform.SetParent(null);
            wheel.position = wheelBounds.center;
            renderer.transform.SetParent(cachedParent);

            Debug.Log($"{wheel.name} position updated to center of {renderer.name}");
        }

        private void SetHelpText()
        {
            helpText = "If your the wheels of your vehicle have outrageous, weird, or strange rotations, use this editor window to fix the positions of your vehicles wheel pivots. \n\n" +
                "Simply drag in the parent GameObject of your wheel and press the Fix button.";
        }
    }
}