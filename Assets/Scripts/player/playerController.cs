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

    private float leaveGroundTime = 0;
    private float timer = 0;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // ★ HK 风格：开启插值，稳定画面（不影响逻辑）
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        oriPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isGrounded)
            leaveGroundTime = 0;
        else
            leaveGroundTime += Time.deltaTime;

        if (isGrounded)
        {
            movemode = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                ? moveMode.run
                : moveMode.move;

            curSpeed = movemode == moveMode.move ? moveSpeed : RunSpeed;
            rb.gravityScale = 1.2f; // ★ HK：上升期略轻
        }
        else
        {
            curSpeed = movemode == moveMode.move ? moveFloatSpeed : runFloatSpeed;
        }

        // ★ HK：下落更狠，落地更干脆
        if (!Input.GetButton("Jump") && rb.linearVelocity.y <= 0)
            rb.gravityScale = 3.5f;

        // 起跳（保留你原逻辑）
        if (Input.GetButtonDown("Jump") && leaveGroundTime <= 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }

        // ★ HK：Jump Cut（短跳非常关键）
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // ★ 关键：改为 Raw 输入（立刻提升“紧”感）
        if (timer > 0.2f)
            horizontalInput = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("speed", Mathf.Abs(horizontalInput) * curSpeed);
        FlipCharacter();

        anim.SetBool("IsFall", !isGrounded);

        // ===== 横向移动（HK 风格）=====
        float targetX = horizontalInput * curSpeed;

        if (isGrounded)
        {
            // ★ 地面：松手立停（非常 HK）
            if (Mathf.Approximately(horizontalInput, 0f))
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);
        }
        else
        {
            // ★ 空中：允许修正方向，但不瞬移
            float airControl = 0.85f;
            float newX = Mathf.Lerp(rb.linearVelocity.x, targetX, airControl);
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
    }

    void FixedUpdate()
    {
        checkGround();
    }

    void FlipCharacter()
    {
        // ★ 不改 scale，避免物理边缘抖动
        if (horizontalInput > 0)
            sprite.flipX = false;
        else if (horizontalInput < 0)
            sprite.flipX = true;
    }

    void checkGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        
        if (Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                deathLayer
            ))
        {
            GameManager.isDead = true;
        }
    }

    public float t1;
    public void Reset()
    {
        transform.position = oriPos;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        horizontalInput = 0f;
        timer = 0f;

        StartCoroutine(setReset());
    }

    IEnumerator setReset()
    {
        anim.SetBool("Reset", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Reset", false);
    }
}
