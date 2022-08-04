using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStop : MonoBehaviour
{
    public Monkey monkey;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            monkey.GetComponent<Monkey>().enabled = false;
        }
    }
}
