using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public LevelManager.LevelType levelType;

    private Transform cam;
    private Transform promptText;

    private LevelManager levelManager;

    void Awake()
    {
        cam = Camera.main.transform;
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void EnterDoor()
    {
        levelManager.EnterLevel(levelType);
    }
}
