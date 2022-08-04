using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
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
    public float WalkSpeed;
    public float runSpeed = 40f;
	public float climbSpeed = 10f;

	float horizontalMove = 0f, VerticalMove = 0f;

    [Header("Push Raycast")]
    public float distance = 0.75f;
    public LayerMask boxMask;

    public Vector3 respawnPoint;

    bool jump = false;
	bool climb = false;

    //state
    bool run = false;
    bool onLadder = false;
	bool Unjumping = false;
    bool Death = false;
    bool PlayerControlable = true;

    //Push
    GameObject Crate = null;
    bool IsPushing = false;
    bool DirIsRight = false;
    bool DropTheBox = false;
    float PlayerPushDirection = 0;
    float DeathTime = 0;

    private void Awake()
    {
        PlayerRb2d = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
	{
        if (!PlayerControlable)
            return;

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
                //IsPushing = !IsPushing;
            }
        }


        //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(PlayerRb2d.velocity.x));

		/*if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}*/
		//
		if (onLadder)
		{
			/*if (Input.GetButtonDown("Crouch"))
			{
				crouch = true;
			}
			else if (Input.GetButtonUp("Crouch"))
			{
				crouch = false;
			}*/
			ControlClimb();
		}
		else
		{
			ControlJump();
		}
        //
        //if (animator.GetBool("IsJumping")) Debug.LogError("true");
	}

    void FixedUpdate()
	{
        if (!PlayerControlable)
            return;

        /*if (PlayerRb2d.velocity.y == 0)
        {
            Debug.LogError(PlayerRb2d.velocity.y);
            was_Grounded = m_Grounded;
            m_Grounded = true;
        }
        if (!was_Grounded)
            OnLanding();*/
        // Move our character
        JumpingAniamtion();

        controller.Move(horizontalMove * Time.fixedDeltaTime, VerticalMove * Time.fixedDeltaTime, jump, climb);
		jump = false;
	}

    public void JumpingAniamtion()
    {
        if (climb)
            return;
        bool IsJumping = Mathf.Abs(PlayerRb2d.velocity.y) > 0.5f;
        if (IsJumping)
        {
            //float normalizeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //Debug.LogError(normalizeTime);
            //if (normalizeTime >= 0.5f)
            {
                if (PlayerRb2d.velocity.y > 0)
                {
                    animator.SetFloat("New Float", 5f / 12f);
                    //animator.Play("Player_Jump", 0, 2f / 12f);
                    //animator.Update(5f / 12f);
                    //Debug.LogError(PlayerRb2d.velocity.y);
                }
                else if (PlayerRb2d.velocity.y < 0)
                {
                    animator.SetFloat("New Float", 7f / 12f);
                    //animator.Play("Player_Jump", 0, 7f / 12f);
                    //animator.Update(7f / 12f);
                    //Debug.LogError(PlayerRb2d.velocity.y);
                }
                /*else
                {
                    //animator.SetFloat("New Float", 1);
                    animator.Play("Player_Jump", 0, 1);
                    //animator.SetBool("IsJumping", false);
                }*/
            }
        }
        /*bool IsJumping = animator.GetBool("IsJumping")/*Mathf.Abs(PlayerRb2d.velocity.y) > 0.1f;
        Debug.LogError("IsJumping :" + IsJumping);
        if (IsJumping)
        {
            var normalizeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //Debug.LogError(normalizeTime);
            if (normalizeTime >= 0.5f)
            {
               
                if (PlayerRb2d.velocity.y > 0)
                {
                    //animator.SetFloat("New Float", 5.5f / 10f);
                    animator.Play("Player_Jump", 0, 5.5f / 10f);
                    Debug.LogError(PlayerRb2d.velocity.y);
                }
                else if (PlayerRb2d.velocity.y < 0)
                {
                    //animator.SetFloat("New Float", 7f / 10f);
                    animator.Play("Player_Jump", 0, .7f / 10f);
                    Debug.LogError(PlayerRb2d.velocity.y);
                }
                else
                {
                    //animator.SetFloat("New Float", 1);
                    animator.Play("Player_Jump", 0, 1);
                    //animator.SetBool("IsJumping", false);
                }
            }
        }*/

    }

    void ControlClimb()
	{
		if (!climb && Input.GetButtonDown("Jump"))
		{
			climb = true;
			animator.SetBool("Isclimb", true);
			animator.SetBool("IsJumping", false);
            //Debug.LogError(animator.GetBool("IsJumping"));
		}
		VerticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
	}

	public void ControlJump()
	{
		if (!Unjumping && controller.m_Grounded && Input.GetButtonDown("Jump"))
		{
            //Debug.LogError("IsJumping = true");
			jump = true;
            //controller.m_Grounded = false;
            animator.SetBool("IsJumping", true);
		}
	}

	public void OnLanding()
	{
        //Debug.LogError("OnLanding");
		animator.SetBool("IsJumping", false);
        animator.SetFloat("New Float", 0.3f);
    }

	/*public void OnCrouching(bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}*/

    void ControlRun()
    {
        //Set run
        if (StandUpBoxCollider.enabled)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                run = true;
            }
            else
            {
                run = false;
            }
        }
        else
        {
            run = false;
        }
        
        if (Input.GetButton("Horizontal"))
        {
            //Set direction
            float PushDirection = Input.GetAxisRaw("Horizontal");
            if (PushDirection > 0.15f) PlayerPushDirection = 1;
            if (PushDirection < -0.15f) PlayerPushDirection = -1;
            //Set MoveSpeed
            if (run)
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
            if (DirIsRight)
            {
                if (PlayerPushDirection < horizontalMove)   //反向走掉 箱子回歸原本狀態
                {
                    EndPush();
                }
                /*if (PlayerPushDirection < 0)
                {
                }*/
            }
            else
            {
                if (PlayerPushDirection > horizontalMove)
                {
                    EndPush();
                    DropTheBox = true;              //如果碰到FinishingPush 要讓箱子掉下來                            
                }
                /*if (PlayerPushDirection > 0)    //防止拉著箱子
                {
                */
            }
        }
    }
   
    void EndPush()
    {
        IsPushing = false;
        Unjumping = false;
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
    }
    #endregion

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
        if (other.tag == "FallDetector")
        {
            PlayerControlable = false;
            animator.SetTrigger("Death");
            DeathUI.SetActive(true);
            animatorUI.Play("DeathStart");
        }

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
                animator.SetBool("Isclimb", false);
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