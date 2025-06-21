using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;



public class GameEventManager : Singleton<GameEventManager>
{
    [SerializeField] private GameConfig config;
    [SerializeField] private GameObject lineTiming;
    [SerializeField] private Image autoPlayImage;
    
    public bool AutoPlay { private set; get; } = false; // For testing purposes
   
    private readonly List<TileData> tileList = new();
    private Camera mainCamera;

    public int TotalScoreToWinGame { private set; get; }
    public float LineTimingY => lineTiming != null ? lineTiming.transform.position.y : 0f;
    public IReadOnlyList<TileData> TileList => tileList.AsReadOnly();
    public int BPM { private set; get;}
    public void SetBPM(int bpm)
    {
        BPM = bpm;
        if (bpm <= 0)
        {
            Debug.LogError("BPM must be greater than 0");
            BPM = 120; // Default value
        }
    }
    public AudioClip musicClip;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        
      
    }
    public void AutoPlayTrigger()
    {
        AutoPlay = !AutoPlay;
        autoPlayImage.color = AutoPlay ? Color.green : Color.white;
    }
    public void SetUpTile()
    {
        if (musicClip == null || config == null) return;

        tileList.Clear();
        Vector3 firstTilePos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, config.FirstTileViewportY, 0));
        
        if (lineTiming != null)
        {
            lineTiming.transform.position = new Vector3(firstTilePos.x, firstTilePos.y, 0);
        }

        int amountTile = CalculateTileAmount();
        TotalScoreToWinGame = (amountTile - config.ExtraTiles) * config.PerfectScorePerTile;
        int amountFirstTile = Random.Range(config.MinFirstTiles, config.MaxFirstTiles + 1);

        for (int i = 0; i < amountTile; i++)
        {
            int type = i <= amountFirstTile ? 0 : Random.Range(0, 4); // 0: click, 1: hold, 2: pair row 0-1, 3: pair row 1-3
            float ySpawn = i <= amountFirstTile ? firstTilePos.y : RegisterNote();
            float speed = GetFallTime(ySpawn);
            TileData tileData = new(type, ySpawn, speed, type <= 1 ? 1 : 2);
            tileList.Add(tileData);
        }
    }

    private int CalculateTileAmount()
    {
        if (musicClip == null || config == null) return 0;
        return (int)(musicClip.length / (60f / BPM)) + config.ExtraTiles;
    }

    public float RegisterNote()
    {
        if (mainCamera == null || config == null || config.CandidateViewportY.Length == 0) return 0f;

        float newY = config.CandidateViewportY[Random.Range(0, config.CandidateViewportY.Length)];
        Vector3 worldY = mainCamera.ViewportToWorldPoint(new Vector3(0, newY, 0));
        return worldY.y;
    }

    public ScoreResult RegisterHit(Vector3 tileY, int index)
    {
        if (index < 0 || index >= tileList.Count)
        {
            Debug.LogWarning($"Invalid tile index: {index}");
            return ScoreResult.Miss;
        }

        TileData tileData = tileList[index];
        float previousY = LineTimingY;

        if (tileData.HitRemain == 1 && index + 1 < tileList.Count)
        {
            var nextTileData = tileList[index + 1];
            if (lineTiming != null)
            {
                lineTiming.transform.position = new Vector3(0, nextTileData.LineTimingY, 0);
            }
        }
        else
        {
            tileList[index] = new TileData(tileData.Type, tileData.LineTimingY, tileData.Speed, tileData.HitRemain - 1);
        }

        float delta = Mathf.Abs(previousY - tileY.y);
        return EvaluateHit(delta);
    }

    private ScoreResult EvaluateHit(float delta)
    {
        if (config == null) return ScoreResult.Miss;

        if (delta <= config.PerfectHitThreshold) return ScoreResult.Perfect;
        if (delta <= config.GoodHitThreshold) return ScoreResult.Good;
        if (delta <= config.OkHitThreshold) return ScoreResult.Ok;
        return ScoreResult.Miss;
    }

    private float GetFallTime(float ySpawn)
    {
        if (mainCamera == null || config == null) return 0f;

        float topY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        return Mathf.Abs(ySpawn - topY) / (60f / BPM) * config.TileSpeedModifier;

    }

    public void Restart()
    {
        // Clear tile list and reset line timing position
        tileList.Clear();
        if (lineTiming != null && mainCamera != null)
        {
            Vector3 firstTilePos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, config.FirstTileViewportY, 0));
            lineTiming.transform.position = new Vector3(firstTilePos.x, firstTilePos.y, 0);
        }

        // Reset total score to win
        TotalScoreToWinGame = 0;

        // Reinitialize tiles
        SetUpTile();
    }
}

public enum ScoreResult
{
    Perfect,
    Good,
    Ok,
    Miss
}

public struct TileData
{
    public int Type; // 0: click, 1: hold, 2: pair row 0-1, 3: pair row 1-3
    public float LineTimingY;
    public float Speed;
    public int HitRemain;

    public TileData(int type, float lineTimingY, float speed, int hitRemain)
    {
        Type = type;
        LineTimingY = lineTimingY;
        Speed = speed;
        HitRemain = hitRemain;
    }
}