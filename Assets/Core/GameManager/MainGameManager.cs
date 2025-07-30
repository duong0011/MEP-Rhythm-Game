using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MainGameManager : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private SpawnTileManager spawnTileManager;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Game Config")]
    [SerializeField] private int timeToStartGame = 3;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        spawnTileManager.OnGameEnd += OnGameEnd;
        OnGameStart();
    }

    private void OnDestroy()
    {
        spawnTileManager.OnGameEnd -= OnGameEnd;
    }

    public void OnGameStart()
    {
        StopAllCoroutines();

        PlaySFX(AudioManager.Instance?.selectedSFX);

        int bpm = 120;
        if (GameManager.Instance != null)
        {
            audioClip = GameManager.Instance.currentMusicGameInfo.audioClip;
            bpm = GameManager.Instance.currentMusicGameInfo.bpm;
        }

        FadeOutUI(endGameUI, 0.5f);
        spawnTileManager.PlayGame(false);

        ScoreManager.Instance.Restart();
        GameEventManager.Instance.SetBPM(bpm);
        GameEventManager.Instance.musicClip = audioClip;
        GameEventManager.Instance.SetUpTile();
        GameEventManager.Instance.Restart();

        StartCoroutine(StartGameCountdown());
    }

    private IEnumerator StartGameCountdown()
    {
        startGameUI.SetActive(true);
        FadeInUI(startGameUI, 0.2f);

        int timeLeft = timeToStartGame;
        while (timeLeft > 0)
        {
            PlaySFX(AudioManager.Instance?.countdownSFX);
            timeText.text = timeLeft.ToString("F0");
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        yield return FadeOutUI(startGameUI, 1f);
        startGameUI.SetActive(false);
        yield return new WaitForSeconds(0.02f);

        spawnTileManager.PlayGame(true);
        StartCoroutine(MusicTimer(audioClip.length));

        AudioManager.Instance?.PlayMusic(audioClip, false);
    }

    private IEnumerator MusicTimer(float duration)
    {
        yield return new WaitForSeconds(duration + 2f);
        OnGameEnd();
    }

    private void OnGameEnd()
    {
        spawnTileManager.PlayGame(false);
        AudioManager.Instance?.PlayMusic(AudioManager.Instance?.backgroundMusic);
        endGameUI.SetActive(true);
        FadeInUI(endGameUI, 0.5f);
    }

    public void BackToMainMenu()
    {
        PlaySFX(AudioManager.Instance?.selectedSFX);
        LoadingManager.Instance?.LoadNewScene("MainMenu");
    }

    #region Helper Methods

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            AudioManager.Instance?.PlaySFX(clip);
        }
    }

    private Tween FadeInUI(GameObject uiObject, float duration)
    {
        CanvasGroup cg = uiObject.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f;
            return cg.DOFade(1f, duration);
        }
        return null;
    }

    private Tween FadeOutUI(GameObject uiObject, float duration)
    {
        CanvasGroup cg = uiObject.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            return cg.DOFade(0f, duration).OnComplete(() => uiObject.SetActive(false));
        }
        else
        {
            uiObject.SetActive(false);
        }
        return null;
    }

    #endregion
}
