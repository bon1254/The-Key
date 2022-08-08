using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPController : MonoBehaviour 
{
    public bool CPReached;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CPReached = true;
        }

    }
}
