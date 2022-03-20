using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public GameObject canvas;
    public GameObject bullet;
    private GameObject rotatingPart;
    private GameObject spotlight;
    public float detectionRange;
    [Range(0,90)]
    public float detectionAngle;
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

    public Texture redCrosshair;
    public Texture normalCrosshair;
    
    private Transform player;
    private Vector3 lockSmoothTime;

    //public float maxHitRadius;      // where the bullet will land and hit the player
    //public int numOfShotDirections;   // the divisions of area that the bullet can go around the player
    

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        rotatingPart = transform.parent.gameObject;         // rotating part is the parent (turret head)
        defaultDirection = rotatingPart.transform.forward;  // set the base direction to whatever it was in the editor

        //spotlight = rotatingPart.transform.Find("Spotlight").gameObject;
        spotlight = transform.parent.GetComponentInChildren<Light>().gameObject;

        // get the bounds for the turret rotation
        rightEdge = Quaternion.AngleAxis(maxAngle, Vector3.up) * defaultDirection;
        leftEdge = Quaternion.AngleAxis(-maxAngle, Vector3.up) * defaultDirection;
        rotatingPart.transform.forward = leftEdge;  // start facing left

        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float xzScale = detectionRange * Mathf.Tan(detectionAngle * Mathf.Deg2Rad); 
        spotlight.GetComponent<Light>().range = detectionRange;
        spotlight.GetComponent<Light>().spotAngle = detectionAngle;

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
        Vector3 target =  rotatingPart.transform.forward;
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
                target = Vector3.Lerp(leftEdge.normalized, rightEdge.normalized, ((Time.time - lastRotate) / (timeToRotate)));
            }
        }
        
        if (!turningRight)    // ditto ^^
        {
            if(rotatingPart.transform.forward == leftEdge)
            {
                turningRight = true;
                lastRotate = Time.time;
            }
            else
            {
                target = Vector3.Lerp(rightEdge.normalized, leftEdge.normalized, ((Time.time - lastRotate) / (timeToRotate)));
            }
        }

        rotatingPart.transform.forward = Vector3.SmoothDamp(rotatingPart.transform.forward, target, ref lockSmoothTime, turretLockOnTime);
   
    }

    public void CheckDetection()
    {
        // CHECK DETECTION
        RaycastHit hit;
        if(Physics.Raycast(player.position, (this.transform.position - player.position).normalized, out hit, detectionRange))
        {
            // raycast from the player to the turret, if it makes contact with the turret head then check conditions...
            if(hit.transform == this.transform.parent)
            {
                if(InView())
                {
                    hasDetected = true;
                }
                else
                {
                    hasDetected = false;
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

        if(hasDetected)
        {
            canvas.transform.Find("Crosshair").GetComponent<RawImage>().texture = redCrosshair;
        }
        else
        {
            canvas.transform.Find("Crosshair").GetComponent<RawImage>().texture = normalCrosshair;
        }

        //Debug.Log(GameObject.Find("Canvas").transform.Find("Crosshair").GetComponent<RawImage>());


        lastDetection = hasDetected;

    }

    public void TryToShoot()
    {
        
        //check if the palyer is in the turret's field of view and adjust the turret's angle accordingly
        float angle = Vector3.SignedAngle(defaultDirection, player.position - rotatingPart.transform.position, Vector3.up);
        if(Mathf.Abs(angle) > maxAngle + detectionAngle)
        {
            // Out of FOV
            return;
        }

        // rotate the turrent to face the player
        if (InView())
            rotatingPart.transform.forward = Vector3.SmoothDamp(rotatingPart.transform.forward, 
                ((player.transform.position - Vector3.up * 0.1f) - rotatingPart.transform.position).normalized,
                ref lockSmoothTime,
                turretLockOnTime);


        // shoot
        if(lastShot + shootCooldown < Time.time && hasDetected)
        { 

            for(int i = 0; i < shots; i++)
            {
                Vector3 aimingVector = (player.position - Vector3.up * 0.1f) + Random.insideUnitSphere * maxShotRadius;
                Bullet lastBullet = Instantiate(bullet, this.transform.position, Quaternion.Euler(this.transform.eulerAngles + bullet.transform.eulerAngles)).GetComponent<Bullet>();
                lastBullet.direction = (aimingVector - this.transform.position).normalized;

                if(barrels.Length > 0)
                {
                    lastBullet.transform.position = barrels[shotsTaken % barrels.Length].transform.position;
                }
                shotsTaken++;
            }
            

            lastShot = Time.time;
        }

        lastRotate = Time.time;
    }

    private bool InView()
    {
        float angle = Vector3.SignedAngle(transform.forward, (player.position - transform.position).normalized, Vector3.up);
        return (Vector3.Distance(transform.position, player.position) < detectionRange && Mathf.Abs(angle) < detectionAngle);
    }
}
