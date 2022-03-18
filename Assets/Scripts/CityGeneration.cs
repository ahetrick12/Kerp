using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneration : MonoBehaviour
{

    public int blockSize = 10;
    public GameObject[] blockPrefabs;
    public Vector2 cityDimensions = Vector2.one;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < cityDimensions.x; i++)
        {
            for (int j = 0; j < cityDimensions.y; j++)
            {
                GameObject block = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)]);
                block.transform.position = new Vector3(i * blockSize, 0, j * blockSize);
                block.transform.eulerAngles = new Vector3(-90, 0, 90 * Random.Range(0, 3));
            }
        }
    }
}
