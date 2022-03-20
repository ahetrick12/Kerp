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
    private float detectedTime;

    public float maxAngle;
    private Vector3 defaultDirection;
    private Vector3 leftVector;
    private Vector3 rightVector;
    //private bool rotatingRight;
    //private bool reachedTheEdge;
    Vector3 leftEdge;
    Vector3 rightEdge;
    public float timeToRotate;
    private float lastRotate;
    private bool turningRight = true;
    public float turretLockOnTime;
    

    private Vector3 startingForward;

    //public float maxHitRadius;      // where the bullet will land and hit the player
    //public int numOfShotDirections;   // the divisions of area that the bullet can go around the player
    

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        rotatingPart = transform.parent.gameObject;
        defaultDirection = rotatingPart.transform.forward;
        //rotatingRight = true;
        //eachedTheEdge = false;
        rightEdge = Quaternion.AngleAxis(maxAngle, Vector3.up) * defaultDirection; //* this.transform.forward;
        //leftEdge += new Vector3(0, -leftEdge.y, -leftEdge.x);
        leftEdge = Quaternion.AngleAxis(-maxAngle, Vector3.up) * defaultDirection; //this.transform.forward;
        rotatingPart.transform.forward = leftEdge;  // start facing left

        startingForward = transform.forward;

        //Debug.DrawRay(this.transform.position, defaultDirection, Color.black, 10f);
        
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

        //Debug.DrawRay(this.transform.position, defaultDirection, Color.red, 0.1f);
        //Debug.DrawRay(rotatingPart.transform.position, leftEdge, Color.yellow, 0.1f);
        //Debug.DrawRay(rotatingPart.transform.position, rightEdge, Color.green, 0.1f);

        if(turningRight)
        {
            if(rotatingPart.transform.forward == rightEdge)
            {
                turningRight = false;
                lastRotate = Time.time;
            }
            else
            {
                rotatingPart.transform.forward = Vector3.Lerp(leftEdge.normalized, rightEdge.normalized, ((Time.time - lastRotate) / (timeToRotate)));
            }
        }
        else
        {
            if(rotatingPart.transform.forward == leftEdge)
            {
                turningRight = true;
                lastRotate = Time.time;
            }
            else
            {
                rotatingPart.transform.forward = Vector3.Lerp(rightEdge.normalized, leftEdge.normalized, ((Time.time - lastRotate) / (timeToRotate)));
            }
        }

        //Debug.Log($"Turning: {(((Time.time - lastRotate) / (lastRotate + timeToRotate)) * 100f):F2}%");
        //Debug.Log("");

        
        //Debug.DrawRay(this.transform.position, defaultDirection, Color.black, 0.1f);
        
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
                    
                    detectedTime = Time.time;
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
        // orient

        Vector3 turretOrient;

        Debug.Log(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up));

        if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) + 180 < maxAngle)
        {
            turretOrient = rightEdge;
        }
        else if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) + 180 > -maxAngle)
        {
            turretOrient = leftEdge;
        }
        else
        {
            turretOrient = new Vector3((target.transform.position - rotatingPart.transform.position).x, 0f, (target.transform.position - rotatingPart.transform.position).z);
        }

        rotatingPart.transform.forward = Vector3.Lerp(rotatingPart.transform.forward, turretOrient, (Time.time - detectedTime) / turretLockOnTime);

        Debug.Log(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up));

        //Debug.Log($"Turret lock on: {(((Time.time - detectedTime) / turretLockOnTime) * 100f):F2}%");

        // shoot
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
