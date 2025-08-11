using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneSetup
{
    [SerializeField] private string _sceneTag;
    [SerializeField] private string _sceneName;
    [SerializeField] private LoadSceneMode _sceneMode;

    public string SceneTag => _sceneTag;
    public string SceneName => _sceneName;
    public LoadSceneMode SceneMode => _sceneMode;
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneSetup mainSceneSetup;
    [SerializeField] private SceneSetup cleanupSceneSetup;

    private void OnEnable()
    {
        GameEvents.OnRestartGame += Handle_RestartGame;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void OnDisable()
    {
        GameEvents.OnRestartGame -= Handle_RestartGame;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
    }

    private void Handle_RestartGame()
    {
        SceneManager.LoadScene(cleanupSceneSetup.SceneName, LoadSceneMode.Additive);
    }

    private void SceneManager_sceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
    {
        if (loadedScene.name == cleanupSceneSetup.SceneName)
        {
            // Cleanup scene has been loaded, now unload the main scene
            SceneManager.UnloadSceneAsync(mainSceneSetup.SceneName);
        }
        else if (loadedScene.name == mainSceneSetup.SceneName && loadMode == LoadSceneMode.Additive)
        {
            // New main scene has been loaded, now unload the cleanup scene
            SceneManager.UnloadSceneAsync(cleanupSceneSetup.SceneName);
        }
    }

    private void SceneManager_sceneUnloaded(Scene unloadedScene)
    {
        if (unloadedScene.name == mainSceneSetup.SceneName)
        {
            // Main scene has been unloaded, now reload it additively
            SceneManager.LoadScene(mainSceneSetup.SceneName, LoadSceneMode.Additive);
        }
    }
}