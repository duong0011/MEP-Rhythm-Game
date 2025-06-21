using UnityEngine;

public class TileClick : MonoBehaviour
{
    [SerializeField] private Transform checkPivot;
    [SerializeField] private GhostEffect ghostEffect;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject missText;

    public Transform CheckPivot => checkPivot;
    private void OnEnable()
    {
        ghostEffect.gameObject.SetActive(false);
        border.SetActive(false);
        missText.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = true;
       // GetComponent<TileMovement>().OnInvisible += (tile) => gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.touchCount == 0) return;

        foreach (Touch t in Input.touches)
        {
            if (t.phase != TouchPhase.Began) continue;

            Vector2 wp = Camera.main.ScreenToWorldPoint(t.position);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(wp))
            {
                HandleClick();
            }
        }
    }

    public void HandleClick()
    {
        var tileMovement = GetComponent<TileMovement>();
        if (tileMovement.IsChecked) return;

        var result = GameEventManager.Instance.RegisterHit(checkPivot.position, tileMovement.IndexInTileList);
        ScoreManager.Instance.Register(result);
        GetComponent<TileMovement>().Checked();
        if (result != ScoreResult.Miss)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            ghostEffect.gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            ghostEffect.Play();
            border.SetActive(true);
            var audioManager = AudioManager.Instance;
            AudioClip clip = audioManager.clickTileMusic;
            audioManager.PlaySFX(clip);

        }
        else
        {
            var audioManager = AudioManager.Instance;
            AudioClip clip = audioManager.missMusic;
            audioManager.PlaySFX(clip);
            missText.SetActive(true);
        }
    }

    public void TestAutoClick() => HandleClick();
}
