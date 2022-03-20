using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityGeneration : MonoBehaviour
{
    public static CityGeneration instance;

    [System.Serializable]
    public struct Block {
        public GameObject prefab;
        [Range(0,1)]
        public float chance;
    }

    public bool mainMenu = false;

    [Space(20)]

    public GameObject baseplate;
    public Block[] blockPrefabs;
    public GameObject doorPrefab;

    [Space(20)]

    public int blockSize = 10;
    public Vector2 cityDimensions = Vector2.one;
    public float maxRowOffset = 5;
    
    [Space(20)]

    public int levelCount = 5;
    public float minLevelSpawnDistance = 50;

    private float placementOffset;
    private Transform hubBlock;
    private List<Transform> spawnpoints = new List<Transform>();
    private List<Transform> levelDoors;

    private bool assignHubFlag = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!mainMenu)
        {
            // Persist between scenes
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            } 

            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Actual logic
        Vector2 middle = new Vector2((int)(cityDimensions.x / 2), (int)(cityDimensions.y / 2));

        for (int i = 0; i < cityDimensions.x; i++)
        {   
            placementOffset = Random.Range(0, maxRowOffset);

            for (int j = 0; j < cityDimensions.y; j++)
            {
                float xPos = (i - middle.x) * blockSize;
                float zPos = (j - middle.y) * blockSize + placementOffset;
                
                GameObject bp = Instantiate(baseplate, new Vector3(xPos, 0, zPos), Quaternion.Euler(Vector3.right * -90), transform.GetChild(0));
                
                int index;
                float chance, threshold;
                do
                {
                    index = Random.Range(0, blockPrefabs.Length);
                    chance = Random.Range(blockPrefabs[index].chance, 1);
                    threshold = 1 - blockPrefabs[index].chance;
                } while (chance <= threshold);
                
                Transform block = Instantiate(blockPrefabs[index].prefab).transform;

                block.position = new Vector3(xPos, 0, zPos);
                block.eulerAngles = new Vector3(-90, 0, 90 * Random.Range(0, 3));
                block.parent = bp.transform;
                
                // Add each spawnpoint to the list
                foreach(Transform tr in block)
                {
                    if(tr.tag == "Spawnpoint")
                    {
                        spawnpoints.Add(tr);
                    }
                }

                if (i == middle.x && j == middle.y) 
                {
                    assignHubFlag = true;
                }

                if (assignHubFlag && block.childCount != 0)
                {
                    hubBlock = block;
                    assignHubFlag = false;
                }
            }
        }

        if (!mainMenu)
            SpawnDoors();  
    }

    private void SpawnDoors() {
        List<Transform> finalPoints = new List<Transform>();
        List<Transform> viablePoints = new List<Transform>(spawnpoints);

        // Hub door creation
        Transform hubPoint = Instantiate(doorPrefab).transform;
        Transform hubDoorSpawnpoint = hubBlock.GetChild(Random.Range(0, hubBlock.childCount));
        hubPoint.position = hubDoorSpawnpoint.position;
        hubPoint.parent = hubDoorSpawnpoint;

        Vector3 playerPos = hubPoint.position + Vector3.right * -1f;
        FindObjectOfType<LevelManager>().SetLastPosRot(playerPos + Vector3.up * 1, Quaternion.LookRotation(hubPoint.position - playerPos, Vector3.up));
        foreach(Transform point in viablePoints)
        {
            if (point.position == hubPoint.position)
            {
                viablePoints.Remove(point);
                break;
            }
        }

        // Find all final spawnpoints for doors
        while (finalPoints.Count < levelCount)
        {
            Transform potentialPoint = viablePoints[Random.Range(0, viablePoints.Count)];
            if (Vector3.Distance(potentialPoint.position, hubPoint.position) > minLevelSpawnDistance)
            {
                finalPoints.Add(potentialPoint);
            }

            viablePoints.Remove(potentialPoint);
        }

        // Spawn doors at final points
        levelDoors = new List<Transform>();
        for(int i = 0; i < finalPoints.Count; i++)
        {
            Transform door = Instantiate(doorPrefab).transform;
            door.position = finalPoints[i].position;
            door.parent = finalPoints[i];
            door.GetComponentInChildren<Door>().levelType = (LevelManager.LevelType)Random.Range(0, System.Enum.GetValues(typeof(LevelManager.LevelType)).Length);
            levelDoors.Add(door);
        }
    }

    public List<Transform> getLevelDoors() {
        return levelDoors;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.GetChild(0).gameObject.SetActive(LevelManager.inLevel ? false : true);
    }
}
