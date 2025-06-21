using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ButtonConfig", menuName = "Configs/ButtonConfig", order = 1)]
public class ButtonConfig : ScriptableObject
{
    [Header("Audio Settings")]
    public AudioClip SfxSelected; // Âm thanh khi nhấn nút
}

public class Button_Refix : Button
{
    [SerializeField] private ButtonConfig config; // Đảm bảo hiển thị trong Inspector

    protected override void Awake()
    {
        base.Awake();
        ValidateReferences();
        SetupButton();
    }

    private void ValidateReferences()
    {
        if (config == null)
        {
            Debug.LogWarning($"ButtonConfig is not assigned in {gameObject.name}. Please assign in Inspector.");
        }
    }

    private void SetupButton()
    {
        // Thêm sự kiện click vào onClick
        onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager instance is null!");
            return;
        }

        AudioClip clip = config != null ? config.SfxSelected : null;
        if (clip == null)
        {
            Debug.LogWarning("SFX clip is not assigned in ButtonConfig!");
            return;
        }

        AudioManager.Instance.PlaySFX(clip);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onClick.RemoveListener(OnButtonClicked); // Xóa sự kiện để tránh rò rỉ bộ nhớ
    }
}