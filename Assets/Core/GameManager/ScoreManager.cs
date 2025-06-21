using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;



public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private ScoreConfig config;
    [SerializeField] private SpriteRenderer decorSprite;
    [SerializeField] private List<GameObject> scoreResult;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image[] stars;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    private int _comboAmount = -1;
    private int ComboAmount
    {
        get => _comboAmount;
        set
        {
            _comboAmount = Mathf.Min(5, value);
            UpdateComboText();
        }
    }
    private int score = 0;
    private bool isInteractDecorRunning = false;
    private readonly Color starColor = Color.yellow;
    private readonly Color defaultStarColor = Color.white;

    protected override void Awake()
    {
        base.Awake();
    }



    public void Register(ScoreResult result)
    {
        if (!isInteractDecorRunning)
        {
            StartCoroutine(InteractDecorSprite());
        }

        switch (result)
        {
            case ScoreResult.Perfect:
                score += config.PerfectScore * Mathf.Max(1, ComboAmount);
                ComboAmount++;
                TriggerScoreEffect(0);
                break;
            case ScoreResult.Good:
                score += config.GoodScore * Mathf.Max(1, ComboAmount);
                ComboAmount++;
                TriggerScoreEffect(1);
                break;
            case ScoreResult.Ok:
                score += config.OkScore;
                ComboAmount = -1;
                TriggerScoreEffect(2);
                break;
            case ScoreResult.Miss:
                ComboAmount = -1;
                TriggerScoreEffect(3);
                break;
            default:
                Debug.LogWarning($"Unhandled ScoreResult: {result}");
                break;
        }
        AddScore(0);
    }

    private void UpdateComboText()
    {
        if (comboText == null) return;

        if (ComboAmount >= 1)
        {
            comboText.text = $"Combo: X{ComboAmount}";
            comboText.gameObject.SetActive(true);
            PlayScaleAnimation(comboText.gameObject, config.TimeToScaleUp, config.TimeToScaleDown, 1f);
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }

    private void TriggerScoreEffect(int resultIndex)
    {
        if (scoreResult == null || resultIndex < 0 || resultIndex >= scoreResult.Count)
        {
            Debug.LogWarning($"Invalid scoreResult index: {resultIndex}");
            return;
        }

        foreach (GameObject obj in scoreResult)
        {
            if (obj != null) obj.SetActive(false);
        }

        GameObject targetObject = scoreResult[resultIndex];
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            PlayScaleAnimation(targetObject, config.TimeToScaleUp, config.TimeToScaleDown, config.ScoreResultHideDelay);
        }
    }

    private void PlayScaleAnimation(GameObject target, float scaleUpTime, float scaleDownTime, float hideDelay)
    {
        if (target == null) return;

        Sequence scaleSequence = DOTween.Sequence();
        RectTransform rt = target.GetComponent<RectTransform>();
        if (rt == null) return;

        rt.localScale = Vector3.zero;
        scaleSequence.Join(rt.DOScale(1.2f, scaleUpTime).SetEase(Ease.OutBack));
        scaleSequence.Join(rt.DOScale(1f, scaleDownTime).SetEase(Ease.InOutSine).SetDelay(scaleUpTime));
        scaleSequence.Join(rt.DOScale(0f, hideDelay).SetEase(Ease.InOutSine).SetDelay(scaleUpTime));
    }

    public int GetScore() => score;

    public void AddScore(int s)
    {
        score += s;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (totalScoreText == null) return;

        totalScoreText.text = score.ToString();
        PlayScaleAnimation(totalScoreText.gameObject, config.ScoreTextScaleDuration, config.ScoreTextResetDuration, 0f);

        if (progressBar != null && GameEventManager.Instance != null)
        {
            progressBar.fillAmount = Mathf.Min((float)score / GameEventManager.Instance.TotalScoreToWinGame, 1f);
            UpdateStars();
        }
    }

    private void UpdateStars()
    {
        if (stars == null || config.StarThresholds == null) return;

        for (int i = 0; i < stars.Length && i < config.StarThresholds.Length; i++)
        {
            if (stars[i] != null && progressBar.fillAmount >= config.StarThresholds[i])
            {
                stars[i].color = starColor;
            }
        }
    }

    private IEnumerator InteractDecorSprite()
    {
        if (decorSprite == null)
        {
            yield break;
        }

        isInteractDecorRunning = true;

        float time = 0f;
        Color color = decorSprite.color;
        color.a = 1f;
        decorSprite.color = color;

        float startAlpha = color.a;
        while (time < config.TimeDecorInteract)
        {
            time += Time.deltaTime;
            float t = time / config.TimeDecorInteract;
            color.a = Mathf.Lerp(startAlpha, config.MinAlphaColorDecor, t);
            decorSprite.color = color;
            yield return null;
        }

        color.a = config.MinAlphaColorDecor;
        decorSprite.color = color;

        isInteractDecorRunning = false;
    }

    public void Restart()
    {
        // Reset score and combo
        score = 0;
        ComboAmount = -1;

        // Reset progress bar
        if (progressBar != null)
        {
            progressBar.fillAmount = 0f;
        }

        // Reset stars color
        if (stars != null)
        {
            foreach (Image star in stars)
            {
                if (star != null)
                {
                    star.color = defaultStarColor;
                }
            }
        }

        // Reset score result display
        if (scoreResult != null)
        {
            foreach (GameObject obj in scoreResult)
            {
                if (obj != null) obj.SetActive(false);
            }
        }

        // Reset combo text
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }

        // Reset decor sprite
        if (decorSprite != null)
        {
            Color color = decorSprite.color;
            color.a = 1f;
            decorSprite.color = color;
        }

        // Stop any ongoing decor animation
        if (isInteractDecorRunning)
        {
            StopCoroutine(InteractDecorSprite());
            isInteractDecorRunning = false;
        }

        // Update score display
        UpdateScoreDisplay();
    }
}