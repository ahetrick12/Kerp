using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject target;
    //public GameObject this;
    public GameObject bullet;
    private GameObject rotatingPart;
    private GameObject spotlight;
    public float detectionRadius;
    public GameObject[] barrels;
    private int shotsTaken;

    public float shootCooldown;
    private float lastShot;
    private bool hasDetected;
    private bool lastDetection;
    public float maxShotRadius;     // where the bullet will go
    public float firstShotWindup;
    public float shots;
    private float detectedTime;
    //public float snapAngle;
    public float spotlightAngle;

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
    

    //public float maxHitRadius;      // where the bullet will land and hit the player
    //public int numOfShotDirections;   // the divisions of area that the bullet can go around the player
    

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        rotatingPart = transform.parent.gameObject;         // rotating part is the parent (turret head)
        defaultDirection = rotatingPart.transform.forward;  // set the base direction to whatever it was in the editor

        // set spotlight size
        spotlight = rotatingPart.transform.Find("Spotlight").gameObject;
        spotlight.transform.localScale = new Vector3(360 / spotlightAngle, detectionRadius * 2 + 3, 360 / spotlightAngle);

        // get the bounds for the turret rotation
        rightEdge = Quaternion.AngleAxis(maxAngle, Vector3.up) * defaultDirection;
        leftEdge = Quaternion.AngleAxis(-maxAngle, Vector3.up) * defaultDirection;
        rotatingPart.transform.forward = leftEdge;  // start facing left

        
    }

    // Update is called once per frame
    void Update()
    {

        CheckDetection();     

        if(!hasDetected)
        {
            Swivel();
        }
        else
        {
            TryToShoot();
        }

    }

    public void Swivel()
    {

        if(turningRight)
        {
            // if it's reached the outer bound...
            if(rotatingPart.transform.forward == rightEdge)
            {
                turningRight = false;
                lastRotate = Time.time; // start turning the other direction
            }
            else
            {   // else keep turning
                rotatingPart.transform.forward = Vector3.Lerp(leftEdge.normalized, rightEdge.normalized, ((Time.time - lastRotate) / (timeToRotate)));
            }
        }
        else    // ditto ^^
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
   
    }

    public void CheckDetection()
    {
        // CHECK DETECTION
        RaycastHit hit;
        if(Physics.Raycast(target.transform.position, this.transform.position - target.transform.position, out hit, detectionRadius))
        {

            // raycast from the player to the turret, if it makes contact with the turret then check conditions...
            if(hit.transform == this.transform)
            {
                if(detectionRadius < Vector3.Distance(target.transform.position, this.transform.position))
                {
                    hasDetected = false;
                }
                else if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) > maxAngle)
                {
                    hasDetected = false;
                }
                else if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) < -maxAngle)
                {
                    hasDetected = false;
                }
                else
                {
                    hasDetected = true;
                }

                // if it jsut went from not detecting to detecting...
                if(hasDetected != lastDetection && hasDetected == true)
                {
                    // add on to the shot cooldown for the first shot
                    lastShot = Time.time  + firstShotWindup;
                    
                    detectedTime = Time.time;

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

        Vector3 turretOrientation;
        bool outOfFOV = false;

        // check if the palyer is in the turret's field of view and adjust the turret's angle accordingly
        if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) > maxAngle)
        {
            turretOrientation = rightEdge;
            outOfFOV = true;
        }
        else if(Vector3.SignedAngle(defaultDirection, target.transform.position - rotatingPart.transform.position, Vector3.up) < -maxAngle)
        {
            turretOrientation = leftEdge;
            outOfFOV = true;
        }
        else
        {
            turretOrientation = new Vector3((target.transform.position - rotatingPart.transform.position).x, 0f, (target.transform.position - rotatingPart.transform.position).z);
        }

        // rotate the turrent to face the player
        rotatingPart.transform.forward = Vector3.Lerp(rotatingPart.transform.forward, turretOrientation, (Time.time - detectedTime) / turretLockOnTime);

        //Debug.Log($"Turret lock on: {(((Time.time - detectedTime) / turretLockOnTime) * 100f):F2}%");

        // shoot
        if(lastShot + shootCooldown < Time.time && hasDetected && !outOfFOV)
        { 

            for(int i = 0; i < shots; i++)
            {
                Vector3 aimingVector = target.transform.position + Random.insideUnitSphere * maxShotRadius;
                Bullet lastBullet = Instantiate(bullet, this.transform.position, this.transform.rotation).GetComponent<Bullet>();
                lastBullet.direction = (aimingVector - this.transform.position).normalized;

                if(barrels.Length > 0)
                {
                    lastBullet.transform.position = barrels[shotsTaken % barrels.Length].transform.position;
                }
                shotsTaken++;
            }
            

            lastShot = Time.time;
        }
    }
}
