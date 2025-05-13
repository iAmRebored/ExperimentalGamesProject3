using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : MonoBehaviour
{
    public Transform[] path;
    public float speed;
    public float reachDist;
    public float rotationSpeed;
    public int currentPoint = 0;
    private bool move = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            Vector3 directionVector = path[currentPoint].position - transform.position;
            directionVector.y = 0;
            if (directionVector.magnitude <= reachDist && currentPoint < path.Length - 1)
            {
                currentPoint++;
            }
            if (directionVector.magnitude <= reachDist )
            {
                move = false;
            }
            directionVector = directionVector.normalized;
            transform.position += directionVector * Time.deltaTime * speed;
            //transform.forward = directionVector;
            //Quaternion rotatingDir = Quaternion.LookRotation(directionVector, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotatingDir, rotationSpeed * Time.deltaTime);
        }
    }

    public void Triggered()
    {
        move = true;
    }
}
