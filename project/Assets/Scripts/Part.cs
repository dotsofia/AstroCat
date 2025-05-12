using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour, IItem
{
    public static event Action OnPartCollect;
    public void Collect()
    {
        OnPartCollect.Invoke();
        Destroy(gameObject);
    }
}
