using UnityEditor;

namespace e23.VehicleController.Editor
{
    [InitializeOnLoad]
    public static class SetupLauncher
    {
        static SetupLauncher() { ExecuteAlways(); }

        private static void ExecuteAlways()
        {
            AssetDatabase.importPackageCompleted += OpenSetupWindow;
        }

        private static void OpenSetupWindow(string packageName)
        {
            if (packageName.Equals("AVC - Arcade Vehicle Controller"))
            {
                EditorWindow.GetWindow<AVCSetupWindow>(true);
            }
        }
    }
}