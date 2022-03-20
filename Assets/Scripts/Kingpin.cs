using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kingpin : MonoBehaviour
{
    private Transform promptText;

    private string[] dialogue1 = new string[]
    {
        "What do you want, henchman? Oh wait... it's you: the one with the jetpack.",
        "In that case, good thing you are here. I have errands that need running.",
        "Or should I say flying.",
        "Anyhow, why don't you go and collect some more kerp from my underlings.",
        "You remember how right?",
        "Fly to the roof top, I've sent you the locations, and break in.",
        "Sneak through, or be loud about it, it makes no difference to me,",
        "And harvest all the kerp you can carry.",
        "But remember two important things:",
        "Don't get caught,",
        "And don't come back empty handed.",
        "Now, run along."
    };

    private string[] dialogue2 = new string[]
    {
        "Oh good, you have the kerp.",
        "Wait a minute... what do you mean you don't have it all?",
        "What are you doing here empty handed!",
        "GET OUT! AND DON'T COME BACK UNTIL IT'S ALL HERE!",
        "GET HARVESTING!"
    };

    private string[] dialogue3 = new string[]
    {
        "Henchman. Welcome back to the den. Got what I asked for?",
        "Ahh, there's the stuff. I think I'll try some myself.",
        "*nom*",
        "Hmm, good work today. This is fine kerp-",
        "But it tastes-",
        "It tastes-",
        "You poisoned this!",
        "You'll pay-",
        "Hrngh...",
        "*bleh*"
    };

    private int dialogueIndex;

    public Vector3 dialogueLocation;

    public Text speechBubble;
    

    // Start is called before the first frame update
    void Awake()
    {
        dialogueIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speechBubble.transform.rotation = Camera.main.transform.rotation;
    }

    public void Talk()
    {
        if(LevelManager.kerpCount == 0)
        {
            if(dialogueIndex < dialogue1.Length)
            {
                speechBubble.text = dialogue1[dialogueIndex];
                dialogueIndex++;
            }
            else
            {
                speechBubble.text = "";
                dialogueIndex = 0;
            }
        }
        else if(LevelManager.kerpCount < 3)
        {
            if(dialogueIndex  < dialogue2.Length)
            {
                speechBubble.text = dialogue2[dialogueIndex];
                dialogueIndex++;
            }
            else
            {
                speechBubble.text = "";
                dialogueIndex = 0;
            }
        }
        else if(LevelManager.kerpCount == 3)
        {
            if(dialogueIndex  < dialogue3.Length)
            {
                speechBubble.text = dialogue3[dialogueIndex];
                dialogueIndex++;
            }
            else
            {
                speechBubble.text = "";
                SceneManager.LoadScene("Credits");
            }
        }
        
        
    }

    // public void onHover(Transform cam)
    // {
    //     promptText.gameObject.SetActive(true);
    //     promptText.rotation = cam.rotation;
    // }
}
