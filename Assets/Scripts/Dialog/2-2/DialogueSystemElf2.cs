using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemElf2 : MonoBehaviour
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
    public GameObject DialogueManager;

    [Header("TypingSystem")]
    public string[] sentences;
    private int index;
    public float typingspeed = 0.025f;
    public float FadeTime;
    public bool ForTreeCheck = false;
    public bool IsFinishTalking = false;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;

    [Header("Audio")]
    public AudioManager audioManager;

    Animator animator;
    Coroutine c;
    // Start is called before the first frame update
    private void Awake()
    {
        playermovement.PlayerControlable = false;     //玩家一開始不動             
        Invoke("StartDialogue", 3);
    }

    public void StartDialogue()
    {
        FadeIn();
        c = StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        textDisplay.text = string.Empty;

        IsTyping = true;
        IsTalking = true;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        IsTyping = false;
        IsFinishTalking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsTalking && ForTreeCheck == false)  //按下左鍵+對話開啟
        {            
            playermovement.PlayerControlable = true;
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
                textDisplay.text = string.Empty;
                ForTreeCheck = true;
            }
            else   //還沒講完話
            {
                StartCoroutine(Type());
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
}
