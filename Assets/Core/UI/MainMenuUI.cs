using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] public GameObject PlaylistItemPrefab;
    [SerializeField] public RectTransform playlisContainer;
    [SerializeField] public GameObject playList;
    [SerializeField] public GameObject playButton;
    public float FadeDuration = 0.5f;


    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance is null!");
            return;
        }

        foreach (var musicInfo in GameManager.Instance.musicGameData.musicGameInfos)
        {
            if (PlaylistItemPrefab == null || playlisContainer == null) continue;

            var item = Instantiate(PlaylistItemPrefab, playlisContainer);
            var playListItemUI = item.GetComponent<PlayListItemUI>();
            if (playListItemUI != null)
            {
                playListItemUI.SetData(musicInfo.musicName, musicInfo.artistName);
                playListItemUI.Index = GameManager.Instance.musicGameData.musicGameInfos.IndexOf(musicInfo);
            }
        }
    }

    public void PlayMusicIndex(int index)
    {
        GameManager.Instance.Play(index);
    }

    public void ShowPlayList()
    {

        StartCoroutine(TogglePlayList());
    }

    private IEnumerator TogglePlayList()
    {
        bool isPlayListActive = playList.activeSelf;

        CanvasGroup playListCanvasGroup = playList.GetComponent<CanvasGroup>();
        CanvasGroup playButtonCanvasGroup = playButton.GetComponent<CanvasGroup>();

        // Thêm CanvasGroup nếu chưa có
        if (playListCanvasGroup == null)
        {
            playListCanvasGroup = playList.AddComponent<CanvasGroup>();
            playListCanvasGroup.alpha = isPlayListActive ? 1f : 0f;
        }
        if (playButtonCanvasGroup == null)
        {
            playButtonCanvasGroup = playButton.AddComponent<CanvasGroup>();
            playButtonCanvasGroup.alpha = isPlayListActive ? 0f : 1f;
        }

        // Toggle trạng thái
        if (!isPlayListActive)
        {
            // Hiển thị playList, ẩn playButton
            playList.SetActive(true);
            playListCanvasGroup.DOFade(1f, FadeDuration);
            playButtonCanvasGroup.DOFade(0f, FadeDuration).OnComplete(() => playButton.SetActive(false));
        }
        else
        {
            // Ẩn playList, hiển thị playButton
            playButton.SetActive(true);
            playListCanvasGroup.DOFade(0f, FadeDuration).OnComplete(() => playList.SetActive(false));
            playButtonCanvasGroup.DOFade(1f, FadeDuration);
        }

        yield return null;
    }
}