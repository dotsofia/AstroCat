using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
            PlayerPrefs.SetFloat("SFXVolume", 1f);
        }
        else
        {
            Slider volumeSlider = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>();
            volumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);

        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    public static void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        audioSource.volume = volume;
    }
}
