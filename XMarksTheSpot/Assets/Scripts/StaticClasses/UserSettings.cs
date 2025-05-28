using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public static class UserSettings
{
    private static float masterVolume = 1f;
    private static float musicVolume = 1f;
    private static float sfxVolume = 1f;
    private static float difficultyMultiplier = 1f;

    private static AudioMixer mixer;

    private static float ConvertToDB(float volume)
    {
        return (volume*80)-80;
    }

    public static void SetAudioMixer(AudioMixer mixer)
    {
        UserSettings.mixer = mixer;
        Debug.Log("Mixer set.");
    }


    public static void SetMasterVolume(float masterVolume)
    {
        UserSettings.masterVolume = masterVolume;
        mixer.SetFloat("Master", ConvertToDB(masterVolume));
    }

    public static void SetMusicVolume(float musicVolume)
    {
        UserSettings.musicVolume = musicVolume;
        mixer.SetFloat("Music", ConvertToDB(musicVolume));
    }

    public static void SetSFXVolume(float sfxVolume)
    {
        UserSettings.sfxVolume = sfxVolume;
        mixer.SetFloat("SFX", ConvertToDB(sfxVolume));
    }

    public static void SetDifficultyMultiplier(float difficultyMultiplier)
    {
        UserSettings.difficultyMultiplier = difficultyMultiplier;
    }

    public static float GetMasterVolume()
    {
        return masterVolume;
    }

    public static float GetMusicVolume()
    {
        return musicVolume;
    }

    public static float GetSFXVolume()
    {
        return sfxVolume;
    }

    public static float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }
}
