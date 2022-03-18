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
    public float standHeight;
    public float crouchHeight;
    private float height;
    public float walkSpeed;
    public float crouchSpeed;
    public float runSpeed;
    private float speed;
    private bool isFlying = false;
    public float jumpStrength;
    public float gravity;

    private Rigidbody rb;

    private Vector3 velocity;
    //private Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //acceleration = new Vector3(0, gravity, 0);
        height = standHeight;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLookRotation();

        UpdatePosition();
    }

    private void UpdatePosition()
    {

        //velocity = Vector3.zero;
        velocity += new Vector3(0, gravity, 0);

        if(!isFlying)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, Vector3.down, out hit);

            //Debug.Log(this.transform.position.y - hit.point.y + " and height is " + height);
            if(this.transform.position.y - hit.point.y <= height)
            {
                this.transform.position = new Vector3(hit.point.x, hit.point.y + height, hit.point.z);
                //Debug.Log(this.transform.position.y - hit.point.y);
                velocity = new Vector3(velocity.x, 0, velocity.z);
            }
            else
            {
                this.transform.position += new Vector3(0, velocity.y, 0);
            }
            
            
            Vector3 forwardVector = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;

            // Handle special movement cases
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
                height = standHeight;
            }
            else if(Input.GetKey(KeyCode.LeftControl))
            {
                speed = crouchSpeed;
                height = crouchHeight;
            }
            else
            {
                speed = walkSpeed;
                height = standHeight;
            }

            // Handle input for movement

            if(Input.GetKey(KeyCode.W))
            {
                this.transform.position += forwardVector * speed;
            }

            if(Input.GetKey(KeyCode.S))
            {
                this.transform.position += -forwardVector * speed;
            }

            if(Input.GetKey(KeyCode.D))
            {
                this.transform.position += rightVector * speed;
            }

            if(Input.GetKey(KeyCode.A))
            {
                this.transform.position += -rightVector * speed;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                velocity += new Vector3(0, jumpStrength, 0);
            }

            this.transform.Translate(velocity);
            
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
