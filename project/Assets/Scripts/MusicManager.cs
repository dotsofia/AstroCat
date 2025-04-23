using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    void Awake()
    {   
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }
        else
        {
            Slider volumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
    }

    public static void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Instance.audioSource.volume = volume;
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            Instance.audioSource.clip = audioClip;
        }
        if (Instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                Instance.audioSource.Stop();
            }
            Instance.audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        Instance.audioSource.Pause();
    }
}
