using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextMeshProScaler : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public float ScaleFactor = 1.2f; 
    public float Duration = 1f;
    public Ease EaseType = Ease.InOutSine; 


    private void OnEnable()
    {
        StartScaleAnimation();
    }

    private void OnDisable()
    {
        
        if (textMeshPro != null)
        {
            textMeshPro.transform.DOKill();
        }
    }

    private void StartScaleAnimation()
    {

        textMeshPro.transform.localScale = Vector3.one;
        textMeshPro.transform.DOScale(ScaleFactor, Duration)
            .SetEase(EaseType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}