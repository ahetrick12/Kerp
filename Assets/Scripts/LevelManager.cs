using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static bool inLevel = false;
    public static int kerpCount = 0;


    public enum LevelType {Easy, Medium, Hard};

    public string[] easyLevelNames, mediumLevelNames, hardLevelNames;
    public string hubSceneName = "Alex";

    private Transform player;
    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform lastDoor;
    private int lastKerpAmount;
    
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

    public void EnterLevel(LevelType type, Transform door)
    {
        inLevel = true;
        lastPos = player.position;
        lastRot = player.GetChild(0).rotation;
        lastDoor = door;
        lastKerpAmount = kerpCount;

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

    public void ReturnToCity()
    {
        SceneManager.LoadScene("Alex");
        inLevel = false;
        if (kerpCount > lastKerpAmount)
        {
            lastDoor.GetComponent<Interactable>().clicked = true;
        }
    }

    public void SetLastPosRot(Vector3 pos, Quaternion rot)
    {
        lastPos = pos;
        lastRot = rot;
    }

    public void IncrementKerpCount()
    {
        kerpCount++;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!inLevel)
        {
            player = FindObjectOfType<PlayerMovement>().transform;
            
            player.position = lastPos;
            player.GetChild(0).rotation = lastRot;
        }
    }
}
