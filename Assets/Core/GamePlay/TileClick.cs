using UnityEngine;

public class TileClick : MonoBehaviour
{
    [SerializeField] private Transform checkPivot;
    [SerializeField] private GhostEffect ghostEffect;
    [SerializeField] private GameObject missText;

    private TileMovement tileMovement;
    private SpriteRenderer spriteRenderer;
    private Collider2D tileCollider;

    public Transform CheckPivot => checkPivot;

    private void Awake()
    {
        tileMovement = GetComponent<TileMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tileCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ghostEffect?.gameObject.SetActive(false);
        missText?.SetActive(false);
        spriteRenderer.enabled = true;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Began) continue;

            Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (tileCollider != null && tileCollider.OverlapPoint(touchWorldPos))
            {
                HandleClick();
            }
        }
    }

    public void HandleClick()
    {
        if (tileMovement == null || tileMovement.IsChecked) return;

        var result = GameEventManager.Instance.RegisterHit(checkPivot.position, tileMovement.IndexInTileList);
        ScoreManager.Instance.Register(result);
        tileMovement.Checked();

        if (result == ScoreResult.Miss)
        {
            ShowMiss();
        }
        else
        {
            ShowSuccess();
        }
    }

    private void ShowSuccess()
    {
        if (ghostEffect != null)
        {
            ghostEffect.gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            ghostEffect.Play();
        }

        PlaySFX(AudioManager.Instance?.clickTileMusic);
    }

    private void ShowMiss()
    {
        PlaySFX(AudioManager.Instance?.missMusic);
        if (missText != null)
        {
            missText.SetActive(true);
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            AudioManager.Instance?.PlaySFX(clip);
        }
    }

    public void TestAutoClick() => HandleClick();
}
