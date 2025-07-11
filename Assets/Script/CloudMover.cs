using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f;                // How fast the cloud moves
    public float resetPositionX = 20f;        // When to reset cloud (off screen right)
    public float startPositionX = -20f;       // Where to place cloud (off screen left)

    void Update()
    {
        // Move cloud to the right
        transform.position += Vector3.right * speed * Time.deltaTime;

        // If it goes too far, reset to left side
       if (transform.position.x > resetPositionX)
{
    float randomY = Random.Range(2f, 5f); // Random height range
    transform.position = new Vector3(startPositionX, randomY, transform.position.z);
}

    }
}
