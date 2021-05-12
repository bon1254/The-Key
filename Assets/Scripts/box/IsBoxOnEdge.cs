using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBoxOnEdge : MonoBehaviour
{
    public bool PlayerPushing = false;
    public bool Falling = false;
    public bool Locked = false;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (PlayerPushing || (Locked && collision.gameObject.tag != "Pushable")) return;
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pushable") && GetComponent<Rigidbody2D>().velocity == Vector2.zero) MakeBoxStatic();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Locked && collision.gameObject.tag != "Pushable") return;
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pushable") MakeBoxFloat();
    }

    public void UseBox()
    {
        PlayerPushing = true; Locked = false;
    }
    public void PutDown()
    {
        PlayerPushing = false;
    }

    public void OnLock() 
    {
        Locked = true;
        MakeBoxStatic();
    }
    public void Unlock()
    {
        Locked = false;
    }

    void MakeBoxFloat()
    {
        Falling = true;
        GetComponent<FixedJoint2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void MakeBoxStatic()
    {
        Falling = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<FixedJoint2D>().enabled = true;
    }
}
