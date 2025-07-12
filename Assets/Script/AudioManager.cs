using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip BackgroundMusic;
    public AudioClip Death;
    public AudioClip Win;
    public AudioClip Jump;
    public AudioClip Walk;
    public AudioClip picked;

    private void Awake()
    {
        // Singleton pattern to avoid duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (BackgroundMusic != null)
        {
            musicSource.clip = BackgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary> Plays a general SFX. </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Optional helpers for common actions:

    public void PlayJumpSFX() => PlaySFX(Jump);
    public void PlayWalkSFX() => PlaySFX(Walk);
    public void PlayDeathSFX() => PlaySFX(Death);
    public void PlayWinSFX() => PlaySFX(Win);
    public void PlayPickedSFX() => PlaySFX(picked);

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }

        public void PlayDeathWithMusicPause(float resumeDelay = 10f)
    {
        StartCoroutine(PlayDeathThenResumeMusic(resumeDelay));
    }

    private IEnumerator PlayDeathThenResumeMusic(float delay)
    {
        musicSource.Pause(); // 🛑 Pause background music
        PlaySFX(Death);      // 🔊 Play death sound
        yield return new WaitForSeconds(delay);
        musicSource.UnPause(); // ▶ Resume background music
    }

}
