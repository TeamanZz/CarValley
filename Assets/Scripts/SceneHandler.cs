using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    public List<GameObject> tracksList = new List<GameObject>();

    public int currentLevelindex;

    private void Awake()
    {
        Instance = this;
        tracksList[currentLevelindex].SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}