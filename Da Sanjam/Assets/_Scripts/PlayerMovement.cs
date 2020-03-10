using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    float jumpVel = 8f;

    [SerializeField]
    [Range(0, 5)]
    float gravExtra = 2.5f;

    private bool grounded;
    private bool facingRight = true;

    float groundedRemember = 0;
    [SerializeField]
    float groundedRememberTime = .25f;

    float jumpPressedRemember = 0;
    [SerializeField]
    float jumpPressedRememberTime = .2f;

    [SerializeField]
    [Range(0, 1)]
    float cutJumpHeight = 0.5f;
    bool jumpButtonUp = false;

    private float Xdir;
    
    [SerializeField]
    [Range(0, 1)]
    float xVelDampBasic = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float xVelDampWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float xVelDampWhenTurning = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float xVelDampStopWhenInAir = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float xVelDampTurnWhenInAir = 0.5f;

    [SerializeField]
    private LayerMask lmGround;
    
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //check if grounded
        CheckIfGrounded();
        
        //Get movement speed and direction
        Xdir = Input.GetAxisRaw("Horizontal");

        //checks and remembers if player was grounded
        groundedRemember -= Time.deltaTime;
        if (grounded)
        {
            groundedRemember = groundedRememberTime;
            jumpButtonUp = false;
        }

        //checks and remembers if a button gets pressed
        jumpPressedRemember -= Time.deltaTime;
        if(Input.GetButtonDown("Jump"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpButtonUp = true;
        }
    }

    private void FixedUpdate()
    {
        CalculateMovement(Xdir);

        Flip(Xdir);

        CalculateJumping(groundedRemember, jumpPressedRemember);


        //falling a little faster
        if (rb.velocity.y < 0)
        {
            Vector2 fasterFall = (Vector2.up * (Physics2D.gravity.y * gravExtra)) * Time.deltaTime;
            rb.velocity += fasterFall;
        }
    }

    private void CheckIfGrounded()
    {
        //Using overlapbox to check if the player hit the ground. I'm using a layer mask
        //to filter out the rest of the objects in the scene
        Vector2 groundedBoxCheckPos = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 groundedBoxSize = (Vector2)transform.localScale + new Vector2(-0.02f, 0);
        grounded = Physics2D.OverlapBox(groundedBoxCheckPos, groundedBoxSize, 0, lmGround);
    }

    private void CalculateJumping(float ground, float jumpPress)
    {
        if ((ground > 0) && (jumpPress > 0))
        {
            groundedRemember = 0;
            jumpPressedRemember = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpVel);
        }

        if(jumpButtonUp)
        {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutJumpHeight);
        }
    }

    private void CalculateMovement(float mXdir)
    {
        float xVel = rb.velocity.x;
        xVel += Xdir;

        if (Mathf.Abs(Xdir) < 0.01f && grounded)
            xVel *= Mathf.Pow(1f - xVelDampWhenStopping, Time.deltaTime * 10f);      
        else if ((Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(xVel)) && grounded)
            xVel *= Mathf.Pow(1f - xVelDampWhenTurning, Time.deltaTime * 10f);
        else if((Mathf.Abs(Xdir) < 0.01f) && !grounded)
            xVel *= Mathf.Pow(1f - xVelDampStopWhenInAir, Time.deltaTime * 10f);
        else if ((Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(xVel)) && !grounded)
            xVel *= Mathf.Pow(1f - xVelDampTurnWhenInAir, Time.deltaTime * 10f);
        else
            xVel *= Mathf.Pow(1f - xVelDampBasic, Time.deltaTime * 10f);

        //sets xVel to zero if number gets too small
        if ((xVel < 0.001f) && (xVel > -0.001f))
            xVel = 0;

        Flip(xVel);
        rb.velocity = new Vector2(xVel, rb.velocity.y);

    }

    private void Flip(float xDirection)
    {
        if(xDirection > 0 && !facingRight || xDirection < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
