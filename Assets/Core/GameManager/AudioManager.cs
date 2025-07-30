using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip clickTileSFX;
    public AudioClip holdTileSFX;
    public AudioClip selectedSFX;
    public AudioClip countdownSFX;
    public AudioClip missSFX;
    public AudioClip winGameSFX;
    public AudioClip loseGameSFX;

    private float musicVolume = 1f;
    private float sfxBaseVolume = 0.3f;
    private float sfxVolume = 1f;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            musicSource.volume = musicVolume;
        }
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            sfxSource.volume = sfxVolume * sfxBaseVolume;
        }
    }

    private void Start()
    {
        PreloadAllGameAudio();
        PlayMusic(backgroundMusic);
    }

    // SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX() => sfxSource.Stop();

    // Music
    public void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = 0.5f)
    {
        if (clip == null || clip == musicSource.clip) return;
        StartCoroutine(PlayMusicRoutine(clip, loop, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 0.1f)
    {
        musicSource.DOFade(0f, fadeDuration).OnComplete(() => musicSource.Stop());
    }

    private IEnumerator PlayMusicRoutine(AudioClip clip, bool loop, float fadeDuration)
    {
        float originalVolume = musicVolume;

        // Fade out current music
        yield return musicSource.DOFade(0f, fadeDuration).WaitForCompletion();
        musicSource.Stop();

        // Load clip if needed
        if (clip.loadState == AudioDataLoadState.Unloaded)
        {
            clip.LoadAudioData();
            while (clip.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();

        // Fade in new music
        yield return musicSource.DOFade(originalVolume, fadeDuration).WaitForCompletion();
    }

    // Preload tất cả audio trong danh sách bài hát để tránh giật
    public void PreloadAllGameAudio()
    {
        var musicInfos = GameManager.Instance?.musicGameData?.musicGameInfos;
        if (musicInfos == null) return;

        foreach (var info in musicInfos)
        {
            var clip = info.audioClip;
            if (clip != null && clip.loadState == AudioDataLoadState.Unloaded)
                clip.LoadAudioData();
        }
    }
}
