using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class DialogueSystemPrologueSlate : MonoBehaviour
{ 
    public string[] sentences;
    private int index = 0;
    public Text textDisplay;

    public GameObject DialogueManager;
    public CanvasGroup DialogueCanvas;
    public CanvasGroup StoryMapCanvas;
    public GameObject StoryMap;
    public GameObject PrologueSC;

    public float typingspeed = 0.025f;
    public float FadeTime5 = 1f;
    public PrologueSceneChange prologueSceneChange;
    public DialogueSystemPrologueWhySoShaking soShaking;

    public SimpleCameraShakeInCinemachine simpleCameraShakeInCinemachine;
    public CinemachineVirtualCamera VirtualCameraSlate;
    public AudioSource audioSource;

    public GameObject Player;
    public Rigidbody2D PlayerRb2d;

    public bool InSlate = false;
    public bool CheckKeyCodeE = false;
    public bool ChangeScene = false;
    public bool IsTalking = false;
    public bool IsTyping = false;
    public bool IsFinishTalking = false;
    public bool ForSlateCheck = false;
    public bool WhySoShaking = false;

    Coroutine c;

    // Start is called before the first frame update
    public void StartDialogue()
    {
        typingspeed = 0.025f;
        FadeIn();   
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
        if (Input.GetKeyDown(KeyCode.E) && InSlate == true)
        {
            InSlate = false;
            CheckKeyCodeE = true;
            StartDialogue();
            PrologueSC.SetActive(true);
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

        if (Input.GetMouseButtonDown(0) && IsTalking == true && ForSlateCheck == false)  //按下左鍵+對話開啟
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
                ForSlateCheck = true;
                FadeInStoryMap();
                Invoke("ShakeCamera", 4);
            }
            else   //還沒講完話
            {
                StartCoroutine(Type());
            }
        }
    }

    public void ShakeCamera()   //換場景搖動鏡頭
    {
        simpleCameraShakeInCinemachine.ShakeElapsedTime = simpleCameraShakeInCinemachine.ShakeDuration;
        prologueSceneChange.LoadScene();
        audioSource.Play();
        WhySoShaking = true;
        soShaking.StartDialogue();
    }

    public void FadeInStoryMap()
    {
        StoryMap.SetActive(true);
        StartCoroutine(FadeCanvasGroup(StoryMapCanvas, StoryMapCanvas.alpha, 1));
        Invoke("FadeOutStoryMap", 3);
    }
    public void FadeOutStoryMap()
    {
        StartCoroutine(FadeCanvasGroup(StoryMapCanvas, StoryMapCanvas.alpha, 0));
        Invoke("CloseStoryMap", 3);
    }
    public void CloseStoryMap()
    {
        StoryMap.SetActive(false);
        ChangeScene = true;
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
        if (collision.tag == "Player")
        {
            InSlate = true;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(FadeCanvasGroup(DialogueCanvas, DialogueCanvas.alpha, 0));
            InSlate = false;
        }
    }
}
