using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 0.2f;
    [SerializeField] private float restartDelay = 5f; // delay in seconds
    [SerializeField] private VideoPlayer loseVideoPlayer; // Assign in inspector
    [SerializeField] private GameObject loseVideoUI; 
    private Vector3 startPosition;
     private bool hasPlayed = false;

    private void Start()
    {
        startPosition = transform.position;
    }
    public void DisableFog()
{
    this.enabled = false;  // disables Update + OnTriggerEnter2D
}


    private void Update()
    {
        transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;

            // Play death sound + pause music
            AudioManager.Instance.PlayDeathWithMusicPause();

            // Show and play lose video
            loseVideoUI.SetActive(true);
            loseVideoPlayer.Play();
         


            // Wait for video to finish, then restart
            StartCoroutine(WaitForVideoAndRestart());
        }
    }

    private IEnumerator WaitForVideoAndRestart()
    {
        // Wait until video finishes playing
        while (loseVideoPlayer.isPlaying)
        {
            yield return null;
        }

        // Optional: small delay after video
        yield return new WaitForSeconds(6f);

        // Load Stage0
        SceneManager.LoadScene("Stage0");
    }
}
