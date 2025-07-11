using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 40f;
    private bool isFacingRight = true;

    private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Variables for double-click long jump
    private float lastSpacePressTime;
    private const float doubleClickTimeThreshold = 0.3f; // Time in seconds to detect a double-click
    private bool isLongJumping = false;
    [SerializeField] private float longJumpingPowerMultiplier = 1f; // How much higher the long jump is
    [SerializeField] private float longJumpGravityScale = 0.5f; // Reduced gravity for slow descent
    private float originalGravityScale; // To store the Rigidbody\'s original gravity scale

    void Awake()
    {
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale; // Store original gravity scale
    }

  void Update()
{
    horizontal = Input.GetAxisRaw("Horizontal");
    animator.SetFloat("Speed", Mathf.Abs(horizontal));

    // Handle Jump (Space) - only if grounded
    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        animator.SetTrigger("Jump");
        animator.SetTrigger("OneJump");
        animator.SetBool("IsFlying", true);
    }

    // Handle Fly (F key) - only in air and not already flying
    if (Input.GetKeyDown(KeyCode.F) && !IsGrounded() && !isLongJumping)
    {
        StartLongJump();
    }

    // Cut jump height if Space is released early (optional)
    if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && !isLongJumping)
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    // Reset states on landing
    if (IsGrounded())
    {
        animator.SetBool("IsFlying", false);
        animator.ResetTrigger("OneJump");
        if (isLongJumping)
        {
            EndLongJump();
        }
    }

    Flip();
}


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void StartLongJump()
    {
        isLongJumping = true;
        rb.gravityScale = longJumpGravityScale; // Apply reduced gravity for slow descent
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower * longJumpingPowerMultiplier); // Higher jump
        animator.SetTrigger("Jump"); // Trigger Jump animation
        animator.SetTrigger("OneJump"); // Trigger OneJump for long jump
        animator.SetBool("IsFlying", true); // Set isFlying to true for airborne state
    }

    private void EndLongJump()
    {
        isLongJumping = false;
        rb.gravityScale = originalGravityScale; // Restore original gravity
    }
}


