using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSetting : MonoBehaviour
{
    public GameObject SmallerBox;
    public GameObject LaregerBox;
    //public bool IsFitted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Pushable")
        {
            SmallerBox.GetComponent<IsBoxOnEdge>().Unlock();
            SmallerBox.transform.SetParent(gameObject.transform.parent); 
        }
    }

    void OnCollisionEnter2D(Collision2D box)
    {
        if (box.transform.tag == "Pushable")
        {
            SmallerBox.GetComponent<IsBoxOnEdge>().OnLock();
            SmallerBox.transform.SetParent(gameObject.transform); //SmallerBox是子物件           
            Debug.Log(123);
        }        
    }
}
