using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Music and Timing")]

    public float TileSpeedModifier = 0.8f;

    [Header("Tile Setup")]
    public float FirstTileViewportY = 0.125f;
    public int ExtraTiles = 20;
    public int MinFirstTiles = 8;
    public int MaxFirstTiles = 12;
    public float[] CandidateViewportY = { 0.125f, 0.25f, 0.3f, 0.375f };
    public int PerfectScorePerTile = 5;

    [Header("Hit Detection")]
    public float PerfectHitThreshold = 0.2f;
    public float GoodHitThreshold = 0.5f;
    public float OkHitThreshold = 1.1f;
}