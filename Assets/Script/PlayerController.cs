using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;


  void Awake()
  {
    playerControls = new PlayerControls();
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

  void Update()
  {
    PlayerInput();
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
    }

  private void Move()
  {
    rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}