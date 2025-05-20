using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float difficultyMultiplier = 1f;

    private Dictionary<string, float> difficultyValues = new Dictionary<string, float>();

    public Button saveButton;
    public HorizontalLayoutGroup difficultyButtons;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Default Colors")]
    public ColorBlock defaultButtonColor;
    [Header("Pressed Colors")]
    public ColorBlock pressedButtonColor;

    public void loadSavedData()
    {
        difficultyMultiplier = UserSettings.getDifficultyMultiplier();
        masterVolume = UserSettings.getMasterVolume();
        musicVolume = UserSettings.getMusicVolume();
        sfxVolume = UserSettings.getSFXVolume();
        updateDisplay();
        saveButton.gameObject.SetActive(false);
    }

    public void updateDisplay()
    {
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        updateDifficultyButtons();
    }

    void Awake()
    {
        difficultyValues.Add("Easy", 0.5f);
        difficultyValues.Add("Normal", 1.0f);
        difficultyValues.Add("Hard", 1.5f);
        loadSavedData();
    }

    void Start()
    {
        updateDisplay();
    }

    public void setMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void setMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void setSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void setDifficultyMultiplier(Button difficultyButton)
    {
        this.difficultyMultiplier = difficultyValues.GetValueOrDefault(difficultyButton.name);
        updateDifficultyButtons();
        Debug.Log("Changed difficulty to "+difficultyButton.name);
        saveButton.gameObject.SetActive(true);
    }
    
    private void updateDifficultyButtons()
    {
        foreach (Button button in difficultyButtons.GetComponentsInChildren<Button>())
        {
            bool pressed = (difficultyMultiplier == difficultyValues.GetValueOrDefault(button.name));
            button.colors = pressed ? pressedButtonColor : defaultButtonColor;
        }
    }

    public void saveSettings()
    {
        UserSettings.setDifficultyMultiplier(difficultyMultiplier);
        UserSettings.setMasterVolume(masterVolume);
        UserSettings.setMusicVolume(musicVolume);
        UserSettings.setSFXVolume(sfxVolume);
        saveButton.gameObject.SetActive(false);
    }

}
