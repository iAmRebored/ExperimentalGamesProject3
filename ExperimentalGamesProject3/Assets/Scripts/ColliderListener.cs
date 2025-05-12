using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ColliderListener : MonoBehaviour
{
    public GameObject target;
    void Awake()
    {
        // Check if Colider is in another GameObject
        //Collider2D collider = GetComponentInChildren<Collider2D>();
        Collider collider = target.GetComponent<Collider>();
        if (collider.gameObject != gameObject)
        {
            ColliderBridge cb = collider.gameObject.AddComponent<ColliderBridge>();
            cb.Initialize(this);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        // Do your stuff here
    }
    public void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<GirlAI>().Triggered();
        Destroy(other.gameObject);
        UnityEngine.Debug.Log("Check");
    }
}
