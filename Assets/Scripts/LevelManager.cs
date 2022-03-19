using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static bool inLevel = false;

    public enum LevelType {Easy, Medium, Hard};

    public string[] easyLevelNames, mediumLevelNames, hardLevelNames;
    public string hubSceneName = "Alex";

    private Transform player;
    private Vector3 lastPos;
    private Quaternion lastRot;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        } 

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    public void EnterLevel(LevelType type)
    {
        inLevel = true;
        lastPos = player.position;
        lastRot = player.GetChild(0).rotation;

        print ("Entered a level of type " + type);
        switch (type)
        {
            case LevelType.Easy: 
                SceneManager.LoadScene(easyLevelNames[Random.Range(0, easyLevelNames.Length)]);
                break;
            case LevelType.Medium: 
                SceneManager.LoadScene(mediumLevelNames[Random.Range(0, mediumLevelNames.Length)]);
                break;
            case LevelType.Hard: 
                SceneManager.LoadScene(hardLevelNames[Random.Range(0, hardLevelNames.Length)]);
                break;
        }            
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("Alex");
        inLevel = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!inLevel)
        {
            player = FindObjectOfType<PlayerMovement>().transform;
            
            player.position = lastPos;
            player.GetChild(0).rotation = lastRot;
        }
    }
}
