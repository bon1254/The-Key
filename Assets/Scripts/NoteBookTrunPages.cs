using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookTrunPages : MonoBehaviour
{
    public List<GameObject> Pages = new List<GameObject>();
    public GameObject[] _p;
    int i;
    public GameObject bookBlank;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
       // bookBlank = GameObject.Find("NoteBookBlank");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
           // Destroy(boolBlank);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q");

            Pages.Remove(Pages[0]);
        }*/
        /*
        if(Input.GetKeyDown(KeyCode.E))
        {
            Pages.Add(_p[1]);
            Pages[2] = _p[1];
        }   */
    }

    public void DestroyBlank()
    {
        Destroy(bookBlank);
    }

    public void TrunBookPagesRight()
    {
        Pages[i].SetActive(true);
        audioSource.Play();
        if (Pages.Count -1 > i)
        {
            i++;  
        }               
    }

    public void TrunBookPagesLeft()
    {
        Pages[i].SetActive(false);
        audioSource.Play();
        if (i > 0)
        {
            i--;
        }        
    }
}
