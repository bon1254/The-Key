using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.  
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character    
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
   
    const float k_GroundedRadius = 0.3f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;
    bool wasGrounded;            // Whether or not the player is grounded.
    public Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.

    float jumpOnTime;

    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    bool was_Grounded;
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();         
    }

    public void OnJump()
    {
        jumpOnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - jumpOnTime < 0.15f) return;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
       

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                 m_Grounded = true;
                 if (!wasGrounded)
                 {
                     OnLandEvent.Invoke();
                 }
            }            
        }
    }
    //
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_GroundCheck.position, k_GroundedRadius);
    }
    //
    public void Move(float move_X, float move_Y, bool jump, bool climb)
    {
        if (climb)
        {
            if (m_Rigidbody2D.gravityScale != 0)
            {
                //Debug.LogError("climb");
                m_Rigidbody2D.gravityScale = 0;
                m_Rigidbody2D.velocity = Vector2.zero;
            }
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move_X * 10f, move_Y * 10f);
            // And then smoothing it out and applying it to the character

           //if (targetVelocity.magnitude > 0)
                m_Rigidbody2D.velocity = targetVelocity;
           // else
               // m_Rigidbody2D.velocity = Vector2.zero;
        }
        else
        {        
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl) {

                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move_X * 10f, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                //m_Rigidbody2D.velocity = targetVelocity;

                // If the input is moving the player right and the player is facing left...
                if (move_X > 0 && !m_FacingRight) {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move_X < 0 && m_FacingRight) {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}