using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed = 5f;  
    public float jumpForce = 7f;  

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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(horizontalInput));
        FlipCharacter();
        
        if (Input.GetButtonDown("Jump")) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump"); 
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void FlipCharacter()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}