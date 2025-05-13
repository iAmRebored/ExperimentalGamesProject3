using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public Transform player, target;
    float mouseX, mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 25);

        
        if (Input.GetMouseButton(0)) // 0 = left mouse button
        {
            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            transform.LookAt(target);
        }
        //target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        //transform.LookAt(target);
        //player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
