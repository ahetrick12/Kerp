using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kingpin : MonoBehaviour
{
    private Transform promptText;

    private string[] dialogue = new string[]
    {
        "What do you want, henchman? Oh wait... it's you: the one with the jetpack.",
        "In that case, good thing you are here. I have errands that need running.",
        "Or should I say flying.",
        "Anyhow, why don't you go and collect some more kerp from my underlings.",
        "You remember how right?",
        "Fly to the roof top, I've sent you the locations, and break in.",
        "Sneak through, or be loud about it, it makes no difference to me,",
        "And grab all the kerp you can carry.",
        "But remember two important things:",
        "Don't get caught,",
        "And don't come back empty handed.",
        "Now, run along."
    };

    private int dialogueIndex;

    public Vector3 dialogueLocation;

    public Text speechBubble;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speechBubble.transform.rotation = Camera.main.transform.rotation;
    }

    public void Talk()
    {
        if(dialogueIndex + 1 < dialogue.Length)
        {
            speechBubble.text = dialogue[dialogueIndex];
            dialogueIndex++;
        }
        else
        {
            speechBubble.text = "";
            //this.GetComponent<Interactable>().promptText.Get = "";
        }
        
    }

    // public void onHover(Transform cam)
    // {
    //     promptText.gameObject.SetActive(true);
    //     promptText.rotation = cam.rotation;
    // }
}
