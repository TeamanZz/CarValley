using System.Collections.Generic;
using UnityEngine;

namespace e23.VehicleController.Examples
{
    public class ExampleWaypath : MonoBehaviour
    {
        [SerializeField] List<Transform> path;
        [SerializeField] Color lineColor = Color.white;

        public int PathNodeCount { get { return path.Count; } }
        public Transform PathNode(int index) { return path[index]; } 

        void OnDrawGizmos () 
        {
            Transform[] pathNodes = transform.GetComponentsInChildren<Transform>();
            path = new List<Transform>();

            for (int i = 0; i < pathNodes.Length; i++) {
                Transform node = pathNodes[i];

                if (node != transform) {
                    path.Add(node);
                }
            }

            for (int i = 0; i < path.Count; i++) {
                Vector3 pos = path[i].position;
                if (i > 0) {
                    Vector3 prevPos = path[i - 1].position;
                    Gizmos.color = lineColor;
                    Gizmos.DrawLine(prevPos, pos);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(pos, 0.3f);
                }
            }
        }
    }
}