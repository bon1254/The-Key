using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologueSceneChange : MonoBehaviour
{  
    public AsyncOperation LoadingSceneToOne;
    public Image fade;
    public Animator animator;

    bool AnimationPlayFinished = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Load()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        yield return new WaitForSecondsRealtime(2);
        LoadingSceneToOne = SceneManager.LoadSceneAsync("MonkeyOne");
        LoadingSceneToOne.allowSceneActivation = false;
        while (LoadingSceneToOne.progress < .8)
        {
            yield return new WaitForEndOfFrame();
        }
        LoadingSceneToOne.allowSceneActivation = true;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StartCoroutine(WaitUntilAnimationFinish());
    }

    IEnumerator WaitUntilAnimationFinish()
    {
        yield return new WaitForSecondsRealtime(2);
        animator.Play("PrologueBlackFadeOut");
        while (!AnimationPlayFinished)
        {
            yield return new WaitForSecondsRealtime(1);
        }       
        yield return new WaitForSecondsRealtime(1);
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;       
        Destroy(gameObject);
    }

    public void DoSwitchScene()
    {       
        StartCoroutine(Load());
    }
    public void DestoryThis()
    {
        AnimationPlayFinished = true;
    }

    public void LoadScene()
    {
        animator.Play("PrologueBlackFadeIn");
    }

    private void OnDestroy()
    {
        Debug.LogError("I'm dead now");
    }
}
