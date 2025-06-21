using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private SpawnTileManager spawnTileManager;
    [SerializeField] private int timeToStartGame = 3;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private AudioClip audioClip;


    private void Start()
    {
        OnGameStart();
        spawnTileManager.OnGameEnd += OnGameEnd;
    }

    public void OnGameStart()
    {
        StopAllCoroutines();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSelected);
        int bpm = 120; // Default BPM
        if (GameManager.Instance != null)
        {
            audioClip = GameManager.Instance.currentMusicGameInfo.audioClip;
            bpm = GameManager.Instance.currentMusicGameInfo.bpm;
        }
        spawnTileManager.gameObject.SetActive(false);
        endGameUI.SetActive(false);
        spawnTileManager.ResetSpawn();
       
        ScoreManager.Instance.Restart();
        GameEventManager.Instance.SetBPM(bpm); // Set default BPM, can be changed later
        GameEventManager.Instance.musicClip = audioClip;
        GameEventManager.Instance.SetUpTile();
        GameEventManager.Instance.Restart();
        StartCoroutine(StartGameColdDown());

    }
    IEnumerator StartGameColdDown()
    {
        startGameUI.SetActive(true);
        int timeLeft = timeToStartGame;
        while (timeLeft > 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFXCountDown);
            timeText.text = timeLeft.ToString("F0");
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        startGameUI.SetActive(false);
        spawnTileManager.gameObject.SetActive(true);
        float timeAudio = audioClip.length;
        StartCoroutine(MusicTimer(timeAudio));
        AudioManager.Instance.PlayMusic(audioClip, false);

    }
    IEnumerator MusicTimer(float timeAudio)
    {
        float timeLeft = timeAudio + 2f;
        while (timeLeft >= 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        OnGameEnd();
    }
    private void OnGameEnd()
    {
        var clip = AudioManager.Instance.endGameMusic;
        AudioManager.Instance.PlayMusic(clip);
        spawnTileManager.gameObject.SetActive(false);
        endGameUI.SetActive(true);
    }
    private void OnDestroy()
    {
        spawnTileManager.OnGameEnd -= OnGameEnd;
    }
    public void BackToMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSelected);
        LoadingManager.Instance.LoadNewScene("MainMenu");
    }
}
