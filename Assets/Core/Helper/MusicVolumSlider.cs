using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumSlider : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChanged);
        slider.value = AudioManager.Instance.MusicVolume;
    }
    private void OnValueChanged(float value)
    {
        AudioManager.Instance.MusicVolume = value;
    }
    
}
