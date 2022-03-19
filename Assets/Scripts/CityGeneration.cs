using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneration : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public GameObject doorPrefab;

    [Space(20)]

    public int blockSize = 10;
    public Vector2 cityDimensions = Vector2.one;
    public float maxRowOffset = 5;
    
    [Space(20)]

    public float levelCount = 5;
    public float minLevelSpawnDistance = 50;

    private float placementOffset;
    private Transform hubBlock;
    private List<Transform> spawnpoints = new List<Transform>();

    private bool assignHubFlag = false;

    // Start is called before the first frame update
    private void Start()
    {
        Vector2 middle = new Vector2((int)(cityDimensions.x / 2), (int)(cityDimensions.y / 2));

        for (int i = 0; i < cityDimensions.x; i++)
        {   
            placementOffset = Random.Range(0, maxRowOffset);

            for (int j = 0; j < cityDimensions.y; j++)
            {
                Transform block = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)]).transform;

                float xPos = (i - middle.x) * blockSize;
                float yPos = (j - middle.y) * blockSize + placementOffset;

                block.position = new Vector3(xPos, 0, yPos);
                block.eulerAngles = new Vector3(-90, 0, 90 * Random.Range(0, 3));
                block.parent = transform;
                
                // Add each spawnpoint to the list
                foreach(Transform tr in block)
                {
                    if(tr.tag == "Spawnpoint")
                    {
                        spawnpoints.Add(tr);
                        // Transform door = Instantiate(doorPrefab).transform;
                        // door.position = tr.position;
                        // door.parent = tr;
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
        foreach(Transform point in finalPoints)
        {
            Transform door = Instantiate(doorPrefab).transform;
            door.position = point.position;
            door.parent = point;
            Debug.DrawLine(door.position, door.position + Vector3.up * 50, Color.red);
        }
    }
}
