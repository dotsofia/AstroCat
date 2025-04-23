using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> instructions;
    public GameObject optionsMenu;
    bool isActive = false;
    int currentInstruction = 0;

    void Start()
    {
        ShowInstruction(currentInstruction);
    }

    void ShowInstruction(int index)
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
        isActive = !isActive;
        optionsMenu.SetActive(isActive);

        Time.timeScale = isActive ? 0f : 1f;
    }

    public void NextInstruction()
    {
        HideInstruction(currentInstruction++);
        ShowInstruction(currentInstruction);
    }
}