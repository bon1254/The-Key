using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDoorOpen : MonoBehaviour
{
    [SerializeField]
    GameObject Door;
    public AudioSource Open;
    public bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
               Debug.Log("isture");
               isOn = true;
               Open.Play();
               Door.SetActive(true);
            }
        }
        
    }

}
