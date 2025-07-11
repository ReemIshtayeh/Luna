using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movementInput; // Stores the raw input from the player controls
    private bool isGrounded;
    private bool hasDoubleJumped = false;
    private bool isFlying = false;

    private float doubleTapTime;
    private float tapDelay = 0.3f; // Time window for double tap
    private int spaceTapCount = 0;

    void Awake()
    {
        // Initialize player controls, Rigidbody2D, and Animator components
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Enable player controls and subscribe to the Jump action
        playerControls.Enable();
        playerControls.Movement.Jump.performed += ctx => OnJumpPressed();
    }

    void OnDisable()
    {
        // Disable player controls and unsubscribe from the Jump action
        playerControls.Movement.Jump.performed -= ctx => OnJumpPressed();
        playerControls.Disable();
    }

    void Update()
    {
        // Read player input for movement
        PlayerInput();

        // Update animator's "Speed" parameter for Idle/Walk animations
        // Use the magnitude of the movement vector to determine if the character is moving
        animator.SetFloat("Speed", movementInput.magnitude);

        // Ground check using an OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Reset double jump capability when grounded
        if (isGrounded)
        {
            hasDoubleJumped = false;
            // If grounded, ensure 'Jump' animation is false
            animator.SetBool("Jump", false);
        }

        // Animation logic for jumping
        // Set "Jump" to true if moving upwards and not grounded
        if (rb.velocity.y > 0.1f && !isGrounded)
        {
            animator.SetBool("Jump", true);
        }
        else if (isGrounded) // If grounded, ensure jump animation is off
        {
            animator.SetBool("Jump", false);
        }
        // Note: No specific "IsFalling" logic as per your Animator setup.
        // You might need to handle falling animation transitions directly in the Animator
        // from your 'Jump' state or by adding a new parameter if needed later.
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for consistent physics
        Move();

        // Handle flying mode movement
        if (isFlying)
        {
            // In flying mode, allow continuous vertical movement while Jump is pressed
            if (playerControls.Movement.Jump.IsPressed())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            // Disable gravity while flying
            rb.gravityScale = 0f;
        }
        else
        {
            // Re-enable gravity when not flying
            rb.gravityScale = 1f; // Or your default gravity scale
        }
    }

    private void PlayerInput()
    {
        // Read the movement input from the PlayerControls
        movementInput = playerControls.Movement.Move.ReadValue<Vector2>().normalized;

        // Flip the character sprite based on movement direction
        // Assuming your character faces right by default
        if (movementInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else if (movementInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }

    private void Move()
    {
        // Move the Rigidbody2D based on input and move speed
        rb.MovePosition(rb.position + movementInput * (moveSpeed * Time.fixedDeltaTime));
    }

    private void OnJumpPressed()
    {
        spaceTapCount++;

        if (spaceTapCount == 1)
        {
            doubleTapTime = Time.time; // Record time of first tap

            if (isGrounded)
            {
                // Perform a ground jump
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("OneJump"); // Trigger for single jump animation
                isFlying = false; // Ensure flying is off when starting a ground jump
                animator.SetBool("IsFlying", false); // Update animator parameter
            }
            else if (!hasDoubleJumped && !isFlying)
            {
                // Perform a double jump if not grounded and not already double-jumped/flying
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                hasDoubleJumped = true;
                animator.SetTrigger("OneJump"); // Reuse OneJump trigger for double jump
            }
        }
        else if (spaceTapCount == 2 && Time.time - doubleTapTime <= tapDelay)
        {
            // Toggle flying mode on double tap within delay
            isFlying = !isFlying;
            animator.SetBool("IsFlying", isFlying); // Control the flying animation state
            spaceTapCount = 0; // Reset tap count after successful double tap
        }

        // Reset tap count if the delay between taps is too long
        if (Time.time - doubleTapTime > tapDelay)
        {
            spaceTapCount = 0;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a red wire sphere in the editor to visualize the ground check area
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
