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
    }

    private void Start()
    {
        oriPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ===== 离地宽限（你原逻辑，保留）=====
        if (isGrounded)
            leaveGroundTime = 0;
        else
            leaveGroundTime += Time.deltaTime;

        // ===== 地面 / 空中速度模式（原逻辑）=====
        if (isGrounded)
        {
            movemode = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                ? moveMode.run
                : moveMode.move;

            curSpeed = movemode == moveMode.move ? moveSpeed : RunSpeed;
            rb.gravityScale = 1f;
        }
        else
        {
            curSpeed = movemode == moveMode.move ? moveFloatSpeed : runFloatSpeed;
        }

        // ===== 重力控制（原逻辑）=====
        if (Input.GetButton("Jump"))
            rb.gravityScale = 1f;
        else
            rb.gravityScale = 2f;

        // ===== 起跳（原逻辑）=====
        if (Input.GetButtonDown("Jump") && leaveGroundTime <= 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }

        // ===== ★ 新增：Jump Cut（短跳关键）=====
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
        }

        // ===== ★ 改动：Raw 输入（更“紧”）=====
        if (timer > 0.2f)
            horizontalInput = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("speed", Mathf.Abs(horizontalInput) * curSpeed);
        FlipCharacter();
        anim.SetBool("IsFall", !isGrounded);

        // ===== ★ 横向移动：HK 风格 =====
        float targetX = horizontalInput * curSpeed;

        if (isGrounded)
        {
            // 地面松手立停（HK 核心）
            if (Mathf.Approximately(horizontalInput, 0f))
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);
        }
        else
        {
            // 空中：轻微修正，不飘
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
        // ★ 不动 scale 结构，只改数值（保持你原用法）
        if (horizontalInput > 0)
            transform.localScale = new Vector3(2, 2, 2);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-2, 2, 2);
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
        StartCoroutine(setReset());

        transform.position = oriPos;
        transform.localScale = new Vector3(2, 2, 2);

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
