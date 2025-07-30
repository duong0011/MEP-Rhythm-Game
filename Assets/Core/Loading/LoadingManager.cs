using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private GameObject loadingBG; // UI nền loading
    [SerializeField] private float LoadingDelay = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadNewScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty!");
            return;
        }

        if (!IsSceneInBuild(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' is not found in Build Settings!");
            return;
        }
        var clip = AudioManager.Instance.backgroundMusic;
        AudioManager.Instance.PlayMusic(clip);
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private bool IsSceneInBuild(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        if (loadingBG != null)
        {
            CanvasGroup canvasGroup = loadingBG.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                loadingBG.SetActive(true);
                yield return canvasGroup.DOFade(1f, 1f).WaitForCompletion();
            }
            else
            {
                Debug.LogWarning("LoadingBG does not have a CanvasGroup component, using SetActive instead.");
                loadingBG.SetActive(true);
            }
        }

        yield return new WaitForSeconds(LoadingDelay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        if (loadingBG != null)
        {
            CanvasGroup canvasGroup = loadingBG.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                yield return canvasGroup.DOFade(0f, 1f).WaitForCompletion();
                loadingBG.SetActive(false);
            }
            else
            {
                loadingBG.SetActive(false);
            }
        }
    }
}