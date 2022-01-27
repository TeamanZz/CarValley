using UnityEditor;
using UnityEngine;

namespace e23.Editor
{
    public static class EditorBoilerPlate
    {
        public static void DrawSeparatorLine()
        {
            EditorGUILayout.Space();
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        public static void CreateLabelField(string label, bool bold = false)
        {
            GUIStyle editorStyles = bold ? EditorStyles.boldLabel : EditorStyles.label;
            EditorGUILayout.LabelField(label, editorStyles);
        }
    }
}