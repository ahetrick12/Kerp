using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerHandler : MonoBehaviour
{
    public GameObject markerPrefab;

    private Transform[] levelDoors;
    private RectTransform[] markers;

    private CityGeneration cityGen;
    private Camera cam;

    void Awake()
    {
        cityGen = FindObjectOfType<CityGeneration>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelDoors = cityGen.getLevelDoors();
        markers = new RectTransform[levelDoors.Length];

        for(int i = 0; i < levelDoors.Length; i++)
        {
            GameObject marker = Instantiate(markerPrefab, transform.position, Quaternion.identity, transform);
            markers[i] = marker.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < markers.Length; i++)
        {
            if (levelDoors[i].GetComponent<Renderer>().isVisible)
            {
                markers[i].gameObject.SetActive(true);
                markers[i].position = cam.WorldToScreenPoint(levelDoors[i].position + Vector3.up * .5f);
            }
            else
            {
                markers[i].gameObject.SetActive(false);
            }
        }
    }
}
