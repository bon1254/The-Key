using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup[] UIElments;
    public AsyncOperation LoadingScene;
    public GameObject OptionsMenu;
    public GameObject StartMenu;
    public GameObject ExitButton;
    public GameObject OptionButton;
    public GameObject StartButton;
    public GameObject StartToClickCanvas;

    public AudioSource ClickSound;
    public AudioSource HighlightedSound;
    private int StartMenuIndex;

    public Image fade;

    

    void Start()
    {
        //SceneManager.MoveGameObjectToScene(TargetGo, SceneManager.GetActiveScene());
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    bool AtStartAnimationPlayFinished = false;

    IEnumerator Load()
    {
        if (LoadingScene != null) yield break;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        LoadingScene = SceneManager.LoadSceneAsync("prologue");
        LoadingScene.allowSceneActivation = false;
        while (LoadingScene.progress < .8)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.LogError("LOAD STATE CHANGE");
        LoadingScene.allowSceneActivation = true;
        Debug.LogError("DID I DIED?");
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StartCoroutine(WaitUntilAnimationFinish());
    }

    IEnumerator WaitUntilAnimationFinish()
    {
        Debug.LogError("WAIT");
        yield return new WaitForSecondsRealtime(2);
        animator.Play("SceneChangeAnimFinish");
        Debug.LogError("Play");
        while (!AtStartAnimationPlayFinished)
        {
            Debug.LogError("Waiting...");
            yield return new WaitForSecondsRealtime(1);
        }      
        yield return new WaitForSecondsRealtime(1);
        Debug.LogError("Find player");
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        //var Player = GameObject.Find("Player0-0");
        //if (Player != null) Player.GetComponent<DialogueSystemPrologue0>().StartDialogue();
        Destroy(gameObject, 1);
    }

    public void DoSwitchScene_Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Load());
    }
    public void DestoryThis_Start()
    {
        Debug.LogError("Fin");
        AtStartAnimationPlayFinished = true;
    }

    public void LoadScene0()
    {
        OptionButton.SetActive(false);
        ExitButton.SetActive(false);
        StartButton.SetActive(false);
        animator.Play("SceneChangeAnimStart");
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }
   
    public void FadeIn(int i)
    {
        StartCoroutine(FadeCanvasGroup(UIElments[i], UIElments[i].alpha, 1));
    }

    public void FadeOut(int i)
    {
        StartCoroutine(FadeCanvasGroup(UIElments[i], UIElments[i].alpha, 0));
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
        StartToClickCanvas.SetActive(false);
        StopCoroutine("FadeCanvasGroup");
    }

    //起始畫面
    public void StartButtonClick(int i)
    {      
        StartMenuIndex = i;
        StartCoroutine(FadeCanvasGroup(UIElments[i], UIElments[i].alpha, 0));//Fade out StartToClick
    }

    public void MainMenuStartClick(int i)
    {
        StartMenuIndex = i;       
        Invoke("StartMenuStart", 1f);//0.5秒後執行MainMenuStart     
        Debug.Log("MainMenuStartClick");
    }

    public void StartMenuStart()
    {       
        StartMenu.SetActive(true);
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 1));//Fade in StartMenu       
    }

    public void OptionsMenuClick(int i)
    {
        if (StartMenu != null)
        {
            ClickSound.Play();
        }
        StartMenuIndex = i;
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 0));//Fade out StartMenu          
        Invoke("StartMenuCloseClick", 0.5f);//0.5秒後執行OptionsMenuStart     
     
    }

    public void StartMenuCloseClick(int i)
    {
        StartMenuIndex = i;
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 1));//Fade in OptionMenu    
    }

    public void BackStartMenuClick(int i)
    {
        StartMenuIndex = i;
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 0));//Fade out OptionMenu  
        Invoke("OptionMenuCloseClick", 0.5f); //0.5秒後執行MainMenuStart
    }

    public void OptionMenuCloseClick(int i)
    {
        StartMenuIndex = i;
        StartMenu.SetActive(true);
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 1));//Fade in StartMenu              
        OptionsMenu.SetActive(false);
    }

    public void HighLightedSound()
    {
        HighlightedSound.Play();
    }

    public void ClickSoundPlay()
    {
        ClickSound.Play();
    }
   
}
