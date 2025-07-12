using UnityEngine;

public class FlyingAbility : MonoBehaviour
{
    private bool hasFlying = false;
    private bool isFlying = false;

    [SerializeField] private float longJumpMultiplier = 1.5f;
    [SerializeField] private float longJumpGravityScale = 0.4f;

    private float originalGravity;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!hasFlying) return;

        if (Input.GetKeyDown(KeyCode.F) && !IsGrounded() && !isFlying)
        {
            StartFlying();
        }

        if (IsGrounded() && isFlying)
        {
            EndFlying();
        }
    }

    private void StartFlying()
    {
        isFlying = true;
        rb.gravityScale = longJumpGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (rb.velocity.y > 0 ? 0 : 1f) + 40f * longJumpMultiplier);
        //animator.SetTrigger("Jump");
        //animator.SetTrigger("OneJump");
        animator.SetBool("IsFlying", true);
    }

    private void EndFlying()
    {
        isFlying = false;
        rb.gravityScale = originalGravity;
        animator.SetBool("IsFlying", false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Butterfly"))
        {
            hasFlying = true;
            Destroy(other.gameObject);
            Debug.Log("🦋 Flying ability unlocked!");
        }
    }
}
