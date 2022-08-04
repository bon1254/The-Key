using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject DialoguePutFire;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            DialoguePutFire.SetActive(true);
            Destroy(gameObject);
        }
    }
}
