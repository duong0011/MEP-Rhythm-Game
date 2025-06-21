using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayListItemUI : MonoBehaviour
{
    public int Index { set; get; }
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    public void OnClick()
    {
        GameManager.Instance.Play(Index);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSelected);
    }
    public void SetData(string musicName, string artist)
    {
        titleText.text = $"Music Name: {musicName} \n Artist : {artist}";
        ;
    }
}
