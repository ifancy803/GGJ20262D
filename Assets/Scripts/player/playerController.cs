using UnityEngine;

public class playerController : MonoBehaviour
{
    
    public float moveSpeed;  
    public float RunSpeed;  
    public float floatSpeed;
    public float curSpeed;  
    public float jumpForce;  
    
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;   
    private Animator anim;   
    private SpriteRenderer sprite;

    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isGrounded)
        {
            curSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? RunSpeed : moveSpeed;
            rb.gravityScale = 1;
        }
        else
        {
            curSpeed = floatSpeed;
        }

        bool firstReleaseSpace = false;
        if (!isGrounded && !firstReleaseSpace && Input.GetButtonUp("Jump"))
        {
            firstReleaseSpace = true;
            rb.gravityScale = 3;
        }
        
        
        
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(horizontalInput)*curSpeed);
        FlipCharacter();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            firstReleaseSpace = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }


        anim.SetBool("IsFall",!isGrounded);

        
    }

    void FixedUpdate()
    {
        checkGround();
        rb.linearVelocity = new Vector2(horizontalInput * curSpeed, rb.linearVelocity.y);
    }

    void FlipCharacter()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
    }

    void checkGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
    
}