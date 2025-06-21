using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicGameData", menuName = "Configs/MusicGameData", order = 1)]
public class MusicGameData : ScriptableObject
{
    [Header("Music Settings")]
    [SerializeField] public List<MusicGameInfo> musicGameInfos;
}
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
    public AudioClip audioClip;
    public int bpm;
}
