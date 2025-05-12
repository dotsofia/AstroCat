using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public bool CanPause { get; set; } = true;
    public Animator animator;
    public GameObject player;
    public List<GameObject> instructions;
    public GameObject tutorial;
    public GameObject optionsMenu;
    
    bool isActive = false;
    int currentInstruction = 0;


    public void ShowInstruction(int index)
    {
        instructions[index].SetActive(true);
    }

    void HideInstruction(int index)
    {
        instructions[index].SetActive(false);
    }

    void Update()
    {

    }
    public void Pause()
    {
        if (!CanPause) return;
        
        isActive = !isActive;
        tutorial.SetActive(!isActive);
        animator.SetBool("IsOpen", isActive);

        // Time.timeScale = isActive ? 0f : 1f;
    }

    public void NextInstruction()
    {
        Debug.Log(instructions[currentInstruction + 1]);
        HideInstruction(currentInstruction++);
        ShowInstruction(currentInstruction);
    }
}