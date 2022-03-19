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
    

    private float placementOffset;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < cityDimensions.x; i++)
        {   
            placementOffset = Random.Range(0, maxRowOffset);

            for (int j = 0; j < cityDimensions.y; j++)
            {
                GameObject block = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)]);

                float xPos = (i - (int)(cityDimensions.x / 2)) * blockSize;
                float yPos = (j - (int)(cityDimensions.y / 2)) * blockSize + placementOffset;

                block.transform.position = new Vector3(xPos, 0, yPos);
                block.transform.eulerAngles = new Vector3(-90, 0, 90 * Random.Range(0, 3));
                block.transform.parent = transform;
                SpawnDoors(block);
            }
        }
    }

    private void SpawnDoors(GameObject block) {
        List<Transform> spawnpoints = new List<Transform>();
        foreach(Transform tr in block.transform)
        {
            if(tr.tag == "Spawnpoint")
            {
                spawnpoints.Add(tr);
                Transform door = Instantiate(doorPrefab).transform;
                door.position = tr.position;
                door.parent = tr;
            }
        }
    }
}
