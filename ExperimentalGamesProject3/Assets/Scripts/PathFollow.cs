using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    Debug.Log("huh");
    Vector3 movement = Vector3.zero;

    if (Input.GetKey(KeyCode.W))
    {
        Vector3 directionVector = path[currentPoint].position - transform.position;
        directionVector.y = 0;
        if (directionVector.magnitude <= reachDist && currentPoint < path.Length - 1)
        {
            currentPoint++;
        }
        directionVector = directionVector.normalized;
        movement = directionVector * speed * Time.deltaTime;
        transform.position += movement;
    }

    if (Input.GetKey(KeyCode.S))
    {
        Vector3 directionVector;
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
        directionVector = directionVector.normalized;
        movement = directionVector * speed * Time.deltaTime;
        transform.position += movement;
         Debug.Log(currentPoint);
        
    }

    if(currentPoint >= path.Length-1){      

             SceneManager.LoadScene("EndScene");
        }

    // Rotate to face movement direction
    if (movement.sqrMagnitude > 0.0001f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    float actualSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
    animator.SetFloat("Speed", actualSpeed);
    lastPosition = transform.position;
}

    
}
