using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    private int currentProgress = 0;

    public bool CanEnd { get; private set; } = false;

    public static event Action OnReset;

    Vector3 currentLevelPosition;
    public DialogueTrigger dialogueTrigger;

    void Start()
    {
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        Portal.OnPortalEntered += LoadLevel;
        PlayerHealth.OnPlayerDied += Restart;
        Part.OnPartCollect += Progress;

        dialogueTrigger.TriggerDialogue();
    }

    void Progress()
    {
        currentProgress++;

        if (currentProgress >= 3)
        {
            CanEnd = true;
        }
    }
    void Restart()
    {
        MusicManager.PlayBackgroundMusic(true);
        LoadLevel(currentLevelIndex, currentLevelPosition);
        OnReset.Invoke();
    }

    public void LoadLevel(int level, Vector3 position)
    {
        levels[currentLevelIndex].gameObject.SetActive(false); 
        levels[level].gameObject.SetActive(true);

        currentLevelPosition = position;
        player.transform.position = position;

        currentLevelIndex = level;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, Vector3.zero);
    }
}
