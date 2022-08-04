using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour
{
    public Transform target;
    Transform enemyTransform;    
    Rigidbody2D Monkeyrb2d;
    BoxCollider2D boxCollider2D;
    Monkey monkey;

    float myWidth;
    public float speed = 15f;
    public LayerMask groundLayer;
    public bool IsGrounded = true;
    public bool jumpState = false;

    public Animator animator;
    bool facingRight = true;
    public float JumpForce = 125f;

    void Awake()
    {
        monkey = this.gameObject.GetComponent<Monkey>();
        enemyTransform = gameObject.GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Monkeyrb2d = gameObject.GetComponent<Rigidbody2D>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void Update()
    {
        //计算玩家与敌人的距离
        float distance = Vector2.Distance(transform.position, target.position);

        //玩家与敌人的方向向量
        Vector2 temVec = target.position - transform.position;
     
        if (Mathf.Abs(distance) <= 35)
        {
            //rotate to look at the player 
            if ((facingRight && transform.position.x > target.position.x) || (!facingRight && transform.position.x < target.position.x))
            {
                Flip();
            }
                   
            animator.SetBool("IsRunning", true);

            if (!IsGrounded) return;
            //move towards the player
            enemyTransform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);           
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }   
    }

    void ReEnableCollider() {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void FixedUpdate()
    {
        float Raydistance = 5f;

        Vector2 LineCastPos = enemyTransform.position - (- enemyTransform.right * myWidth * 0.35f);
        Debug.DrawRay(LineCastPos, Vector2.down * Raydistance);
        var HaveGround = Physics2D.Raycast(LineCastPos, Vector2.down * Raydistance, Raydistance, groundLayer);
        if (!HaveGround)
        {
            if (!animator.GetBool("IsJumping") && IsGrounded)
            {
                Invoke("ReEnableCollider", .5f);
                Monkeyrb2d.AddForce(new Vector2(facingRight ? JumpForce * .5f : -JumpForce * .5f, JumpForce), ForceMode2D.Impulse);
                animator.SetTrigger("jumpNow");                          
            }
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
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
            animator.SetBool("IsJumping", false);
            animator.SetTrigger("IsAttacking");
            monkey.GetComponent<Monkey>().enabled = false;
        }      
    }
}
