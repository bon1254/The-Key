using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNextLevel : MonoBehaviour
{
    public NextLevel nextLevel;    

    void Awake()
    {
        nextLevel = FindObjectOfType<NextLevel>().GetComponent<NextLevel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            nextLevel.LoadScene();
        }
    }
}
