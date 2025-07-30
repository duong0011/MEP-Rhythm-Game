using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSlider: MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChanged);
        slider.value = AudioManager.Instance.SFXVolume;
    }
    private void OnValueChanged(float value)
    {
        AudioManager.Instance.SFXVolume = value;
    }
}
