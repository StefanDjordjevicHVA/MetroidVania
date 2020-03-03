using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpVel;

    private bool grounded;
    private bool jumped = false;

    public float gravExtra;
    
    private float Xdir;

    [SerializeField]
    LayerMask lmGround;
    
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Xdir = Input.GetAxisRaw("Horizontal") * speed;

        if(Input.GetButtonDown("Jump") && grounded)
        {
            jumped = true;
        }
    }

    private void FixedUpdate()
    {
        //Using overlapbox to check if the player hit the ground. I'm using a layer mask
        //to filter out the rest of the objects in the scene
        Vector2 groundedBoxCheckPos = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 groundedBoxSize = (Vector2)transform.localScale + new Vector2(-0.02f, 0);
        grounded = Physics2D.OverlapBox(groundedBoxCheckPos, groundedBoxSize, 0, lmGround);

        rb.velocity = new Vector2(Xdir, rb.velocity.y);

        //TODO: add timer for more responsiveness
        if (jumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVel);
            jumped = false;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * gravExtra * Time.deltaTime;
        }
    }
}
