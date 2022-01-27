using UnityEngine;
using UnityEditor;

namespace e23.VehicleController.Editor
{
    public class AVCSetupWindow : EditorWindow
    {
        private static AVCSetupWindow instance;
        private static SetupSettings Settings { get { return SetupSettings.Instance; } }

        public static AVCSetupWindow Instance {
            get {
                if (instance != null)
                { return instance; }

                instance = Window;
                if (instance != null)
                { return instance; }

                instance = GetWindow<AVCSetupWindow>(true, Settings.windowTitle);
                return instance;
            }
        }

        private static AVCSetupWindow Window {
            get {
                AVCSetupWindow[] windows = Resources.FindObjectsOfTypeAll<AVCSetupWindow>();
                return windows.Length > 0 ? windows[0] : null;
            }
        }

        private static GUIStyle
            s_button,
            s_titleWhite,
            s_commentWhite,
            s_commentRed,
            s_commentRedBold,
            s_commentOrange,
            s_commentGreen,
            s_windowBackground;

        private bool layerCorrectlyAdded = false;
        private bool layerAlreadyExists = false;

        [MenuItem("Tools/e23/AVC Setup", false, 0)]
        public static void Open()
        {
            GetWindow<AVCSetupWindow>(true, Settings.windowTitle);
            Instance.InitWindow();
        }

        private void OnEnable()
        {
            InitWindow();
            EditorUtility.ClearProgressBar();
        }

        private void InitWindow()
        {
            titleContent = new GUIContent(Settings.windowTitle);
            wantsMouseMove = true;
            minSize = new Vector2(508, 572);
            maxSize = minSize;

            LoadStyles();
        }

        private static void LoadStyles()
        {
            if (Settings.Skin == null)
            {
                Settings.Skin = SetupSettings.GetScriptableObject<GUISkin>("AVCSetupSkin", SetupSettings.ResourcesPath);
            }

            s_button = Settings.Skin.GetStyle("button");
            s_titleWhite = Settings.Skin.GetStyle("TitleWhite");
            s_commentWhite = Settings.Skin.GetStyle("CommentWhite");
            s_commentRed = Settings.Skin.GetStyle("CommentRed");
            s_commentRedBold = Settings.Skin.GetStyle("CommentRedBold");
            s_commentOrange = Settings.Skin.GetStyle("CommentOrange");
            s_commentGreen = Settings.Skin.GetStyle("CommentGreen");
            s_windowBackground = Settings.Skin.GetStyle("WindowBackground");
        }

        private void OnInspectorUpdate() { Repaint(); }

        private void OnGUI()
        {
            GUI.Label(new Rect(-2, 0, s_windowBackground.fixedWidth, s_windowBackground.fixedHeight), s_windowBackground.normal.background);
            GUILayout.BeginArea(new Rect(0, 272, position.width, position.height - 272));
            {
                DrawContent();
            }
            GUILayout.EndArea();
        }

        private void DrawContent()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Settings.windowTitle, s_titleWhite);
                GUILayout.Space(15);
                
                Displayessage();
                GUILayout.Space(10);
                if (layerAlreadyExists)
                {
                    GUILayout.Label("Layer 9 already in use, please see 'Setting up layers' (pg 3) in the documentation.", s_commentRedBold, GUILayout.ExpandWidth(true));
                }
                else if (layerCorrectlyAdded)
                {
                    GUILayout.Label("Layer added successfully", s_commentGreen, GUILayout.ExpandWidth(true));
                }
                else
                {
                    DisplayButton();
                }
            }
            GUILayout.EndVertical();
        }

        private void Displayessage()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(25);
                GUILayout.BeginVertical();
                {
                    GUILayout.Label(Settings.setupMessage_1, s_commentWhite, GUILayout.ExpandWidth(true));
                    GUILayout.Label(Settings.setupMessage_2, s_commentWhite, GUILayout.ExpandWidth(true));
                    GUILayout.Space(25);
                    GUILayout.Label(Settings.setupMessage_3, s_commentWhite, GUILayout.ExpandWidth(true));
                    GUILayout.Label(Settings.setupWarning, s_commentRed, GUILayout.ExpandWidth(true));
                    GUILayout.Label(Settings.setupMessage_4, s_commentWhite, GUILayout.ExpandWidth(true));
                    GUILayout.Space(25);
                    GUILayout.Label(Settings.setupNote, s_commentOrange, GUILayout.ExpandWidth(true));
                }
                GUILayout.EndVertical();
                GUILayout.Space(25);
            }
            GUILayout.EndHorizontal();
        }

        private void DisplayButton()
        {
            if (GUILayout.Button("Add Layer", s_button, GUILayout.Height(50)))
            {
                AddLayer();
            }
        }

        private void AddLayer()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layers = tagManager.FindProperty("layers");
            if (layers == null || !layers.isArray)
            {
                Debug.LogWarning("Can't set up the layers. It's possible the format of the layers and tags data has changed in this version of Unity.");
                Debug.LogWarning("Layers is null: " + (layers == null));
                return;
            }

            SerializedProperty layerSP = layers.GetArrayElementAtIndex(9);
            if (string.IsNullOrEmpty(layerSP.stringValue))
            {
                layerSP.stringValue = "Ground";
                layerCorrectlyAdded = true;
            }
            else
            {
                layerAlreadyExists = true;
            }
            
            tagManager.ApplyModifiedProperties();
        }
    }
}