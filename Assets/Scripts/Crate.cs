using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public Rigidbody crateRigidbody;

    private void Awake()
    {
        crateRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // SetCrateBehaivor();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            transform.parent = null;
            transform.localScale = new Vector3(1.3f, 0.15f, 1);
        }
    }

    private void SetCrateBehaivor()
    {
        if (Mathf.Abs(carRigidbody.velocity.x) + Mathf.Abs(carRigidbody.velocity.z) >= 6.8f)
        {
            crateRigidbody.constraints = RigidbodyConstraints.None;
        }
        else
            crateRigidbody.constraints = RigidbodyConstraints.FreezePosition;
    }
}