using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{   
    [Header("GetComponent")]
    public BoxCollider2D StandUpBoxCollider;
    public CharacterController2D controller;
    public Animator animator;
    Rigidbody2D PlayerRb2d;

    [Header("UI")]
    public Animator animatorUI;
    public GameObject DeathUI;

    [Header("Speed")]
    public float runSpeed;
    public float WalkSpeed;
    public float climbSpeed = 10f;

    float horizontalMove = 0f, VerticalMove = 0f;

    [Header("Push Raycast")]
    public float distance = 0.8f;
    public LayerMask boxMask;

    public Vector3 respawnPoint;

    bool jump = false;
    bool climb = false;

    public GameObject Player;   
    public LevelManager gameLevelManager;

    [Header("State")]
    public bool Run = false;
    public bool onLadder = false;
    public bool Unjumping;
    public bool Death = false;
    public bool PlayerControlable = true;

    //Push
    GameObject Crate = null;
    public bool IsPushing = false;
    public bool DirIsRight;
    public bool DropTheBox = false;
    float NowPushDirection;
    float PlayerPushDirection = 0;
    
    float DeathTime = 0;
    
    void Awake()
    {
        PlayerRb2d = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    void Start()
    {
        respawnPoint = transform.position;
        gameLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {   
        if (!PlayerControlable) return;
       
        ControlRun();

        //旋轉格子(互動)
        if (BlockInTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                block.ChangeCurrentAngle();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetCrate();               
            }
        }

        PushBox();

        animator.SetFloat("Speed", Mathf.Abs(PlayerRb2d.velocity.x));

        if (onLadder)
        {           
            ControlClimb();
        }
        else
        {
            ControlJump();
        }       
    }

    void FixedUpdate()
    {
        if (!PlayerControlable)
            return;
      
        JumpingAniamtion();

        controller.Move(horizontalMove * Time.fixedDeltaTime, VerticalMove * Time.fixedDeltaTime, jump, climb);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animator.SetFloat("New Float", 0.3f);
    }

    void ControlClimb()
    {
        if (!climb && Input.GetButtonDown("Jump"))
        {
            climb = true;
            animator.SetBool("IsClimbing", true);
            animator.SetBool("IsJumping", false);
        }
        VerticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
    }

    public void ControlJump()
    {
        if (!Unjumping && controller.m_Grounded && Input.GetButtonDown("Jump"))
        {
            controller.OnJump();
            jump = true;
            animator.SetBool("IsJumping", true);
        }
    }

    public void JumpingAniamtion()
    {
        if (climb)
            return;

        bool IsJumping = Mathf.Abs(PlayerRb2d.velocity.y) > 0.4f;
        
        if (IsJumping)
        {           
            if (PlayerRb2d.velocity.y > 0)
            {
                animator.SetFloat("New Float", 5f / 12f);
            }
            else if (PlayerRb2d.velocity.y < 0)
            {
                animator.SetFloat("New Float", 7f / 12f);
            }                          
        }
    }

    void ControlRun()
    {
        //Set run
        if (StandUpBoxCollider.enabled)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Run = true;
            }
            else
            {
                Run = false;
            }
        }
        else
        {
            Run = false;
        }

        if (Input.GetButton("Horizontal"))
        {                     
           //Set direction
            float PushDirection = Input.GetAxisRaw("Horizontal");          
            //Set MoveSpeed
            if (Run)
            {
                horizontalMove = PushDirection * runSpeed;
            }
            else
            {
                horizontalMove = PushDirection * WalkSpeed;
            }
        }
        else
            horizontalMove = 0;
    }

    #region 推箱子
    void GetCrate()
    {
        if (IsPushing)
        {
            EndPush();
            return;
        }

        if (Player.transform.localScale.x > 0.01f)
            PlayerPushDirection = 1f;
        if (Player.transform.localScale.x < -0.01f)
            PlayerPushDirection = -1f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right * transform.localScale.x, distance, boxMask);
        if (hit.collider != null && hit.collider.tag == "Pushable")
        {
            Crate = hit.collider.gameObject;
            if (Crate.GetComponent<IsBoxOnEdge>() == null) Crate.AddComponent<IsBoxOnEdge>();

            IsPushing = true;
            Unjumping = true;
            animator.SetBool("IsPushing", true);
            if (Crate.transform.position.x > transform.position.x)
            {
                DirIsRight = true;
            }
            else
            {
                DirIsRight = false;
            }
            Crate.GetComponent<FixedJoint2D>().connectedBody = PlayerRb2d;
            Crate.GetComponent<FixedJoint2D>().enabled = true;
            Crate.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Crate.GetComponent<IsBoxOnEdge>().UseBox();
        }
    }

    public void PushBox()
    {
        if (IsPushing)
        {
            if (Player.transform.localScale.x * PlayerPushDirection < 0)
                EndPush();
        }
    }

    void EndPush()
    {
        IsPushing = false;
        Unjumping = false;
        Crate.GetComponent<IsBoxOnEdge>().PutDown();
        animator.SetBool("IsPushing", false);
        if (Crate != null)
        {
            Crate.GetComponent<FixedJoint2D>().enabled = false;
            Crate.GetComponent<FixedJoint2D>().connectedBody = null;
            if (!Crate.GetComponent<IsBoxOnEdge>().Falling)
            {
                var CrateRb2d = Crate.GetComponent<Rigidbody2D>();
                CrateRb2d.isKinematic = true;
                CrateRb2d.velocity = Vector2.zero;
            }
        }
        print("end");
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + Vector2.right * transform.localScale.x * distance);
    }

    #region 碰撞器
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if (!Death)
            {
                Death = false;
                PlayerControlable = false;
                animator.SetTrigger("Death");
                DeathUI.SetActive(true);
                animatorUI.Play("DeathStart");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;
        }

        if (other.tag == "Spike" && PlayerControlable)
        {
            DeathTime = Time.time;
            PlayerControlable = false;
            animator.SetLayerWeight(1, 1.0f);//Death Layer
        }

        if (other.tag == "Ladder")
        {
            onLadder = true;
        }
        if (other.tag == "NextLevel")
        {
            enabled = false;
            animator.SetFloat("Speed", 0);
            PlayerRb2d.velocity = Vector2.zero;
        }

        if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            onLadder = false;
            if (climb)
            {
                climb = false;
                animator.SetBool("IsClimbing", false);
                PlayerRb2d.gravityScale = 5.2f;
            }
        }

        if (other.tag == "blocks")
        {
            BlockInTrigger = false;
        }
    }

    public BlockRotationRandom block;
    public bool BlockInTrigger = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "blocks")
        {
            block = other.GetComponent<BlockRotationRandom>();
            BlockInTrigger = true;
            Debug.Log("123");
        }
    }
    #endregion
}
