using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour
{
    public Vector2 respawnPoint;
    public Transform target;
    Transform enemyTransform;    
    Rigidbody2D Monekyrb2d;
    BoxCollider2D boxCollider2D;

    float myWidth;
    public float speed = 15f;
    public LayerMask groundLayer;
    public bool IsGrounded;
    public bool IsBlocked;

    public Animator animator;
    bool facingRight = true;
    public float JumpForce = 700f;

    void Awake()
    {
        respawnPoint = gameObject.transform.localPosition;
        enemyTransform = gameObject.GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Monekyrb2d = gameObject.GetComponent<Rigidbody2D>();
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

      
        Debug.Log(IsGrounded);       
    }

    void FixedUpdate()
    {
        Vector2 LineCastPos = enemyTransform.position - (- enemyTransform.right * myWidth * 0.25f);
        Debug.DrawLine(LineCastPos, LineCastPos + Vector2.down * 3.2f);
        IsGrounded = Physics2D.Linecast(LineCastPos, LineCastPos + Vector2.down, groundLayer);
        //IsBlocked = Physics2D.Linecast(LineCastPos, LineCastPos - enemyTransform. , groundLayer);
        if (!IsGrounded)
        {
            //Monekyrb2d.AddForce(new Vector2(0, JumpForce));
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
}
