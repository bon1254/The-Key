using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForE : MonoBehaviour
{
    [SerializeField]
    GameObject Fire123;

    public AudioSource Burning;
    public Animator animator;
    public BoxCollider2D boxCollider2d;
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
            if (Input.GetKeyDown(KeyCode.E))
            {
               isOn = true;
               Burning.Play();
               animator.SetBool("UP", true);
               Fire123.SetActive(true);
               boxCollider2d.enabled = false;
            }
        }
        
    }
}
