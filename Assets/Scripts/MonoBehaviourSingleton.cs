using UnityEngine;
using UnityEngine.Assertions;

namespace Utils
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private const string SingletonIsNull = "An instance of {0} is needed in the scene, but there is none.";
        private const string SingletonDuplicate = "Multiple instances of {0} found in the scene. Only one should exist.";

        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"Singleton {typeof(T)} instance already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            var singletonObject = new GameObject($"{typeof(T)} (Singleton)");
                            _instance = singletonObject.AddComponent<T>();

                            //    DontDestroyOnLoad(singletonObject);

                            Debug.Log($"Created new singleton instance of {typeof(T)}");
                        }
                        else
                        {
                            //       DontDestroyOnLoad(_instance.gameObject);
                        }

                        Assert.IsNotNull(_instance, string.Format(SingletonIsNull, typeof(T)));

                        // Check for duplicates
                        var instances = FindObjectsByType<T>(FindObjectsSortMode.None);
                        if (instances.Length > 1)
                        {
                            Debug.LogError(string.Format(SingletonDuplicate, typeof(T)));
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i].gameObject);
                            }
                        }
                    }
                    return _instance;
                }
            }
        }

        public static bool IsInstanceExists
        {
            get
            {
                if (_applicationIsQuitting) return false;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();
                    }
                    return _instance != null;
                }
            }
        }

        protected virtual void Awake()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = this as T;
                    //        DontDestroyOnLoad(gameObject);
                }
                else if (_instance != this)
                {
                    Debug.LogWarning($"Duplicate instance of {typeof(T)} detected. Destroying the new one.");
                    Destroy(gameObject);
                    return;
                }
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}