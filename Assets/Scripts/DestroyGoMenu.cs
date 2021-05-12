using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyGoMenu : MonoBehaviour
{
    public List<GameObject> TargetGo = new List<GameObject>();

    
    private static DestroyGoMenu instance = null;
    public static DestroyGoMenu Instance
    {
        get { return instance; }
    }
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
    }

    public void DestoryAll()
    {
        Debug.LogError(TargetGo.Count);
        for (int i = 0; i < TargetGo.Count; i++)
        {
            Debug.LogError("i : " + i);

            SceneManager.MoveGameObjectToScene(TargetGo[i], SceneManager.GetActiveScene());
        }
        TargetGo.Clear();
    }

    public void AddList(GameObject _o)
    {
        TargetGo.Add(_o);
    }
}
