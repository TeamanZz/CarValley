using UnityEditor;
using UnityEngine;

namespace e23.VehicleController.Editor
{
    //[CreateAssetMenu(fileName = nameof(SetupSettings), menuName = "e23/AVC/Setup Settings", order = 9)]
    public class SetupSettings : ScriptableObject
    {
        private const string FILE_NAME = "AVCSetupSettings";
        public static string ResourcesPath { get { return "Assets/e23/ArcadeVehicleController/Scripts/Editor/Resources"; } }

        private static SetupSettings instance;

        public static SetupSettings Instance {
            get {
                if (instance != null)
                { return instance; }

                instance = GetScriptableObject<SetupSettings>(FILE_NAME, ResourcesPath);
                return instance;
            }
        }

        public string windowTitle = "Arcade Vehicle Controller";
        public GUISkin Skin;
        public string setupMessage_1;
        public string setupMessage_2;
        public string setupMessage_3;
        public string setupMessage_4;
        public string setupWarning;
        public string setupNote;

        public static T GetScriptableObject<T>(string fileName, string resourcesPath) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(resourcesPath))
            { return null; }
            if (string.IsNullOrEmpty(fileName))
            { return null; }

            if (!resourcesPath[resourcesPath.Length - 1].Equals(@"\"))
            { resourcesPath += @"\"; }

            var obj = (T)Resources.Load(fileName, typeof(T));
#if UNITY_EDITOR
            if (obj != null) return obj;
            obj = CreateAsset<T>(resourcesPath, fileName);
#endif
            return obj;
        }

        private static T CreateAsset<T>(string relativePath, string fileName, string extension = ".asset")
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(relativePath))
            { return null; }
            if (string.IsNullOrEmpty(fileName))
            { return null; }

            if (!relativePath[relativePath.Length - 1].Equals(@"\"))
            { relativePath += @"\"; }

            var asset = CreateInstance<T>();
            if (!AssetDatabase.IsValidFolder(relativePath)) 
            { return null; }

            AssetDatabase.CreateAsset(asset, relativePath + fileName + extension);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            
            return asset;
        }

        public void SetDirty(bool saveAssets)
        {
            EditorUtility.SetDirty(this);
            if (saveAssets) { AssetDatabase.SaveAssets(); }
        }
    }
}