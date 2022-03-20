using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float reach = 1;

    private bool interact = false;

    private Camera cam;

    private int kerpAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // LateUpdate to be in accordance with Door stuff
    void LateUpdate()
    {
        interact = false;
        if (Input.GetKeyDown(KeyCode.E)) interact = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, reach)) 
        {
            if (hit.transform.tag == "Door")
            {
                Door door = hit.transform.GetComponentInParent<Door>(); 
                door.onHover(cam.transform);

                if (interact)
                {
                    door.EnterDoor();
                }
            }

            if(hit.transform.name == "Kingpin" && interact)
            {
                Debug.Log("Getting quest");
            }

            if(hit.transform.tag == "Kerp" && interact)
            {
                //Debug.Log("eatin kerp");
                try
                {
                    GameObject kerp = hit.transform.Find("Kerp").gameObject;
                    if(kerp.activeInHierarchy)
                    {
                        //Debug.Log("It's active");
                        Destroy(kerp);
                        kerpAmount++;
                    }
                }
                catch
                {
                    Debug.Log("aint no kerp boi");
                }

                
            }
        }
    }
}
