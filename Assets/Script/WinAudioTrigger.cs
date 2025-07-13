using UnityEngine;

public class WinAudioTrigger : MonoBehaviour
{
private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)   // Use OnTriggerEnter for 3D
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            AudioManager.Instance.StopMusic();     // ⏹ Stop background music
            AudioManager.Instance.PlayWinSFX();    // 🔊 Play win sound

            Debug.Log("🎉 Player won! Win SFX played, music stopped.");
        }
    }
}
