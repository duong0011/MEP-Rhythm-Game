using DG.Tweening;
using UnityEngine;

public class TileHold : MonoBehaviour
{
    public float holdThreshold = 0.5f;
    [SerializeField] private Transform checkPivot;
    [SerializeField] private Transform endPivot;
    [SerializeField] private GameObject startPivot;
    [SerializeField] private GameObject missText;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject plus3GO;
    [SerializeField] private int scoreWhenHoldFull = 3;

    public Transform CheckPivot => checkPivot;

    private float firstTouchY;
    private bool isBeingTouched = false;
    private int touchingFingerId = -1;
    private bool isHolding = true;

    private void OnEnable()
    {
        startPivot.SetActive(false);
        isBeingTouched = false;
        touchingFingerId = -1;
        isHolding = true;
        trailRenderer.gameObject.SetActive(false);
        trailRenderer.emitting = false;
        startPivot.transform.position = checkPivot.position;
        trailRenderer.transform.position = checkPivot.position;
        GetComponent<SpriteRenderer>().DOFade(1f, 0);
        startPivot.GetComponent<SpriteRenderer>().DOFade(1f, 0);
        missText.SetActive(false);
        plus3GO.SetActive(false);
    }

    void Update()
    {
        Vector2 posTouching = Vector2.zero;
        foreach (Touch touch in Input.touches)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            Collider2D col = GetComponent<Collider2D>();

            // Bắt đầu chạm
            if (touch.phase == TouchPhase.Began)
            {
                if (!isBeingTouched && col == Physics2D.OverlapPoint(worldPos))
                {
                    isBeingTouched = true;
                    touchingFingerId = touch.fingerId;
                    CheckBeganTouch(worldPos);
                }
            }

            // Đang giữ
            if (isBeingTouched && touch.fingerId == touchingFingerId  &&
                (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                posTouching = worldPos;
                isHolding = true;
            }

            // Kết thúc chạm
            if (isBeingTouched && touch.fingerId == touchingFingerId && touch.phase == TouchPhase.Ended)
            {
                EndTouch();
            }
        }
        if (isHolding)
        {
            OnHold(posTouching);
        }
    }

    private void OnHold(Vector2 currentTouchingPosition)
    {
        if (startPivot.transform.position.y >= endPivot.position.y)
        {
            EndTouch();
            plus3GO.SetActive(true);
            GetComponent<SpriteRenderer>().DOFade(0.5f, 0);
            startPivot.GetComponent <SpriteRenderer>().DOFade(0.5f, 0);
            ScoreManager.Instance.AddScore(scoreWhenHoldFull);
            return;
        }
        startPivot.transform.position = new Vector3(startPivot.transform.position.x, firstTouchY, 0);
    }

    private void EndTouch()
    {
        isBeingTouched = false;
        touchingFingerId = -1;
        isHolding = false;
        trailRenderer.emitting = false;
        _particleSystem.gameObject.SetActive(false);
        trailRenderer.gameObject.SetActive(false); // Tắt trail khi kết thúc
         AudioManager.Instance.StopSFX();

    }

    private void CheckBeganTouch(Vector2 touchPos)
    {
        var tileMovement = GetComponent<TileMovement>();
        if (tileMovement.IsChecked) return;
        startPivot.transform.position = new(startPivot.transform.position.x, touchPos.y);
        var result = GameEventManager.Instance.RegisterHit(checkPivot.position, tileMovement.IndexInTileList);
        ScoreManager.Instance.Register(result);
        tileMovement.Checked();
        if (result != ScoreResult.Miss)
        {
            firstTouchY = touchPos.y;
            startPivot.SetActive(true);
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.gameObject.transform.position = startPivot.transform.position;
            trailRenderer.emitting = true;
            _particleSystem.gameObject.SetActive(true);
            var audioManager = AudioManager.Instance;
            var clip = audioManager.holdTileSFX;
            audioManager.PlaySFX(clip);
        }
        else
        {
            var audioManager = AudioManager.Instance;
            var clip = audioManager.missSFX;
            audioManager.PlaySFX(clip);
            missText.SetActive(true);
        }
    }

    public void TestAutoClick(Vector2 touchPos)
    {
        CheckBeganTouch(touchPos);
        isHolding = true;
    }
}