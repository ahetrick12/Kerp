using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public Vector3 direction;
    public float collisionDistance;
    private Vector3 spawn;
    public float maxDistance;
    public float distanceToDamage;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spawn = this.transform.position;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;

        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, direction, out hit, collisionDistance))
        {
            Destroy(this.gameObject);
            //Debug.Log("I hit something");
        }

        if(Vector3.Distance(this.transform.position, spawn) > maxDistance)
        {
            Destroy(this.gameObject);
        }

        //Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position, Color.black, 0.01f);
        //Debug.DrawRay(player.transform.position, Vector3.up, Color.black, 0.01f);
        if((player.transform.position - this.transform.position).magnitude < distanceToDamage)
        {
            Destroy(this.gameObject);
            //Debug.Log("Hit player");
        }
    }
}
