using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [Header("Audio Clip")]
    public AudioClip backGroundMS;
    public AudioClip hoverMusic;
    public AudioClip clickTileMusic;
    public AudioClip holdTileMusic;
    public AudioClip endGameMusic;
    public AudioClip sfxSelected;
    public AudioClip SFXCountDown;
    public AudioClip missMusic;

    private float volume = 1f;
    public float Volume
    {
        get => volume;
        set
        {
            volume = value;
            SetVolum(volume);
        }
    }
    private void Start()
    {
        musicSource.clip = backGroundMS;
        musicSource.Play();
    }
    public void SetVolum(float volume)
    {
        musicSource.volume = volume;
        SFXSource.volume = volume * 0.4f;
    }
    public void StopSFX()
    {
        SFXSource.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip, bool isLooping = true)
    {
        if (clip == null) return;
        musicSource.loop = isLooping;
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
