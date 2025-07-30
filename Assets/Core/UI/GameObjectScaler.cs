using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameObjectScaler : MonoBehaviour
{ 
    [SerializeField] private GameObject _gameObject;
    public float ScaleFactor = 1.2f; 
    public float Duration = 1f;
    public Ease EaseType = Ease.InOutSine; 


    private void OnEnable()
    {
        StartScaleAnimation();
    }

    private void OnDisable()
    {
        
        if (_gameObject != null)
        {
            _gameObject.transform.DOKill();
        }
    }

    private void StartScaleAnimation()
    {

        _gameObject.transform.localScale = Vector3.one;
        _gameObject.transform.DOScale(ScaleFactor, Duration)
            .SetEase(EaseType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}