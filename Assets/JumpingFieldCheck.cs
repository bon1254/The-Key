using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFieldCheck : MonoBehaviour
{
    public bool IsMonkey = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            IsMonkey = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            IsMonkey = false;
        }
    }
}
