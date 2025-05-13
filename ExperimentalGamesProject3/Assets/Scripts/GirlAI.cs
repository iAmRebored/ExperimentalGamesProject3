using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : MonoBehaviour
{
    public Transform[] path;
    public float speed;
    public float reachDist;
    public int currentPoint = 0;
    private bool move = false;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (move)
        {
            Vector3 directionVector = path[currentPoint].position - transform.position;
            directionVector.y = 0;

            if (directionVector.magnitude <= reachDist && currentPoint < path.Length - 1)
            {
                currentPoint++;
                move = false;
                animator.SetBool("isWalking", false); 
                return;
            }


            directionVector = directionVector.normalized;
            transform.position += directionVector * Time.deltaTime * speed;


            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void Triggered()
    {
        move = true;
    }
}