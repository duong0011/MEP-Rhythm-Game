using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public MusicGameData musicGameData;
    public MusicGameInfo currentMusicGameInfo { private set; get; }
    public void Play(int index)
    {
        currentMusicGameInfo = musicGameData.musicGameInfos[index];
        LoadingManager.Instance.LoadNewScene("MainGame");
    }

}
[System.Serializable]
public struct MusicGameInfo
{
    public string musicName;
    public string artistName;
    public Sprite coverImage;
    public AudioClip audioClip;
    public int bpm;
}
