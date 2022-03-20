using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool returnToHubOverride = false;
    public LevelManager.LevelType levelType;

    // private Transform promptText;

    private LevelManager levelManager;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.instance;
        // promptText = transform.Find("Canvas").Find("Prompt");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterDoor()
    {
        if (returnToHubOverride)
        {
            levelManager.ReturnToCity(true);
        }
        else
        {
            levelManager.EnterLevel(levelType, transform);
        }
    }
}
