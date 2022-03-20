using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject canvas;
    public float fadeoutTime = 5;

    private Image fadeoutPanel;
    private float elapsedTime = 0;

    private LevelManager levelManager;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        } 

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fadeoutPanel = canvas.transform.Find("Fadeout").GetComponent<Image>();
        fadeoutPanel.color = new Color(0,0,0,0);
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void BeingDeathSequence()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        elapsedTime = 0;
        while (elapsedTime < fadeoutTime)
        {
            Color col = fadeoutPanel.color;
            col.a = Mathf.Lerp(0, 255, (elapsedTime / fadeoutTime));
            fadeoutPanel.color = col;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        levelManager.ReturnToCity();

        elapsedTime = 0;
        while (elapsedTime < fadeoutTime)
        {
            Color col = fadeoutPanel.color;
            col.a = Mathf.Lerp(255, 0, (elapsedTime / fadeoutTime));
            fadeoutPanel.color = col;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
