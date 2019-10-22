using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public bool PlayerControlable = true;

    public CharacterController2D controller;
    public Animator animator;
    public Vector3 respawnPoint;
    public LevelManager gameLevelManager;
    public BoxCollider2D StandUpBoxCollider;


    public float runSpeed;
    public float WalkSpeed;
    public float climbSpeed = 10f;

    float horizontalMove;
    float VerticalMove;

    public float distance = 0.75f;
    public LayerMask boxMask;

    GameObject Crate = null;
    Rigidbody2D CrateRb2d;
    bool IsPushing = false;
    float NowPushDirection;
    bool DirIsRight;

    public bool CanRotated = false;
    public bool IsHitting = true;
    public bool onLadder = false;
    public bool jump = false;
    public bool Unjumging;
    public bool Death = false;
    public bool Push = false;
    public bool Run = false;

    float DeathTime = 0;

    void Start()
    {
        respawnPoint = transform.position;     
        gameLevelManager = FindObjectOfType<LevelManager>();
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void DoJump(bool jumpStatus)
    {        
        animator.SetBool("IsJumping", jumpStatus);
    }

    public void OnClimbing()
    {
        animator.SetBool("IsClimbing", true);
    }

    public void Die()
    {
        animator.SetBool("Death", true);
    }

    public void OnPushing()
    {
        animator.SetBool("IsPushing", true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !Unjumging)
        {
            if (!onLadder)
            {
                jump = true;
            }
        }

        if (StandUpBoxCollider.enabled)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Run = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Run = false;
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Run = true;
            }
            else if (Input.GetKeyUp(KeyCode.RightShift))
            {
                Run = false;
            }
        }
        else
        {
            Run = false;
        }

        if (Run)
        {
            NowPushDirection = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        else
        {
            NowPushDirection = Input.GetAxisRaw("Horizontal") * WalkSpeed;
        }

        if (BlockInTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                block.ChangeCurrentAngle();//旋轉圖片               
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            IsPushing = !IsPushing;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        VerticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
    }

    void FixedUpdate()
    {
        if (PlayerControlable)
        {            
            //Push box direction 
            if (IsPushing)
            {
                if(DirIsRight)
                {
                    if(NowPushDirection < 0)
                    {
                        if (NowPushDirection < horizontalMove)
                        {
                            IsPushing = false;

                            Crate.GetComponent<FixedJoint2D>().connectedBody = null;
                            Crate.GetComponent<FixedJoint2D>().enabled = false;
                            Unjumging = false;
                            CrateRb2d.isKinematic = true;
                            CrateRb2d.velocity = Vector2.zero;
                            Debug.Log("NoPush");
                            animator.SetBool("IsPushing", false);
                        }
                    }
                }
                else
                {
                    if (NowPushDirection > 0)
                    {
                        if (NowPushDirection > horizontalMove)
                        {
                            IsPushing = false;

                            Crate.GetComponent<FixedJoint2D>().connectedBody = null;
                            Crate.GetComponent<FixedJoint2D>().enabled = false;
                            Unjumging = false;
                            CrateRb2d.isKinematic = true;
                            CrateRb2d.velocity = Vector2.zero;
                            Debug.Log("NoPush");
                            animator.SetBool("IsPushing", false);
                        }
                    }
                }
            }
            horizontalMove = NowPushDirection;           

            //Pull and push box
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x , transform.position.y), Vector2.right * transform.localScale.x, distance, boxMask);

          
            if (hit.collider != null && hit.collider.gameObject.tag == "Pushable")
            {                               
                if (IsPushing)
                {
                    if(IsHitting)
                    {
                        animator.SetBool("IsPushing", true);
                        Crate = hit.collider.gameObject;
                        if(Crate.transform.position.x > transform.position.x)
                        {
                            DirIsRight = true;
                        }
                        else
                        {
                            DirIsRight = false;
                        }
                        CrateRb2d = Crate.GetComponent<Rigidbody2D>();
                        Crate.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                        Crate.GetComponent<FixedJoint2D>().enabled = true;
                        Unjumging = true;
                        CrateRb2d.isKinematic = false;
                        IsHitting = false;
                    }
                }
                else
                {
                    animator.SetBool("IsPushing", false);
                    Crate.GetComponent<FixedJoint2D>().connectedBody = null;
                    Crate.GetComponent<FixedJoint2D>().enabled = false;
                    Unjumging = false;
                    CrateRb2d.isKinematic = true;
                    CrateRb2d.velocity = Vector2.zero;
                }                
            }          
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, VerticalMove * Time.fixedDeltaTime, jump, onLadder);
            jump = false;
        }
        else
        {
            controller.Move(0 ,0, false, false);
            if (animator.GetBool("Death"))
            {
                animator.SetBool("Death", false);
            }
            if (Time.time - DeathTime >= .75f)
            {
                PlayerControlable = true;
                gameObject.SetActive(false);
                gameLevelManager.Respawn();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , transform.position.y) + Vector2.right * transform.localScale.x * distance);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FallDetector")
        {
            Debug.Log("Fall");
            gameLevelManager.Respawn();
        }

        if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;
        }

        if (other.tag == "Spike" && PlayerControlable)
        {
            DeathTime = Time.time;
            PlayerControlable = false;
            animator.SetBool("Death", true);
        }

        if (other.tag == "Ladder")
        {
            onLadder = true;
            animator.SetBool("IsClimbing", onLadder);
            animator.SetBool("IsJumping", false);
        }

        if (other.tag == "FinishPushing")
        {
            if(IsPushing == true)
            {
            Crate.GetComponent<FixedJoint2D>().enabled = false;
            CrateRb2d.isKinematic = false;
            animator.SetBool("IsPushing", false);
            Debug.Log("Cut");
            }
        }

        if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;
        }       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            gameObject.SetActive(false);
            gameLevelManager.Respawn();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder") {
            onLadder = false;
            animator.SetBool("IsClimbing", onLadder);    
        }

        if (other.tag == "blocks")
        {
            BlockInTrigger = false;
        }      
    }

    public BlockRotationRandom block;
    public bool BlockInTrigger = false;

    void OnTriggerStay2D(Collider2D other) //Player腳本的
    {
        if (other.tag == "Ladder") {
            onLadder = true;
            if (Input.GetButton("Jump")) {
                animator.SetBool("IsClimbing", onLadder);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsCrouching", false);
            }
        }

        if (other.tag == "blocks")
        {
            block = other.GetComponent<BlockRotationRandom>();
            BlockInTrigger = true;
            Debug.Log("123");           
        }
        
    }


}