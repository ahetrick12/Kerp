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

    //public float maxHitRadius;      // where the bullet will land and hit the player
    //public int numOfShotDirections;   // the divisions of area that the bullet can go around the player
    

    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;
        defaultDirection = this.transform.eulerAngles;
        rotatingPart = transform.parent.gameObject;

        Debug.DrawRay(this.transform.position, defaultDirection);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, target.transform.position - this.transform.position, Color.blue, 0.1f);

        RaycastHit hit;
        if(Physics.Raycast(target.transform.position, this.transform.position - target.transform.position, out hit, detectionRadius))
        {
            //Debug.Log(hit.transform.name);

            if(hit.transform == this.transform)
            {
                hasDetected = true;
                //Debug.Log("Turret sees player");
                TryToShoot();
            }
            else
            {
                hasDetected = false;
            }
        }

        //Debug.Log("Detection: " + hasDetected);

        if(hasDetected != lastDetection)
        {
            lastShot = Time.time + firstShotWindup;
        }
        lastDetection = hasDetected;

        if(!hasDetected)
        {
            Swivel();
        }
    }

    public void Swivel()
    {

    }

    public void TryToShoot()
    {
        if(lastShot + shootCooldown < Time.time)
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
