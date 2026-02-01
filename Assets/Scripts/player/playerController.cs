using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class playerController : Singleton<playerController>
{
    private Vector3 oriPos;
    
    public float moveSpeed;  
    public float RunSpeed;  
    public float runFloatSpeed;
    public float moveFloatSpeed;
    public float curSpeed;  
    public float jumpForce;

    enum moveMode
    {
        run,
        move
    }

    private moveMode movemode;
    
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer, deathLayer;
    
    private Rigidbody2D rb;   
    private Animator anim;   
    private SpriteRenderer sprite;

    private float horizontalInput;

    private float leaveGroundTime=0;
    private float timer = 0;

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
        timer += Time.deltaTime;
        if (isGrounded)
        {
            leaveGroundTime = 0;
        }
        else
        {
            leaveGroundTime += Time.deltaTime;
        }
        if (isGrounded)
        {
            movemode= (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? moveMode.run:moveMode.move;
            curSpeed = movemode == moveMode.move ? moveSpeed : RunSpeed;
            rb.gravityScale = 1f;
        }
        else
        {
            curSpeed = movemode==moveMode.move? moveFloatSpeed:runFloatSpeed;
        }
        
        if (Input.GetButton("Jump"))
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 2f;
        }
        

        if (Input.GetButtonDown("Jump") && leaveGroundTime<=0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }

        if(timer>0.2f)
            horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(horizontalInput)*curSpeed);
        FlipCharacter();
        
        anim.SetBool("IsFall",!isGrounded);
        
        rb.linearVelocity = new Vector2(horizontalInput * curSpeed, rb.linearVelocity.y);

        
    }

    void FixedUpdate()
    {
        checkGround();
        
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

    public float t1;
    public void Reset()
    {
        Debug.Log(oriPos);

        StartCoroutine(setReset());
        
        //transform.DOLocalMove(oriPos, t1).SetEase(Ease.OutCubic);
        //rb.MovePosition(oriPos);
        transform.position = oriPos;
        transform.localScale = new Vector3(2, 2, 2);
    
        // 同时重置速度，避免惯性影响
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        horizontalInput = 0f;
        timer = 0f;

        
    }

    IEnumerator setReset()
    {
        anim.SetBool("Reset", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Reset", false);
    }
}