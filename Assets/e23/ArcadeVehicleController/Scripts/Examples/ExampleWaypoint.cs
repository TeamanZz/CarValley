using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace e23.VehicleController.Examples
{
    public class ExampleWaypoint : MonoBehaviour
    {
        void OnDrawGizmos () 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}