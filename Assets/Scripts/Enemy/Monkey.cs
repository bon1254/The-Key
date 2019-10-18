using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour
{
    public Vector2 respawnPoint;
    public Transform target;
    Transform enemyTransform;

    public float speed = 15f;
    public Animator animator;
    bool facingRight = true;

    Rigidbody2D Monekyrb2d;
    public float JumpForce = 700f;
    public bool IsJumping = false;

    void Awake()
    {
        respawnPoint = gameObject.transform.localPosition;
        Debug.Log(respawnPoint);
        enemyTransform = this.GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Monekyrb2d = gameObject.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {

        //计算玩家与敌人的距离
        float distance = Vector2.Distance(transform.position, target.position);

        //玩家与敌人的方向向量
        Vector2 temVec = target.position - transform.position;
        
        if (Mathf.Abs(distance) <= 25)
        {
            //rotate to look at the player 
            if ((facingRight && transform.position.x > target.position.x) || (!facingRight && transform.position.x < target.position.x))
            {
                Flip();
            }
                   
            animator.SetBool("IsRunning", true);
            //move towards the player
            enemyTransform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);           
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.forward = -transform.forward;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.transform.localPosition = respawnPoint;
        }       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "JumpingField")
        {
            if (IsJumping == false)
            {
                IsJumping = true;
                Monekyrb2d.AddForce(new Vector2(0f, JumpForce));
                animator.SetBool("IsJumping", true);
                Debug.Log(IsJumping + "Enter");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "JumpingField")
        {
            if (IsJumping == true)
            {
                IsJumping = false;
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsRunning", true);
                Debug.Log(IsJumping + "Exit");
            }
        }
    }
}
