using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    public int level = 1;
    public Vector3 position;

    public static event Action<int, Vector3> OnPortalEntered;

    public bool automatic = false;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        Teleport();
    }

    private void Teleport()
    {
        OnPortalEntered.Invoke(level, position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (automatic)
        {
            OnPortalEntered.Invoke(level, position);
        }
    }
}
