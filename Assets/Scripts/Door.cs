using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform cam;
    private Transform promptText;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        promptText = transform.GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        promptText.rotation = cam.rotation;
        promptText.gameObject.SetActive(false);
    }

    public void onHover()
    {
        promptText.gameObject.SetActive(true);
    }
}
