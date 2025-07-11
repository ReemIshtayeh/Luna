using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ───────── Movement & jump ─────────
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 40f;

    // ───────── Long-jump ability ─────────
    [Header("Long-Jump")]
    private bool hasLongJump = false;
    private bool isLongJumping = false;
    [SerializeField] private float longJumpingPowerMultiplier = 1.2f;
    [SerializeField] private float longJumpGravityScale = 0.4f;
    private float originalGravityScale;

    // ───────── Components & helpers ─────────
    private bool isFacingRight = true;
    private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // ───────────────────────────────────────
    private void Awake()
    {
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale;
    }

   private void Update()
{
    horizontal = Input.GetAxisRaw("Horizontal");
    animator.SetFloat("Speed", Mathf.Abs(horizontal));

    // Jump (space)
    if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
    {
        Jump();
    }

    // Fly (F key)
    if (hasLongJump && Input.GetKeyDown(KeyCode.F) && !IsGrounded() && !isLongJumping)
    {
        Debug.Log("✈ Fly triggered");
        StartLongJump();
    }

    // Cut jump if released early
    if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && !isLongJumping)
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    // Reset when landed
    if (IsGrounded())
    {
        animator.SetBool("IsFlying", false);
        animator.ResetTrigger("OneJump");
        if (isLongJumping) EndLongJump();
    }

    Flip();
}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        animator.SetTrigger("Jump");
        animator.SetTrigger("OneJump");
        animator.SetBool("IsFlying", true);
    }

    private void StartLongJump()
    {
        isLongJumping = true;
        rb.gravityScale = longJumpGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower * longJumpingPowerMultiplier);
        animator.SetTrigger("Jump");
        animator.SetTrigger("OneJump");
        animator.SetBool("IsFlying", true);
        Debug.Log("🦋 Long Jump Activated!");
    }

    private void EndLongJump()
    {
        isLongJumping = false;
        rb.gravityScale = originalGravityScale;
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
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    // ─── Unlock flying by collecting butterfly ───
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Butterfly"))
        {
            hasLongJump = true;
            Destroy(other.gameObject);
            Debug.Log("🦋 Long Jump Unlocked!");
            // Optional: play sound, VFX, or show UI here
        }
    }
}