using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private bool goNextLevel;
    [SerializeField] private string levelName;

    [Header("Video Settings")]
    [SerializeField] private GameObject videoUI;          // RawImage canvas or parent
    [SerializeField] private VideoPlayer videoPlayer;     // Assign VideoPlayer component

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            // Show and play the transition video
            videoUI.SetActive(true);
            videoPlayer.Play();

            StartCoroutine(WaitForVideoToEnd());
        }
    }

    private IEnumerator WaitForVideoToEnd()
    {
        // Wait until video finishes playing
        while (videoPlayer.isPlaying)
            yield return null;

        yield return new WaitForSeconds(5f); // optional pause

        if (goNextLevel)
        {
            SceneControler.instance.NextLevel();
        }
        else
        {
            SceneControler.instance.LoadScene(levelName);
        }
    }
}
