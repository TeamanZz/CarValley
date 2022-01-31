using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
