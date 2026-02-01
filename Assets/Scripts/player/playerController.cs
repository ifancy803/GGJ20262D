using System;
using UnityEngine;

public class playerController : Singleton<playerController>
{
    private Vector3 oriPos;
    
    public float moveSpeed;  
    public float RunSpeed;  
    public float floatSpeed;
    public float curSpeed;  
    public float jumpForce;  
    
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer, deathLayer;
    
    private Rigidbody2D rb;   
    private Animator anim;   
    private SpriteRenderer sprite;

    private float horizontalInput;

    private float leaveGroundtTime=0;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        oriPos = transform.position;
    }

    void Update()
    {
        if (isGrounded)
        {
            leaveGroundtTime = 0;
        }
        else
        {
            leaveGroundtTime += Time.deltaTime;
        }
        if (isGrounded)
        {
            curSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? RunSpeed : moveSpeed;
            rb.gravityScale = 1f;
        }
        else
        {
            curSpeed = floatSpeed;
        }
        
        if (Input.GetButton("Jump"))
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 2f;
        }
        

        if (Input.GetButtonDown("Jump") && leaveGroundtTime<=0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }


        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(horizontalInput)*curSpeed);
        FlipCharacter();
        
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
        
        if(Physics2D.OverlapCircle(
               groundCheck.position,
               groundCheckRadius,
               deathLayer
               )
           )
        {
            GameManager.isDead = true;
        }
    }

    public void Reset()
    {
        Debug.Log(oriPos);
        // 使用 Rigidbody2D 的 MovePosition 方法
        rb.MovePosition(oriPos);
    
        // 同时重置速度，避免惯性影响
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}