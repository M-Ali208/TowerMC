using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    public bool hit;
    public Vector2Int mousePos;

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
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");
        
        hit= Input.GetMouseButton(0);

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
       
        if(horizontal > 0)
            transform.localScale = new Vector3(-1, 1,1 );
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
            movement.y = jumpForce;
        }
        rb.velocity = movement;
    }
    private void Update()
    {
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool("hit", hit);
    }  

 }
    
