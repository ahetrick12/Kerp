using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // mouse vars
    public float sensitivity;
    //private float horizontalSens; // sensitivity
    //private float verticalSens; // sensitivity
    private float xRot = 0f;
    private float yRot = 0f;
    private Camera cam;

    // position vars
    public float height;
    public float walkSpeed;
    public float crouchSpeed;
    public float runSpeed;
    private float speed;
    private bool isFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLookRotation();

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if(!isFlying)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, Vector3.down, out hit);

            this.transform.position = new Vector3(hit.point.x, hit.point.y + height, hit.point.z);

            Vector3 forwardVector = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;

            if(Input.GetKey(KeyCode.LeftShift))
                speed = runSpeed;
            else if(Input.GetKey(KeyCode.LeftControl))
                speed = crouchSpeed;
            else
                speed = walkSpeed;

            if(Input.GetKey(KeyCode.W))
            {
                //Debug.DrawRay(this.transform.position, movementVector, Color.green, 0.1f);
                this.transform.Translate(forwardVector * speed);
            }

            if(Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(-forwardVector * speed);
            }

            if(Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(rightVector * speed);
            }

            if(Input.GetKey(KeyCode.A))
            {
                this.transform.Translate(-rightVector * speed);
            }
            
        }
    }

    private void UpdateLookRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        cam.transform.eulerAngles = new Vector3(xRot, yRot, 0.0f);
    }
}
