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
                Interactable door = hit.transform.GetComponentInParent<Interactable>(); 
                door.onHover(cam.transform);

                if (interact)
                {
                    door.GetComponent<Door>().EnterDoor();
                }
            }

            if(hit.transform.name == "Kingpin")
            {
                Interactable kingpin = hit.transform.GetComponent<Interactable>();
                if (!kingpin.clicked) 
                {
                    kingpin.onHover(cam.transform);
                
                    if (interact)
                    {
                        kingpin.GetComponent<Kingpin>().Talk();
                        //Debug.Log("talked");
                    }
                }
            }

            if(hit.transform.tag == "Kerp")
            {
                Interactable kerpBox = hit.transform.GetComponentInParent<Interactable>();
                if (!kerpBox.clicked) 
                {
                    kerpBox.onHover(cam.transform);
                
                    if (interact)
                    {
                        try
                        {
                            GameObject kerp = hit.transform.parent.Find("Kerp").gameObject;
                            if(kerp.activeInHierarchy)
                            {
                                //Debug.Log("It's active");
                                Destroy(kerp);
                                kerpAmount++;
                                kerpBox.clicked = true;
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
    }
}
