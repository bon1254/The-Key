using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBrokenWood : MonoBehaviour
{
    Rigidbody2D LargerBoxrb2D;
    Rigidbody2D BrokenWoodrb2D;
    //Animation animationbox;
    public GameObject BrokenWood;

    // Start is called before the first frame update
    void Start()
    {
        LargerBoxrb2D = GameObject.Find("箱子").GetComponent<Rigidbody2D>();
        BrokenWoodrb2D = GameObject.Find("架子1").GetComponent<Rigidbody2D>();
        //animationbox = GameObject.Find("箱子").GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("isture");
                BrokenWoodrb2D.isKinematic = false;
                BrokenWood.GetComponent<BoxCollider2D>().isTrigger = true;
                Invoke("DropedWood", 1f);
                LargerBoxrb2D.isKinematic = false;
                Invoke("KinematicStart", 0.8f);
            }
        }

    }

    void DropedWood()
    {
        BrokenWoodrb2D.GetComponent<HingeJoint2D>().enabled = false;
    }

    void KinematicStart()
    {
        LargerBoxrb2D.isKinematic = true;
    }
}
