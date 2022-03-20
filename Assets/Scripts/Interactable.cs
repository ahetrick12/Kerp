using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public bool clicked = false;

    private Transform promptText;   

    void Start()
    {
        promptText = transform.Find("Canvas").Find("Prompt");
    }

    void Update()
    {
        promptText.gameObject.SetActive(false);
    }

    // public void onHover(Transform cam)
    // {
    //     promptText.gameObject.SetActive(true);
    //     promptText.rotation = cam.rotation;
    // }
}
