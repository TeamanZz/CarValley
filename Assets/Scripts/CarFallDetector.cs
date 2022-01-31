using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFallDetector : MonoBehaviour
{

    public static CarFallDetector Instance;
    private void Awake()
    {
        Instance = this;
    }
    public bool canFall;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            canFall = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
            canFall = true;
    }
}