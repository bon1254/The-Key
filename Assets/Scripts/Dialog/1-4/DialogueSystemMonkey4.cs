using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueSystemMonkey4 : DialogueSystemBase
{
    [Header("Player")]
    public GameObject Player;
    public Animator animator;
    public Rigidbody2D PlayerRb2d;
    public PlayerMovement playermovement;

    [Header("UI")]
    public Text textDisplay;
    public Image textImage;
    public GameObject StoryMap;
    public GameObject Tip;

    [Header("UIScripts")]
    public NoteBookTrunPages NBtrunPages;
    public Slates slatesBB;

    [Header("CanvasGroup")]
    public CanvasGroup DialogueCanvas;
    public CanvasGroup TipCanvas;
    public CanvasGroup StoryMapCanvas;

    [Header("DialogueSystem")]
    public GameObject DialogueManager;
    public GameObject DialogueBlocks;
    public GameObject DialogueSlateBlock;

    [Header("States")]
    public bool InBlock = false;
    public bool InSlateBlock = false;

    [Header("TypingSystem")]
    public string[] sentences;
    private int index;
    public float typingspeed = 0.025f;
    public float FadeTime;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;

    [Header("NextLevel")]   
    public NextLevel nextLevel;

    Coroutine c;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerRb2d.GetComponent<Rigidbody2D>();
        nextLevel = FindObjectOfType<NextLevel>().GetComponent<NextLevel>();
        slatesBB = FindObjectOfType<Slates>().GetComponent<Slates>();
        NBtrunPages = FindObjectOfType<NoteBookTrunPages>().GetComponent<NoteBookTrunPages>();
        playermovement.PlayerControlable = false;     //玩家一開始不動      
        Invoke("StartDialogue", 3);
    }

    public override void StartDialogue()
    {
        DialogueManager.SetActive(true);        
        c =  StartCoroutine(Type());
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
            //Player.GetComponent<PlayerMovement>().enabled = true;
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
                        playermovement.PlayerControlable = true;    //玩家可以動了
                        FadeOut();
                        DialogueBlocks.SetActive(false);
                        TipFadeIn();
                    }
                    else if (index == 3)    //先對話4句話，超過段落數量時結束                
                    {
                        FadeOut();
                        DialogueSlateBlock.SetActive(false);
                        animator.Play("SlateBlockBright");
                    }
                    else if (index == 5)    //先對話4句話，超過段落數量時結束        
                    {
                        FadeOut();
                        NBtrunPages.Pages.Add(NBtrunPages._p[1]);
                        slatesBB.Slate2Appear();
                        nextLevel.LoadScene();
                    }                  
                    else
                    {
                        c = StartCoroutine(Type());
                    }
                }
            }
        }
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
        StopCoroutine("FadeCanvasGroup");
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

    public void StoryMapFadeIn()
    {
        StoryMap.SetActive(true);
        StartCoroutine(FadeCanvasGroup(StoryMapCanvas, StoryMapCanvas.alpha, 1));
        Invoke("StoryMapFadeOut", 5);
    }
    public void StoryMapFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(StoryMapCanvas, StoryMapCanvas.alpha, 0));
        Invoke("CloseStoryMap", 5);
    }
    public void CloseStoryMap()
    {
        StoryMap.SetActive(false);
        c = StartCoroutine(Type());
        FadeIn();
    }
 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MonkeyFourDialogueBlocks")
        {
            if (InBlock == false)
            {
                FadeIn();
                DialogueManager.SetActive(true);
                InBlock = true;
                c = StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();  //玩家不動  
                playermovement.PlayerControlable = false;
            }
        }
        if (collision.tag == "MonkeyFourDialogueSlateBlock")
        {
            if (InSlateBlock == false)
            {
                FadeIn();
                DialogueManager.SetActive(true);
                InSlateBlock = true;
                c = StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();  //玩家不動  
                playermovement.PlayerControlable = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MonkeyFourDialogueBlocks")
        {
            InBlock = false;
        }

        if (collision.tag == "MonkeyFourDialogueBlocks")
        {
            InSlateBlock = false;
        }
    }
}
