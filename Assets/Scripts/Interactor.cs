using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float reach = 5;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // LateUpdate to be in accordance with Door stuff
    void LateUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, reach)) 
        {
            if (hit.transform.tag == "Door")
            {
                hit.transform.GetComponentInParent<Door>().onHover();
            }
        }
    }
}
