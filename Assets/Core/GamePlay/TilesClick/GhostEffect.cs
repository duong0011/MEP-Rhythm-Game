using UnityEngine;
using DG.Tweening;
using System;

public class GhostEffect : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    Vector3 originalPosition;
    Vector3 originalRotation;
    Vector3 originalScale;
    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        originalRotation = transform.eulerAngles;
    }

    public void Play()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.DOFade(0f, 0.5f)
          .SetEase(Ease.Linear);
    }

    void OnEnable()
    {
        OnReset();
        var sr = GetComponent<SpriteRenderer>();
        sr.DOFade(1f, 0f); 

    }
    private void OnReset()
    {
        transform.localPosition = originalPosition;
        transform.localScale = originalScale;
        transform.eulerAngles = originalRotation;
    }
}
