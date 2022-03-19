using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject target;
    //public GameObject this;
    public GameObject bullet;
    private GameObject rotatingPart;
    public float detectionRadius;

    public float shootCooldown;
    private float lastShot;
    private bool hasDetected;
    private bool lastDetection;
    public float maxShotRadius;     // where the bullet will go
    public float firstShotWindup;
    public float shots;

    public float maxAngle;
    private Vector3 defaultDirection;
    private Vector3 leftVector;
    private Vector3 rightVector;
    //private bool rotatingRight;
    //private bool reachedTheEdge;

    private Vector3 startingForward;

    //public float maxHitRadius;      // where the bullet will land and hit the player
    //public int numOfShotDirections;   // the divisions of area that the bullet can go around the player
    

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        defaultDirection = this.transform.forward;
        rotatingPart = transform.parent.gameObject;
        //rotatingRight = true;
        //eachedTheEdge = false;

        startingForward = transform.forward;

        Debug.DrawRay(this.transform.position, defaultDirection, Color.black, 10f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, target.transform.position - this.transform.position, Color.blue, 0.1f);
        //Debug.DrawRay(this.transform.position, defaultDirection, Color.black, 10f);

        CheckDetection();

        //Debug.Log("Detection: " + hasDetected);
        
        

        if(!hasDetected)
        {
            Swivel();
        }
        else
        {
            TryToShoot();
        }

        //Debug.Log("Detection: " + hasDetected);
    }

    public void Swivel()
    {
        // if(rotatingRight)
        // {
        //     if(!reachedTheEdge)
        //     {
        //         rotatingPart.transform.RotateAround(rotatingPart.transform.position, Vector3.up, 20 * Time.deltaTime);

        //         if(rotatingPart.transform.eulerAngles.y > maxAngle)
        //         {
        //             reachedTheEdge = true;
        //         }
        //     }
        //     else
        //     {
        //         rotatingPart.transform.RotateAround(rotatingPart.transform.position, Vector3.up, -20 * Time.deltaTime);

        //         if(rotatingPart.transform.eulerAngles.y < 0.1f)
        //         {
        //             Debug.Log("Got to the middle");
        //             rotatingRight = false;
        //             reachedTheEdge = false;
        //         }
        //     }

            
        // }
        // else if(!rotatingRight)
        // {
        //     if(!reachedTheEdge)
        //     {
        //         Debug.Log("Turning to outside of left");
        //         rotatingPart.transform.RotateAround(rotatingPart.transform.position, Vector3.up, -20 * Time.deltaTime);

        //         if(rotatingPart.transform.eulerAngles.y < 360 - maxAngle)
        //         {
        //             Debug.Log("Reached the left edge");
        //             reachedTheEdge = true;
        //         }
        //     }
        //     else
        //     {
        //         rotatingPart.transform.RotateAround(rotatingPart.transform.position, Vector3.up, 20 * Time.deltaTime);

        //         if(rotatingPart.transform.eulerAngles.y > 359.9f)
        //         {
        //             rotatingRight = false;
        //             reachedTheEdge = false;
        //         }
        //     }
        // }

        // float speed = 20;
        // rotatingPart.transform.RotateAround(rotatingPart.transform.position, Vector3.up, speed * Time.deltaTime);
        // float angle = Vector3.Angle(startingForward, transform.forward) * Mathf.Sign(speed);
        // if (Vector3.Angle(startingForward, transform.forward) * Mathf.Sign(speed) > maxAngle)
        // {
        //     speed *= -1;
        // }
        // print(Mathf.Sign(speed));

        // Debug.DrawRay(this.transform.position, startingForward, Color.black, 0.1f);
        // Debug.DrawRay(this.transform.position, transform.forward, Color.black, 0.1f);

        //Debug.Log(rotatingPart.transform.eulerAngles.y);

        // SOLUTION

        // Vector3 v = targetPosition - bulletOriginPosition;
        // Vector3 v2 = Quaternion.AngleAxis(angleVariance, Vector3.forward) * v;
        // Vector3 v3 = Quaternion.AngleAxis(-angleVariance, Vector3.forward) * v;
        
        // positions[0] = targetPosition;
        // positions[1] = bulletOriginPosition + v2;
        // positions[2] = bulletOriginPosition + v3;
        
        Debug.DrawRay(this.transform.position, defaultDirection, Color.black, 0.1f);
        
    }

    public void CheckDetection()
    {
        // CHECK DETECTION
        RaycastHit hit;
        if(Physics.Raycast(target.transform.position, this.transform.position - target.transform.position, out hit, detectionRadius))
        {
            //Debug.Log(hit.transform.name);

            if(hit.transform == this.transform)
            {
                if(detectionRadius < Vector3.Distance(target.transform.position, this.transform.position))
                {
                    hasDetected = false;
                }
                else
                {
                    hasDetected = true;
                }


                if(hasDetected != lastDetection && hasDetected == true)
                {
                    lastShot = Time.time  + firstShotWindup;
                    
                    //Debug.Log("Dtected for first time");
                }
                
            }
            else
            {
                hasDetected = false;
            }
        }
        
        lastDetection = hasDetected;
    }

    public void TryToShoot()
    {
        if(lastShot + shootCooldown < Time.time && hasDetected)
        { 
            //Debug.DrawRay(this.transform.position, aimingVector - this.transform.position, Color.red, 5f);

            for(int i = 0; i < shots; i++)
            {
                Vector3 aimingVector = target.transform.position + Random.insideUnitSphere * maxShotRadius;
                Bullet lastBullet = Instantiate(bullet, this.transform.position, this.transform.rotation).GetComponent<Bullet>();
                lastBullet.direction = (aimingVector - this.transform.position).normalized;
            }
            

            lastShot = Time.time;
        }
    }

    // void OnDrawGizmos() {
    //     Gizmos.DrawWireSphere(target.transform.position, maxShotRadius);
    // }
}
