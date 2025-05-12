using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    public UIController UI;

    private bool firstDialogue = true;
    private bool winDialogue = false;
    private bool quitDialogue = false;

    public static event Action OnWinDialogueComplete;
    public static event Action OnQuitDialogueComplete;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        quitDialogue = dialogue.isQuitDialogue;
        winDialogue = dialogue.isWinDialogue;

        UI.CanPause = false;

        animator.SetBool("IsOpen", true);
        dialogueText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        UI.CanPause = true;

        animator.SetBool("IsOpen", false);

        if (firstDialogue)
        {
            firstDialogue = false;
            UI.ShowInstruction(0);
        }

        if (quitDialogue) OnQuitDialogueComplete.Invoke();
        if (winDialogue) OnWinDialogueComplete.Invoke();
    }
}
