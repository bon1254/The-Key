using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PasueMenu : MonoBehaviour
{
    public Animator animator1;
    public static bool GameIsPaused = false;
    public AsyncOperation LoadingScene;
    public GameObject PausedMenuCanvas;
    public GameObject OptionsCanvas;

    public GameObject NoteBook;
    public GameObject Slate;
    public DestroyGoMenu destroyGoMenu;

    public CanvasGroup[] UIElments;
    public GameObject Player;

    private int StartMenuIndex;

    
    private static PasueMenu instance = null;
    public static PasueMenu Instance
    {
        get { return instance; }
    }
    // Start is called before the first frame update
    void Awake()
    {    
        
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        destroyGoMenu = FindObjectOfType<DestroyGoMenu>().GetComponent<DestroyGoMenu>();
        destroyGoMenu.AddList(gameObject);
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            if (GameIsPaused)
            {               
                Resume();
            }
            else
            {              
                Paused();
            }
        }
    }

    IEnumerator LoadToStart()
    {
        yield return new WaitForSecondsRealtime(0.8f);       
        LoadingScene = SceneManager.LoadSceneAsync("Start");
        LoadingScene.allowSceneActivation = false;
        while (LoadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        animator1.Play("PasuedAnimToMainFinish");
        LoadingScene.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1.0f);

        Destroy(gameObject);
    }

    public void RestartGame()
    {
        PausedMenuCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);// loads current scene        
        Time.timeScale = 1f;       
    }

    public void LoadSceneToStart()
    {
        destroyGoMenu.DestoryAll();
        //
        animator1.Play("PasuedAnimToMainStart");
        StartCoroutine(LoadToStart());                 
        Invoke("DoFadeOut", 1f);
    }  

    public void DoFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 0));
    }

    //暫停鍵//////////////////
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

    public void PasuedBacktoStartClick(int i)
    {
        StartMenuIndex = i;
        Invoke("PasuedBacktoStart", 0.5f);//0.5秒後執行OptionsMenuStart     
        Debug.Log(UIElments[StartMenuIndex]);
    }

    public void PasuedBacktoStart()
    {
        StartCoroutine(FadeCanvasGroup(UIElments[StartMenuIndex], UIElments[StartMenuIndex].alpha, 0));//Fade out OptionMenu  
        Debug.Log(UIElments[StartMenuIndex]);
    }
    /////////////////

    public void Resume()
    {
        PausedMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void OptionsCanvasPaused()
    {
        OptionsCanvas.SetActive(true);
        PausedMenuCanvas.SetActive(false);
    }

    public void OptionsCanvasPausedBack()
    {
        OptionsCanvas.SetActive(false);
        PausedMenuCanvas.SetActive(true);
    }

    public void Paused()
    {
        PausedMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void notebookOpen()
    {
        NoteBook.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void notebookClose()
    {
        NoteBook.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void SlateOpen()
    {
        Slate.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void SlateClose()
    {
        Slate.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
 
}
