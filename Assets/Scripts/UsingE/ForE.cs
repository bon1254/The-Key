using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForE : MonoBehaviour
{
    [SerializeField]
    GameObject Fire;

    public AudioSource Burning;
    public Animator animator;
    public BoxCollider2D boxCollider2d;
    public bool isOn;
    public bool _CanOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_CanOn == true)
            {
                isOn = true;
                Burning.Play();
                animator.SetBool("UP", true);
                Fire.SetActive(true);
                boxCollider2d.enabled = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _CanOn = true;
        }
        
    }
}
