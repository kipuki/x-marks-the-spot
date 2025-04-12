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

    private static float convertToDB(float volume)
    {
        return (volume*80)-80;
    }

    public static void setAudioMixer(AudioMixer mixer)
    {
        UserSettings.mixer = mixer;
        Debug.Log("Mixer set.");
    }


    public static void setMasterVolume(float masterVolume)
    {
        UserSettings.masterVolume = masterVolume;
        mixer.SetFloat("Master", convertToDB(masterVolume));
    }

    public static void setMusicVolume(float musicVolume)
    {
        UserSettings.musicVolume = musicVolume;
        mixer.SetFloat("Music", convertToDB(musicVolume));
    }

    public static void setSFXVolume(float sfxVolume)
    {
        UserSettings.sfxVolume = sfxVolume;
        mixer.SetFloat("SFX", convertToDB(sfxVolume));
    }

    public static void setDifficultyMultiplier(float difficultyMultiplier)
    {
        UserSettings.difficultyMultiplier = difficultyMultiplier;
    }

    public static float getMasterVolume()
    {
        return masterVolume;
    }

    public static float getMusicVolume()
    {
        return musicVolume;
    }

    public static float getSFXVolume()
    {
        return sfxVolume;
    }

    public static float getDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }
}
