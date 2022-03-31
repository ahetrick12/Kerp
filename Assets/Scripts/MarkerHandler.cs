using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerHandler : MonoBehaviour
{
    public GameObject markerPrefab;

    private List<Transform> levelDoors;
    private List<RectTransform> markers;

    private CityGeneration cityGen;
    private Camera cam;
    private Transform hubMarker = null;

    void Awake()
    {
        if (LevelManager.inLevel) return;

        cityGen = FindObjectOfType<CityGeneration>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.inLevel) return;

        levelDoors = cityGen.getLevelDoors();
        markers = new List<RectTransform>();

        for(int i = 0; i < levelDoors.Count; i++)
        {
            GameObject marker = Instantiate(markerPrefab, transform.position, Quaternion.identity, transform);
            markers.Add(marker.GetComponent<RectTransform>());
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (LevelManager.inLevel) return;

        try
        {
            for (int i = 0; i < levelDoors.Count; i++)
            {
                if(levelDoors[i].GetComponentInChildren<Interactable>().clicked)
                {
                    levelDoors.Remove(levelDoors[i]);

                    Destroy(markers[i].gameObject);
                    markers.RemoveAt(i);
                    continue;
                }

                //if (levelDoors[i].GetComponentInChildren<Renderer>().isVisible)
                if (IsVisibleFrom(levelDoors[i].GetComponentInChildren<Renderer>(), cam))
                {
                    markers[i].gameObject.SetActive(true);
                    
                    markers[i].position = cam.WorldToScreenPoint(levelDoors[i].position + Vector3.up * .5f);
                }
                else
                {
                    markers[i].gameObject.SetActive(false);
                }
            }

            if (levelDoors.Count == 0)
            {
                if (hubMarker == null)
                {
                    for (int i = 0; i < markers.Count; i++)
                    {
                        markers[i].gameObject.SetActive(false);
                    }

                    hubMarker = Instantiate(markerPrefab, transform.position, Quaternion.identity, transform).transform;
                }

                Transform point = FindObjectOfType<CityGeneration>().hubPoint;
                if (point.GetComponentInChildren<Renderer>().isVisible)
                {
                   hubMarker.gameObject.SetActive(true);
                    
                    hubMarker.position = cam.WorldToScreenPoint(point.position + Vector3.up * .5f);
                }
                else
                {
                    hubMarker.gameObject.SetActive(false);
                }
            }
        }
        catch {}
    }

    // this works better than renderer.isVisible
    private bool IsVisibleFrom(Renderer r, Camera camera) {
    
        if (Vector3.Dot(cam.transform.forward, r.transform.position - cam.transform.position) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
