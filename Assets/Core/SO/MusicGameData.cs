using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicGameData", menuName = "Configs/MusicGameData", order = 1)]
public class MusicGameData : ScriptableObject
{
    [Header("Music Settings")]
    [SerializeField] public List<MusicGameInfo> musicGameInfos;
}