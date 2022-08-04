using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemPrologueWhySoShaking : MonoBehaviour
{    
    public string[] sentences;
    private int index = 0;
    public Text textDisplay;

    public GameObject DialogueManager;
    public CanvasGroup DialogueCanvas;
    public DialogueSystemPrologueSlate dialogueSystemPrologueSlate;
    public DialogueSystemPrologueFabric dialogueSystemPrologueFabric;
    public float typingspeed = 0.025f;
    public float FadeTime = 1f;

    public GameObject Player;
    public Rigidbody2D PlayerRb2d;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    public void StartDialogue()
    {
        Debug.Log("go");
        FadeIn();   //FadeIn 
        PlayerRb2d.velocity = Vector2.zero;
        Player.GetComponent<PlayerMovement>().animator.Rebind();
        Player.GetComponent<PlayerMovement>().enabled = false; //玩家不動  
        StartCoroutine(Type());
        StopCoroutine(dialogueSystemPrologueSlate.Type());
        StopCoroutine(dialogueSystemPrologueFabric.Type());
        Invoke("FadeOut", 3.5f);
    }

    public IEnumerator Type()
    {
        yield return new WaitForSeconds(.5f);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueSystemPrologueSlate.WhySoShaking == true)
        {
            dialogueSystemPrologueSlate.WhySoShaking = false;
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
            }
            else
            {
                textDisplay.text = "";

            }
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

        yield return new WaitForSeconds(1.2f);
        StopCoroutine("FadeCanvasGroup");
    }   
}
