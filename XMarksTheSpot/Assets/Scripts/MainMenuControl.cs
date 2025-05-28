using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuControl : MonoBehaviour
{

    public string gameSceneName = "gameScene";
    public RectTransform[] panels;

    public AudioMixer mixer;


    void Awake()
    {
        UserSettings.SetAudioMixer(mixer);
    }

    public void StartGame()
    {
        SceneSwitcher.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Game has stopped.");
        Application.Quit();
    }

    public void OpenPanel(RectTransform panelToOpen)
    {
        foreach (RectTransform panel in panels)
        {
            if (panel != panelToOpen && panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(false);
        }
        panelToOpen.gameObject.SetActive(true);
    }
}
