using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsTool : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("PlayerPrefs/Clear Player Prefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
