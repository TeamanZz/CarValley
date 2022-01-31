using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollisionDetection : MonoBehaviour
{
    private CarFallDetector carFallDetector;
    private void OnCollisionEnter(Collision other)
    {
        SceneHandler.Instance.RestartScene();
    }
}
