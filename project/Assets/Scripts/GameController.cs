using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;

    public GameObject player;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public static event Action OnReset;

    Vector3 currentLevelPosition;

    void Start()
    {
        progressAmount = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        Portal.OnPortalEntered += LoadLevel;
        PlayerHealth.OnPlayerDied += Restart;
    }

    void Restart()
    {
        MusicManager.PlayBackgroundMusic(true);
        LoadLevel(currentLevelIndex, currentLevelPosition);
        OnReset.Invoke();
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
    }

    public void LoadLevel(int level, Vector3 position)
    {
        levels[currentLevelIndex].gameObject.SetActive(false); 
        levels[level].gameObject.SetActive(true);

        currentLevelPosition = position;
        player.transform.position = position;

        currentLevelIndex = level;
        progressAmount = 0;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, Vector3.zero);
    }
}
