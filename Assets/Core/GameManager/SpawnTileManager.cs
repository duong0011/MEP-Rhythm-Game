using System;
using UnityEngine;

public class SpawnTileManager : MonoBehaviour
{
    [SerializeField] private PoolTiles clickPool;
    [SerializeField] private PoolTiles holdPool;

    public event Action OnGameEnd;
    private int BPM => GameEventManager.Instance.BPM;
    private Camera cam;
    private float[] stripX = { 0.125f, 0.375f, 0.625f, 0.875f };
    private float nextSpawnTime = 0;
    private int currentTileIdex = 0;

    void Awake() => cam = Camera.main;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
            SpawnTile();
    }

    void SpawnTile()
    {
        var currentTile = GameEventManager.Instance.TileList[currentTileIdex];
        int type = currentTile.Type;

        float speed = currentTile.Speed;

        float delay = 60f / BPM;

        switch (type)
        {
            case 0:
                SpawnSingle(clickPool,currentTileIdex, speed, UnityEngine.Random.Range(0, 4));
                nextSpawnTime = Time.time + delay;
                break;
            case 1:
               
                SpawnSingle(holdPool, currentTileIdex, speed, UnityEngine.Random.Range(0, 4));
                nextSpawnTime = Time.time + delay * 3;
                break;
            case 2:
                SpawnPair(clickPool, currentTileIdex, speed, new[] { 0, 2 });
                nextSpawnTime = Time.time + delay;
                break;
            case 3:
                SpawnPair(clickPool, currentTileIdex, speed, new[] { 1, 3 });
                nextSpawnTime = Time.time + delay;
                break;
        }
        currentTileIdex++;
    }

    void SpawnSingle(PoolTiles pool,int indexInTileList, float speed, int index)
    {
        var tile = pool.GetPooledItem();
        SetupTile(tile, currentTileIdex, speed, index);
    }

    void SpawnPair(PoolTiles pool,int indexInTileList, float speed, int[] indices)
    {
        foreach (var i in indices)
            SpawnSingle(pool, indexInTileList, speed, i);
    }

    void SetupTile(TileMovement tile,int indexInTileList ,float speed, int stripIndex)
    {
        tile.SetTileData(indexInTileList, speed);
        Vector3 vp = new Vector3(stripX[stripIndex], 1f, 0);
        Vector3 wp = cam.ViewportToWorldPoint(vp);
        tile.transform.position = new Vector3(wp.x, wp.y, 0);
        tile.OnInvisible += ReturnTile;

        float width = cam.orthographicSize * 2f * cam.aspect;
        float quadWidth = width / 4f;

        tile.transform.localScale = new Vector3(
            quadWidth / 2.75f,
            tile.transform.localScale.y,
            tile.transform.localScale.z
        );
    }

    void ReturnTile(TileMovement tile)
    {
        tile.OnInvisible -= ReturnTile;
        if(!tile.IsChecked)
        {
            OnGameEnd?.Invoke();
        }
        clickPool.ReturnActivedItemToPool(tile);
        holdPool.ReturnActivedItemToPool(tile);
    }

    public void ResetSpawn()
    {
        currentTileIdex = 0;
        nextSpawnTime = 0;
        clickPool.ReturnAllTiemsToPool();
        holdPool.ReturnAllTiemsToPool();
    }
}
