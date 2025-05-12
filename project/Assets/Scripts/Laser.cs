using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserEffect; // Assign LaserEffect in Inspector
    public bool isActive = true;
    public float interval = 1.0f;

    void OnEnable()
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

        if (laserEffect != null)
        {
            laserEffect.SetActive(isActive);
        }
    }

    void OnDisable()
    {
        CancelInvoke("TimedLaser");
    }
}
