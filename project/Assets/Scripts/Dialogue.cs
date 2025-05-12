using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextAreaAttribute(3, 10)]
    public string[] sentences;

    public bool isQuitDialogue;
    public bool isWinDialogue;
}
