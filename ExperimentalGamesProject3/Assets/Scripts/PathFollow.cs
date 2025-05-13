using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Transform[] path;
    public float speed;
    public float reachDist;
    public float rotationSpeed;
    public int currentPoint = 0;
    private Vector3 startingPoint;

    public Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        startingPoint = transform.position;
        animator = GetComponent<Animator>(); // Get the Animator component
        
        lastPosition = transform.position;
    }

    void Update()
    {
        bool rotate = false;
        Vector3 directionVector;
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            directionVector = path[currentPoint].position - transform.position;
            directionVector.y = 0;
            if (directionVector.magnitude <= reachDist && currentPoint < path.Length - 1)
            {
                currentPoint++;
            }
            if (directionVector.magnitude > reachDist)
            {
                rotate = true;
            }
            if (rotate)
            {
                directionVector = directionVector.normalized;
                movement = directionVector * speed * Time.deltaTime;
                transform.position += movement;
                //transform.forward = directionVector;
                Quaternion rotatingDir = Quaternion.LookRotation(directionVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotatingDir, rotationSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (currentPoint == 0)
            {
                directionVector = startingPoint - transform.position;
            }
            else
            {
                directionVector = path[currentPoint - 1].position - transform.position;
            }
            directionVector.y = 0;
            if (directionVector.magnitude <= reachDist && currentPoint > 0)
            {
                currentPoint--;
            }
            if (directionVector.magnitude >= reachDist)
            {
                rotate = true;
            }
            if (rotate)
            {
                directionVector = directionVector.normalized;
                movement = directionVector * speed * Time.deltaTime;
                transform.position += movement;
                //transform.forward = directionVector;
                Quaternion rotatingDir = Quaternion.LookRotation(directionVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotatingDir, rotationSpeed * Time.deltaTime);
            }
        }
        float actualSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        animator.SetFloat("Speed", actualSpeed); 
        lastPosition = transform.position;
    }
    
}
