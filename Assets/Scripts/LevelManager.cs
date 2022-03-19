using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum LevelType {Easy, Medium, Hard};

    public string[] easyLevelNames, mediumLevelNames, hardLevelNames;

    public void EnterLevel(LevelType type)
    {
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
}
