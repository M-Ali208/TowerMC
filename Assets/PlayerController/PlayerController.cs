using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    //public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    public bool hit;

    public Vector3Int mousePos;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask MouseLayer;
    [SerializeField] private Transform groundCheck;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private bool onGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        //groundchek player de�il player�n alt�ndaki block�un parent�n�n layer�na bak�lacak
    }
    private void mouseBlockCheck()
    {
        Physics.Raycast(mousePos,Vector3.back, 1,MouseLayer);
    }

    private void FixedUpdate()
    {
       horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        
        hit= Input.GetMouseButton(0);
        if (hit)
        {
            
        }

    
        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1); 
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1); 

       
        if (jumpInput > 0.1f && onGround)
        {
            if (onGrounded())
            movement.y = jumpForce;
            jump = true;
        }
        else if (onGround)
        {
            jump = false;
        }

        rb.velocity = movement;

        Debug.Log(Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer));
    }

    private void Update()
    {
        
        anim.SetFloat("Horizontal", Mathf.Abs(horizontal)); 
        anim.SetBool("hit", Input.GetMouseButton(0));
        anim.SetBool("jump", jump);
    }
}

        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool("hit", hit);
    }  
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
    //Debug.Log(gameObjectYouWantTheParentOf.transform.parent.name);

 }
    
 