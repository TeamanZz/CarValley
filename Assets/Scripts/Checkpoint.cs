using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckPointCollisionHandler checkPointCollisionHandler;
    private bool canCollide = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CheckPointCollisionHandler>(out checkPointCollisionHandler) && canCollide)
        {
            canCollide = false;
            LevelProgress.Instance.AddValueToBar();
        }
    }
}