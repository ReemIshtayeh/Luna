using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip BackgroundMusic;
    public AudioClip Jump;
    public AudioClip Walk;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        musicSource.clip = BackgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
       sfxSource.PlayOneShot(clip);
    }
}
