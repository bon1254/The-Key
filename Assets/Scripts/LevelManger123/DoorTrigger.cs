using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject[] Switches;
    NextLevel nextLevel;

    public AsyncOperation LoadingScene;
    public Animator animator;

    public ForDoorOpen forDoorOpen;
    int noOfSwitches = 0;
    public int SceneInt;

    // Start is called before the first frame update
    void Start()
    {
        GetNoOfSwitches();
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
            if (forDoorOpen.isOn == true)
            {                
                StartCoroutine(LoadScene(SceneInt + 1));
                Debug.Log("go");
            }
        }
    }
}
