using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInteractable
{
    GameController gc;

    public DialogueTrigger dialogueTrigger;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
        DialogueManager.OnWinDialogueComplete += Quit;
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public bool CanInteract()
    {
        return gc.CanEnd;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        dialogueTrigger.TriggerDialogue();
    }
}
