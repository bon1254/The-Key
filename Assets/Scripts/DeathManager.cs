using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject ReStartBT;
    public GameObject MenuBT;
    public AsyncOperation LoadingScene;
    public Animator animator;
    public GameObject Panel;
    public PasueMenu pauseMenu;
    public DestroyGoMenu destroyGoMenu;
    public GameObject deathManager;

    public void Awake()
    {   
        destroyGoMenu = FindObjectOfType<DestroyGoMenu>().GetComponent<DestroyGoMenu>();
        Invoke("CloseDeathManger", 1);
    }
    public void CloseDeathManger()
    {
        deathManager.SetActive(false);
    }

    public void DeathPanelButtonActive()
    {       
        ReStartBT.SetActive(true);
        MenuBT.SetActive(true);
    }

    public void SceneChangeToMenu()
    {
        Panel.SetActive(true);
        //
        destroyGoMenu.DestoryAll();
        //
        StartCoroutine(LoadToStart());
    }

    public IEnumerator LoadToStart()
    {
        animator.Play("PasuedAnimToMainStart");
        LoadingScene = SceneManager.LoadSceneAsync("Start");
        LoadingScene.allowSceneActivation = false;
        while (LoadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        animator.Play("PasuedAnimToMainFinish");
        LoadingScene.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(gameObject);
    }
}
