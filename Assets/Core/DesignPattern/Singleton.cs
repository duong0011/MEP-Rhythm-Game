using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting;
    [SerializeField] protected bool dontDestroyOnLoad = true;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
               
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"[Singleton] {typeof(T)}";

                        var singletonInstance = _instance as Singleton<T>;
                        if (singletonInstance != null && singletonInstance.dontDestroyOnLoad)
                        {
                            DontDestroyOnLoad(singletonObject);
                        }

                       
                    }

                    // Làm mới tham chiếu ngay khi truy cập instance
                    var singleton = _instance as Singleton<T>;
                    if (singleton != null)
                    {
                        singleton.RefreshReferences();
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        _applicationIsQuitting = false; // Reset khi instance mới được tạo

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        RefreshReferences();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            if (!dontDestroyOnLoad)
            {
                
                _instance = null;
            }
            _applicationIsQuitting = true;
        }

        if (!dontDestroyOnLoad)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (_instance == this && !dontDestroyOnLoad)
        {
           
            _instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_instance == this && !dontDestroyOnLoad)
        {
           
            RefreshReferences();
        }
    }

    protected virtual void RefreshReferences()
    {
        // Phương thức ảo để các class con ghi đè và làm mới tham chiếu
    }
}