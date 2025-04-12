using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class mainMenuControl : MonoBehaviour
{

    public string gameSceneName = "gameScene";
    public RectTransform[] panels;

    public AudioMixer mixer;


    void Awake()
    {
        UserSettings.setAudioMixer(mixer);
        // SceneSwitcher.loadScene(gameScene, LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start()
    {
        // SceneManager.GetSceneByName("gameScene");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneSwitcher.loadScene(gameSceneName);
    }

    public void quitGame()
    {
        Debug.Log("Game has stopped.");
        Application.Quit();
    }

    public void openPanel(RectTransform panelToOpen)
    {
        foreach (RectTransform panel in panels)
        {
            if (panel != panelToOpen && panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(false);
        }
        panelToOpen.gameObject.SetActive(true);
    }
}
