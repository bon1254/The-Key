using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemMonkey1Mushroom : MonoBehaviour
{
    bool debug = false;
    public bool Debug_DialogueManagerOn = false;
    public string[] sentences;
    private int index1 = 0;
    public Text textDisplay;

    public GameObject DialogueManager;
    public GameObject Mushroom;
    public GameObject continueButton;
    public CanvasGroup DialogueCanvas;

    public float typingspeed;
    public float FadeTime = 1f;

    public GameObject Player;
    public Rigidbody2D PlayerRb2d;

    public bool InMushroom = false;
    public bool CanMove = false;

    // Start is called before the first frame update
    private void Awake()
    {
        Debug.LogError("I'm IN");
        PlayerRb2d.GetComponent<Rigidbody2D>();
        //Player.GetComponent<PlayerMovement>().enabled = false;     //玩家一開始不動      
    }

    public void StartDialogue()
    {
        typingspeed = 0.05f;
        DialogueManager.SetActive(true);
        FadeIn();
        PlayerRb2d.velocity = Vector2.zero;
        Player.GetComponent<PlayerMovement>().animator.Rebind();
        Player.GetComponent<PlayerMovement>().enabled = false; //玩家不動  
        CanMove = true;
        StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        yield return new WaitForSeconds(.5f);
        foreach (char letter in sentences[index1].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        continueButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug_DialogueManagerOn && Debug_DialogueManagerOn != debug)
        {
            debug = Debug_DialogueManagerOn;
            StartDialogue();
        }       
    }

    public void FadeIn()
    {
        DialogueManager.SetActive(true);
        StartCoroutine(FadeCanvasGroup(DialogueCanvas, DialogueCanvas.alpha, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(DialogueCanvas, DialogueCanvas.alpha, 0));
        Invoke("CloseDialogueManager", 1);
    }

    void CloseDialogueManager()
    {
        DialogueManager.SetActive(false);
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

        print("Done");
        yield return new WaitForSeconds(1.2f);
        StopCoroutine("FadeCanvasGroup");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {            
            InMushroom = true;
            StartDialogue();           
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InMushroom = false;
        }
    }
}
