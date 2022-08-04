﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemMonkey3 : DialogueSystemBase
{
    [Header("Player")]
    public GameObject Player;
    public Rigidbody2D PlayerRb2d;
    public PlayerMovement playermovement;

    [Header("UI")]
    public Text textDisplay;
    public Image textImage;
    public GameObject Tip;

    [Header("CanvasGroup")]
    public CanvasGroup DialogueCanvas;
    public CanvasGroup TipCanvas;

    [Header("DialogueSystem")]
    public GameObject DialogueManager;
    public GameObject DialogueFindHole;

    [Header("States")]   
    public bool FindHole = false;

    [Header("TypingSystem")]
    public string[] sentences;
    private int index;
    public float typingspeed = 0.025f;
    public float FadeTime;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;

    Animator animator;
    Coroutine c;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerRb2d.GetComponent<Rigidbody2D>();
        playermovement.PlayerControlable = false;     //玩家一開始不動      

        Invoke("StartDialogue", 3);
    }

    public override void StartDialogue()
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
        if (Input.GetMouseButtonDown(0) && IsTalking)  //按下左鍵+對話開啟
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
                    if (index == 1)    //先對話2句話，超過段落數量時結束
                    {
                        playermovement.PlayerControlable = true;    //玩家可以動了  
                        FadeOut();
                    }
                    else if (index == 2)    //先對話1句話，超過段落數量時結束          
                    {
                        playermovement.PlayerControlable = true;     //玩家可以動了
                        FadeOut();
                        DialogueFindHole.SetActive(false);
                        TipFadeIn();                        
                    }
                    else
                    {
                        c = StartCoroutine(Type());
                    }
                }
            }
        }
    }

    public void TipFadeIn()
    {
        Tip.SetActive(true);
        StartCoroutine(FadeCanvasGroup(TipCanvas, TipCanvas.alpha, 1));
        Invoke("TipFadeOut", 3.5f);
    }

    public void TipFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(TipCanvas, TipCanvas.alpha, 0));
        Invoke("CloseTip", 3.5f);
    }

    void CloseTip()
    {
        Tip.SetActive(false);      
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MonkeyThreeDialogueFindHole")
        {
            if (FindHole == false)
            {
                FindHole = true;
                FadeIn();   //FadeIn 
                c = StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();  //玩家不動  
                playermovement.PlayerControlable = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MonkeyThreeDialogueFindHole")
        {
            FindHole = false;
        }       
    }
}
