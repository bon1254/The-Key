using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemPrologue0 : DialogueSystemBase
{
    public string[] sentences;
    private int index = 0;
    public Text textDisplay;

    public GameObject DialogueManager;
    public CanvasGroup DialogueCanvas;
    public CanvasGroup TipCanvas;
    public GameObject Tip;

    public float typingspeed = 0.025f;
    public float FadeTime = 1f;
    public PrologueSceneChange prologueSceneChange;
    public GameObject Player;
    public Rigidbody2D PlayerRb2d;
    
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;
    public bool IsFinishTalking = false;
    public bool ForZeroCheck = false;
    public bool IsTipAppeared = false;

    Coroutine c;

    private void Awake()
    {       
        PlayerRb2d.GetComponent<Rigidbody2D>();
        Player.GetComponent<PlayerMovement>().enabled = false;     //玩家一開始不動      

        Invoke("StartDialogue", 5);
    }

    public override void StartDialogue()
    {
        typingspeed = 0.025f;
        FadeIn();
        PlayerRb2d.velocity = Vector2.zero;
        Player.GetComponent<PlayerMovement>().animator.Rebind();
        Player.GetComponent<PlayerMovement>().enabled = false; //玩家不動  
        IsTalking = true; //對話開啟
        c = StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsTalking == true && ForZeroCheck == false)  //按下左鍵+對話開啟
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
            if (IsTipAppeared == false)
            {
                TipFadeIn();
            }            
            Invoke("TipFadeOut", 3.5f);           
            if (IsTyping) //正在打字
            {
                textDisplay.text = sentences[index];
                IsTyping = false;
                IsFinishTalking = true;
                StopCoroutine(c);
            }
            else if (IsFinishTalking) //講完話
            {
                FadeOut(); //關閉對話框
                StopCoroutine(c);
                ForZeroCheck = true;
            }
            else   //還沒講完話
            {
                StartCoroutine(Type());
            }
        }
    }

    public void TipFadeIn()
    {
        Tip.SetActive(true);
        IsTipAppeared = true;
        StartCoroutine(FadeCanvasGroup(TipCanvas, TipCanvas.alpha, 1));
    }

    public void TipFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(TipCanvas, TipCanvas.alpha, 0));
        Invoke("CloseTip", 1);
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

    public IEnumerator Type()
    {
        IsTyping = true;
        IsFinishTalking = false;
        yield return new WaitForSeconds(.5f);
        foreach (char letter3 in sentences[index].ToCharArray())
        {
            textDisplay.text += letter3;
            yield return new WaitForSeconds(typingspeed);          
        }
        IsTyping = false;
        IsFinishTalking = true;
    } 
}
