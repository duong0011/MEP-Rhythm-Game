using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Configs/ScoreConfig", order = 1)]
public class ScoreConfig : ScriptableObject
{
    [Header("Score Values")]
    public int PerfectScore = 5;
    public int GoodScore = 4;
    public int OkScore = 3;
    public float[] StarThresholds = { 0.3f, 0.6f, 1.0f };

    [Header("Animation Durations")]
    public float TimeDecorInteract = 0.5f;
    public float MinAlphaColorDecor = 0.4f;
    public float TimeToScaleUp = 0.3f;
    public float TimeToScaleDown = 0.2f;
    public float ScoreTextScaleDuration = 0.1f;
    public float ScoreTextResetDuration = 0.2f;
    public float ScoreResultHideDelay = 0.7f;
}
