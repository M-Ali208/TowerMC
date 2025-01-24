using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private bool hit;
    private bool jump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = false;
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");

    
        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1); 
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1); 

       
        if (jumpInput > 0.1f && onGround)
        {
            movement.y = jumpForce;
            jump = true;
        }
        else if (onGround)
        {
            jump = false;
        }

        rb.velocity = movement;
    }

    private void Update()
    {
        
        anim.SetFloat("Horizontal", Mathf.Abs(horizontal)); 
        anim.SetBool("hit", Input.GetMouseButton(0));
        anim.SetBool("jump", jump);
    }
}

