using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isActive = true;
    public float interval = 1.0f;

    void Start()
    {
        InvokeRepeating("TimedLaser", 0f, interval);
    }
    void TimedLaser()
    {
        isActive = !isActive;

        if (isActive)
        {
            SoundEffectManager.Play("Laser");
        }
        
        gameObject.SetActive(isActive);
    }
}
