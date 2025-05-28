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

    private Dictionary<string, float> difficultyValues = new Dictionary<string, float>() {
        { "Easy", 0.5f },
        { "Normal", 1.0f },
        { "Hard", 1.5f }
    };

    public Button saveButton;
    public HorizontalLayoutGroup difficultyButtons;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Default Colors")]
    public ColorBlock defaultButtonColor;
    [Header("Pressed Colors")]
    public ColorBlock pressedButtonColor;

    public void LoadSavedData()
    {
        difficultyMultiplier = UserSettings.GetDifficultyMultiplier();
        masterVolume = UserSettings.GetMasterVolume();
        musicVolume = UserSettings.GetMusicVolume();
        sfxVolume = UserSettings.GetSFXVolume();
        saveButton.gameObject.SetActive(false);
    }

    public void UpdateDisplay()
    {
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        UpdateDifficultyButtons();
    }

    void Awake()
    {
        LoadSavedData();
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void SetMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void SetSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        saveButton.gameObject.SetActive(true);
    }

    public void SetDifficultyMultiplier(float newDifficultyMultiplier)
    {
        difficultyMultiplier = newDifficultyMultiplier;
        UpdateDifficultyButtons();
        saveButton.gameObject.SetActive(true);
    }
    
    private void UpdateDifficultyButtons()
    {
        foreach (Button button in difficultyButtons.GetComponentsInChildren<Button>())
        {
            bool pressed = (difficultyMultiplier == difficultyValues.GetValueOrDefault(button.name));
            button.colors = pressed ? pressedButtonColor : defaultButtonColor;
        }
    }

    public void SaveSettings()
    {
        UserSettings.SetDifficultyMultiplier(difficultyMultiplier);
        UserSettings.SetMasterVolume(masterVolume);
        UserSettings.SetMusicVolume(musicVolume);
        UserSettings.SetSFXVolume(sfxVolume);
        saveButton.gameObject.SetActive(false);
    }

}
