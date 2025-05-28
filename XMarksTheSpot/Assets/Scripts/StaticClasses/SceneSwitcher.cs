using UnityEngine.SceneManagement;

public static class SceneSwitcher
{
    public static void LoadScene(Scene scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        LoadScene(scene.name, loadSceneMode);
    }

    public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
    
}
