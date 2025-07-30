using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI perfectTimes;
    [SerializeField] TextMeshProUGUI goodTimes;
    [SerializeField] TextMeshProUGUI OKTimes;
    [SerializeField] TextMeshProUGUI MissTimes;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] List<Image> stares = new();
    [SerializeField] GameObject winGame;
    [SerializeField] GameObject loseGame;

    public void ShowResult(int perfectTime, int goodTime, int OKTime,int missTime, int score, int starAmount)
    {
        perfectTimes.text = $"Get {perfectTime} times Perfect";
        goodTimes.text = $"Get {goodTime} times Good";
        OKTimes.text = $"Get {OKTime} times OK";
        MissTimes.text = $"Get {missTime} times Perfect";
        Score.text = score.ToString();
        foreach (Image image in stares)
        {
            image.color = Color.white;
        }
        if (starAmount == 0)
        {
            loseGame.SetActive(true);
            winGame.SetActive(false);
            if(AudioManager.Instance.loseGameSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.loseGameSFX);
            }
            return;
        }
        if (AudioManager.Instance.winGameSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winGameSFX);
        }
        loseGame.SetActive(false);
        winGame.SetActive(true);
        for (int i = 0; i < starAmount; i++) {
            stares[i].color = Color.yellow;
        }
    }
    private void OnEnable()
    {
        ScoreManager.Instance.ShowScoreGameDetail();
    }
}
