using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndGameVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip winClip;
    public VideoClip loseClip;

    public bool isWin = true; // Set this from another script when ending the game

    void Start()
    {
        PlayEnding();
    }

    void PlayEnding()
    {
        videoPlayer.clip = isWin ? winClip : loseClip;
        videoPlayer.Play();

        // Optional: go back to menu after video ends
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("Stage0"); // Replace with your scene
    }
}