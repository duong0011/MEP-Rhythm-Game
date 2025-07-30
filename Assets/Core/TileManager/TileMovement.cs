using UnityEngine;
using System;


public class TileMovement : MonoBehaviour
{
    [SerializeField] Transform endPivot;
    public event Action<TileMovement> OnInvisible;

    private float speed;
    private Camera cam;
    public bool IsChecked { get; private set; } = false;
    private int indexInTileList;
    public int IndexInTileList => indexInTileList;
        
    
    void Awake() => cam = Camera.main;
    void OnEnable() => IsChecked = false;

    public void SetTileData(int index, float s)
    {
        speed = s;
        indexInTileList = index;
    }
    public void Checked() => IsChecked = true;

    void Update()
    {
        if (!IsVisible())
        {
            OnInvisible?.Invoke(this);
           
            return;
        }

        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }
    private void FixedUpdate()
    {
        if (GameEventManager.Instance.AutoPlay)
        {
            AutoPlay();
        }
    }
    private bool IsVisible()
    {
        Vector3 vp = cam.WorldToViewportPoint(endPivot.position);
        return vp.y >= -0.1f;
    }
    private void AutoPlay()
    {
        var checkPivot = GetComponent<TileClick>()?.CheckPivot ?? GetComponent<TileHold>()?.CheckPivot;

        float randomRand = UnityEngine.Random.Range(0f, 0.5f);
        if (!IsChecked && (Math.Abs(checkPivot.position.y - GameEventManager.Instance.LineTimingY) <= randomRand  || checkPivot.position.y < GameEventManager.Instance.LineTimingY))
        {
            if (TryGetComponent<TileClick>(out var click)) click.TestAutoClick();
            if (TryGetComponent<TileHold>(out var hold)) hold.TestAutoClick(checkPivot.position);
        }
    }
}
