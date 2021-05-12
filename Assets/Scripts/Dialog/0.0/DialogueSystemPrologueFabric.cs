using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemPrologueFabric : MonoBehaviour
{
    public string[] sentences;
    private int index = 0;
    public Text textDisplay;

    public GameObject DialogueManager;
    public CanvasGroup DialogueCanvas;

    public float typingspeed = 0.025f;
    public float FadeTime = 1f;

    public GameObject Player;
    public Rigidbody2D PlayerRb2d;

    public bool InFabric = false;
    public bool ForFabricCheck = false;
    public bool IsTalking = false; //對話開啟
    public bool IsTyping = false;
    public bool IsFinishTalking = false;

    Coroutine c;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerRb2d.GetComponent<Rigidbody2D>();     
    }
    
    public void StartDialogue()
    {
        FadeIn() ;   //FadeIn 
        PlayerRb2d.velocity = Vector2.zero;
        Player.GetComponent<PlayerMovement>().animator.Rebind();
        Player.GetComponent<PlayerMovement>().enabled = false; //玩家不動  
        IsTalking = true;
        c = StartCoroutine(Type());
    }

    public IEnumerator Type()
    {
        IsTyping = true;
        IsFinishTalking = false;
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
        if (Input.GetKeyDown(KeyCode.E) && InFabric == true)
        {
            ForFabricCheck = false;
            InFabric = false;
            StartDialogue();
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

        if (Input.GetMouseButtonDown(0) && IsTalking == true && ForFabricCheck == false)  //按下左鍵+對話開啟
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
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
                ForFabricCheck = true;
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

        print("Done");
        yield return new WaitForSeconds(1.2f);
        StopCoroutine("FadeCanvasGroup");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InFabric = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InFabric = false;
        }
    }
}
