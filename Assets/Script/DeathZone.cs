using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private Transform respawnPoint;

    private Vector3 startPosition; // Stores where the death zone starts

    private void Start()
    {
        startPosition = transform.position; // Save the original position at scene start
    }

    private void Update()
    {
        transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Respawn the player
            other.transform.position = respawnPoint.position;

            // Reset player velocity if they have a Rigidbody2D
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;

            // Reset the death zone to its start position
            transform.position = startPosition;
        }
    }
}
