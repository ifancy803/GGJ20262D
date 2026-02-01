using System;
using UnityEngine;

public class playerController : Singleton<playerController>
{
    private Transform oriTransform = null;
    
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
    private bool firstReleaseSpace=false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        oriTransform = transform;
    }
    

    void Update()
    {
        if (isGrounded)
        {
            curSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? RunSpeed : moveSpeed;
            rb.gravityScale = 1f;
        }
        else
        {
            curSpeed = floatSpeed;
        }
        
        if (!isGrounded && !firstReleaseSpace && Input.GetButtonUp("Jump"))
        {
            firstReleaseSpace = true;
            rb.gravityScale = 2f;
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

    public void Reset()
    {
        if (oriTransform == null)
        {
            Debug.LogError("oriTransform is not initialized.");
            return;
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not initialized.");
            return;
        }
        // 使用 Rigidbody2D 的 MovePosition 方法
        rb.MovePosition(oriTransform.position);
    
        // 同时重置速度，避免惯性影响
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}