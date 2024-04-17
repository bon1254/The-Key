using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemMonkey1 : DialogueSystemBase
{
    [Header("Player")]
    public GameObject Player;
    public Rigidbody2D PlayerRb2d;
    public PlayerMovement playerMovement;

    [Header("Enemy")]
    public GameObject EnemyMonkey;

    [Header("UI")]
    public Text textDisplay;
    public Image textImage;
    public GameObject Tip;
    public GameObject NoteBookTip;
    public SwitchSceneAnimationEvery switchSceneAnimationEvery;
    public Slates SlatesController;
    public NoteBookTrunPages NBtrunPages;
    public GameObject Nextlevel;

    [Header("CanvasGroup")]
    public CanvasGroup DialogueCanvas;
    public CanvasGroup NoteBookTipCanvas;
    public CanvasGroup TipCanvas;

    [Header("DialogueSystem")]
    public GameObject DialogueManager;
    public GameObject DialogueVillage;
    public GameObject DialogueEnemyMonkey;

    [Header("States")]
    public bool InVillage = false;
    public bool InEnemyMonkey = false;

    [Header("TypingSystem")]
    private int index;
    public float typingspeed = 0.025f;
    public float FadeTime;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;

    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Sentences")]
    public string[] sentences;

    Animator animator;   
    Coroutine c;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerRb2d.GetComponent<Rigidbody2D>();
        playerMovement.PlayerControlable = false;    //玩家一開始不動                 

        //筆記本更新
        NBtrunPages = FindObjectOfType<NoteBookTrunPages>().GetComponent<NoteBookTrunPages>();
        NBtrunPages.DestroyBlank();
        NBtrunPages.Pages.Add(NBtrunPages._p[0]);
        NBtrunPages.Pages.Remove(NBtrunPages.Pages[0]);

        //石板更新
        SlatesController = FindObjectOfType<Slates>().GetComponent<Slates>();
        SlatesController.Slate1Appear();

        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.Monkey1BGM();
        Invoke("StartDialogue", 5);
    }

    public override void StartDialogue()
    {
        index = 0;
        FadeIn();
        c = StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        IsTyping = true;
        IsTalking = true;
        yield return new WaitForSeconds(1f);

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
        Debug.Log(index);
        Debug.Log(IsTalking);
        Debug.Log(IsTyping);

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
                    if (index == 2)    //先對話2句話，超過段落數量時結束
                    {  
                        FadeOut();
                        NoteBookTipFadeIn();
                    }
                    else if (index == 3 && InVillage == true)    //先對話1句話，超過段落數量時結束                村莊
                    {
                        playerMovement.PlayerControlable = true;    //玩家可以動了
                        FadeOut();
                        DialogueVillage.SetActive(false);
                    }
                    else if (index == 7 && InEnemyMonkey == true)    //先對話4句話，超過段落數量時結束                捲尾猴村民
                    {
                        FadeOut();
                        DialogueEnemyMonkey.SetActive(false);
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

    public void NoteBookTipFadeIn()
    {
        NoteBookTip.SetActive(true);
        StartCoroutine(FadeCanvasGroup(NoteBookTipCanvas, NoteBookTipCanvas.alpha, 1));
        Invoke("NoteBookTipFadeOut", 3.5f);
    }

    public void NoteBookTipFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(NoteBookTipCanvas, NoteBookTipCanvas.alpha, 0));
        Invoke("NoteBookCloseTip", 3.5f);
    }

    void NoteBookCloseTip()
    {
        NoteBookTip.SetActive(false);
        playerMovement.PlayerControlable = true;    //筆記本提示結束 玩家可以動了
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
        Invoke("CloseTip", 1);
    }

    void CloseTip()
    {
        Tip.SetActive(false);
        EnemyMonkey.GetComponent<Monkey>().enabled = true;
        playerMovement.PlayerControlable = true;
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
        if (collision.tag == "DialogueFieldVillage")
        {
            if (InVillage == false)
            {
                FadeIn();   //FadeIn 
                InVillage = true;                
                c = StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                Player.GetComponent<PlayerMovement>().animator.Rebind();
                playerMovement.PlayerControlable = false;   //玩家不動 
            }
        }

        if (collision.tag == "DialogueFieldEnemyMonkey")
        {
            if (InEnemyMonkey == false)
            {
                FadeIn();  //FadeIn 
                InEnemyMonkey = true;
                EnemyMonkey.SetActive(true);
                c = StartCoroutine(Type());
                PlayerRb2d.velocity = Vector2.zero;
                EnemyMonkey.GetComponent<Monkey>().enabled = false; //捲尾猴村民不動
                Player.GetComponent<PlayerMovement>().animator.Rebind();
                playerMovement.PlayerControlable = false;   //玩家不動 
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {       
        if (collision.tag == "DialogueFieldVillage")
        {
            InVillage = false;
        }

        if (collision.tag == "DialogueFieldEnemyMonkey")
        {
            InEnemyMonkey = false;
        }
    }
}
