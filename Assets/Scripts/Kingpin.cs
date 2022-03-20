using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kingpin : MonoBehaviour
{
    private Transform promptText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHover(Transform cam)
    {
        promptText.gameObject.SetActive(true);
        promptText.rotation = cam.rotation;
    }
}
