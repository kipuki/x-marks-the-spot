using UnityEngine.SceneManagement;

public static class SceneSwitcher
{
    public static void loadScene(Scene scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        loadScene(scene.name, loadSceneMode);
    }

    public static void loadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
    
}
