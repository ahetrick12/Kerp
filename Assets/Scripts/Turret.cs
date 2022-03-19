using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject target;
    public GameObject eye;
    public float detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(eye.transform.position, target.transform.position - eye.transform.position, Color.blue, 0.1f);

        RaycastHit hit;
        if(Physics.Raycast(target.transform.position, eye.transform.position - target.transform.position, out hit, detectionRadius))
        {
            //Debug.Log(hit.transform.name);

            if(hit.transform == eye.transform)
                Debug.Log("Turret sees player");
        }
    }
}
