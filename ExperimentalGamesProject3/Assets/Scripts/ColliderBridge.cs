using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    ColliderListener listener;
    public void Initialize(ColliderListener l)
    {
        listener = l;
    }
    void OnCollisionEnter(Collision collision)
    {
        listener.OnCollisionEnter(collision);
    }
    void OnTriggerEnter(Collider other)
    {
        listener.OnTriggerEnter(other);
        Destroy(other.gameObject);
    }
}
