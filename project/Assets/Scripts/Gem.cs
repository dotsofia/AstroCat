using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public static event Action<int> OnGemCollect;
    public int skill = 0;
    public void Collect()
    {
        OnGemCollect.Invoke(skill);
        SoundEffectManager.Play("Gem");
        Destroy(gameObject);
    }
}
