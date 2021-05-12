using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemElfes : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    public Rigidbody2D PlayerRb2d;
    public PlayerMovement playermovement;

    [Header("UI")]
    public Text textDisplay;
    public Image textImage;

    [Header("CanvasGroup")]
    public CanvasGroup DialogueCanvas;

    [Header("DialogueSystem")]
    public GameObject DialogueInjuredElf;
    public GameObject DialogueManager;
    public GameObject EnergyTaking;

    [Header("States")]
    public bool IninjuredElf = false;
    public bool TakingEnergy = false;

    [Header("TypingSystem")]
    public string[] sentences;
    private int index;
    public float typingspeed = 0.025f;
    public float FadeTime;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;

    [Header("Audio")]
    public AudioManager audioManager;

    Animator animator;
    Coroutine c;

    public void StartDialogue()
    {
        FadeIn();
        c = StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        IsTyping = true;
        IsTalking = true;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        IsTyping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsTalking && IninjuredElf == true)  //按下左鍵+對話開啟
        {
            StopCoroutine(c);
            if (IsTyping) //正在打字
            {
                textDisplay.text = sentences[index];
                IsTyping = false;
            }
            else   //講完句子
            {
                textDisplay.text = "";
                IsTalking = false;
                if (index < sentences.Length)
                {
                    index++;
                    if (index == 16)    //先對話1句話，超過段落數量時結束
                    {
                        playermovement.PlayerControlable = true;    //玩家可以動了  
                        FadeOut();
                        StopCoroutine(c);
                        EnergyTaking.SetActive(true);
                        DialogueInjuredElf.SetActive(false);
                    }
                    else if (index == 20)    //幫助妖精
                    {
                        playermovement.PlayerControlable = true;    //玩家可以動了                        
                        DialogueInjuredElf.SetActive(false);
                        FadeOut();
                    }
                    else
                    {
                        c = StartCoroutine(Type());
                    }
                }
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
        yield return new WaitForSeconds(1f);
        StopCoroutine("FadeCanvasGroup");
    }
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IninjuredElf = true;
            FadeIn();           
            StartDialogue();

            PlayerRb2d.velocity = Vector2.zero;
            Player.GetComponent<PlayerMovement>().animator.Rebind();  //玩家不動  
            playermovement.PlayerControlable = false;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IninjuredElf = false;
        }
    }
}
