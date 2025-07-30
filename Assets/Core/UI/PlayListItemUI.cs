using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayListItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int Index { set; get; }

    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image coverImage;

    [Header("Color Settings")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = new Color32(255, 239, 0, 255);
    [SerializeField] private float tweenDuration = 0.25f;

    private Tween colorTween;

    private void Awake()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = defaultColor;
        }
    }

    public void SetData(MusicGameInfo musicGameInfo)
    {
        titleText.text = $"{musicGameInfo.musicName} \n - \n {musicGameInfo.artistName}";
        coverImage.sprite = musicGameInfo.coverImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Khi nhấn (touch down) → chuyển sang màu vàng
        if (backgroundImage != null)
        {
            colorTween?.Kill();
            colorTween = backgroundImage.DOColor(highlightColor, tweenDuration);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Khi thả ra → chuyển về màu gốc và thực hiện hành động click
        if (backgroundImage != null)
        {
            colorTween?.Kill();
            colorTween = backgroundImage.DOColor(defaultColor, tweenDuration);
        }

    }
    public void OnClick()
    {
        GameManager.Instance.Play(Index);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.selectedSFX);
    }
}
