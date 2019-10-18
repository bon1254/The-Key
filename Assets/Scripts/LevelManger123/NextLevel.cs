using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{ 
    GameObject UICanvasX;
    public int SceneInt;
    public AsyncOperation LoadingScene;
    public Animator animator;

    [SerializeField]
    GameObject[] Switches;
    public ForDoorOpen forE;
    int noOfSwitches = 0;

    void Start()
    {
        animator = GameObject.Find("PanelGoNext").GetComponent<Animator>();
        UICanvasX = GameObject.Find("UICanvas");
        GetNoOfSwitches();
        DontDestroyOnLoad(UICanvasX);
    }

    public int GetNoOfSwitches()
    {
        int x = 0;

        for (int i = 0; i < Switches.Length; i++)
        {
            if (Switches[i].GetComponent<ForDoorOpen>().isOn == false)
            {
                x++;
            }
            else if (Switches[i].GetComponent<ForDoorOpen>().isOn == true)
            {
                noOfSwitches--;
            }

        }

        noOfSwitches = x;
        return noOfSwitches;
    }


    public IEnumerator LoadScene(int SceneInt)
    {        
        yield return new WaitForSeconds(1f);
        LoadingScene = SceneManager.LoadSceneAsync(SceneInt);
        LoadingScene.allowSceneActivation = false;
        while (LoadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        animator.Play("MonkeyOneToTwoFinish");
        LoadingScene.allowSceneActivation = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SceneInt = SceneManager.GetActiveScene().buildIndex;      
            StartCoroutine(LoadScene(SceneInt + 1));
            animator.Play("MonkeyOneToTwoStart");
            
            Debug.Log("go");
            Debug.Log(LoadScene(SceneInt + 1));
        }
    }
}
