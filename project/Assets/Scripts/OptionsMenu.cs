using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject confirmationScreen;
    public DialogueTrigger dialogueTrigger;
    public UIController UI;

    void Start()
    {
        DialogueManager.OnQuitDialogueComplete += ShowConfirmationScreen;
    }

    public void Quit()
    {
        dialogueTrigger.TriggerDialogue();
    }

    public void ShowConfirmationScreen()
    {
        confirmationScreen.SetActive(true);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Return()
    {
        confirmationScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
