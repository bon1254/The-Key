using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{ 
    public int SceneInt;
    public AsyncOperation EveryLoadingScene;
    public Animator animator;
    public PlayerMovement playerMovement;
    public DestroyGoMenu destroyGoMenu;
    //public Monkey monkey;

    [SerializeField]
    GameObject[] Switches;
    public ForDoorOpen forE;
    int noOfSwitches = 0;

    private static NextLevel instance = null;
    public static NextLevel Instance
    {
        get { return instance; }
    }


    void Awake()
    {        
        animator = GameObject.Find("PanelGoNext").GetComponent<Animator>();

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

    bool AnimationPlayFinished = false;

    public IEnumerator LoadSceneKeep(int SceneInt)
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        EveryLoadingScene = SceneManager.LoadSceneAsync(SceneInt);
        EveryLoadingScene.allowSceneActivation = false;
        while (EveryLoadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        EveryLoadingScene.allowSceneActivation = true;
        yield return new WaitForSeconds(3f);
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StartCoroutine(WaitUntilAnimationFinish());
    }

    IEnumerator WaitUntilAnimationFinish()
    {
        yield return new WaitForSecondsRealtime(2);
        animator.Play("SceneChangeFinish");//感覺是這個沒做?
        while (!AnimationPlayFinished)
        {
            yield return new WaitForSecondsRealtime(1);
        }      
        yield return new WaitForSecondsRealtime(1);
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    public void DoSwitchScene()
    {
        StartCoroutine(LoadSceneKeep(SceneInt));
    }
    public void DestoryThis()
    {
        AnimationPlayFinished = true;
    }

    public void LoadScene()
    {
        SceneInt = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(SceneInt);
        animator.Play("SceneChangeStart");
    }
}
